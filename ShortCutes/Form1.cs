using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;
using System.Text.RegularExpressions;

using System.Net;
using System.Threading;

namespace ShortCutes
{
    public partial class ShortCutes : Form
    {
        readonly private string temppath = Path.GetTempPath();
        readonly private string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Shortcutes\";
        private Stream AssemblyResource(string resource) => System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ShortCutes.Resources." + resource);
        readonly private Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]");
        readonly private string InvalidFileNameChars = "";
        //Emuthings
        private List<Emulator> EmulatorsList => Emulators.EmulatorsList;
        private Emulator SelectedEmu => EmulatorsList[emulatorcb.SelectedIndex];

        public ShortCutes()
        {
            InitializeComponent();

            foreach(var emu in EmulatorsList)
                emulatorcb.Items.Add(emu.Name);
            emulatorcb.SelectedIndex = 0;

            foreach (char ch in Path.GetInvalidFileNameChars())
                if (!char.IsWhiteSpace(ch) && !char.IsControl(ch))
                    InvalidFileNameChars += ch + " ";

            if(!Directory.Exists(appdata.Remove(appdata.Length - 1)))
                Directory.CreateDirectory(appdata.Remove(appdata.Length - 1));

            if (File.Exists(appdata + @"squaredesign"))
                RectangularDesign = false;

            using (Stream stream = AssemblyResource("loading.gif"))
            using (Bitmap bitmap = new Bitmap(stream))
                bitmap.Save(temppath + @"loading.gif");

            //Extracting XCI-Explorer, which I don't own and only has a little modification || Original Reporsitory: https://github.com/StudentBlake/XCI-Explorer
            if (!File.Exists(appdata + @"XCI-Explorer\XCI-Explorer.exe"))
            {
                using (Stream stream = AssemblyResource("XCI-Explorer.zip"))
                using (FileStream bw = new FileStream(appdata + @"XCI-Explorer.zip", FileMode.Create))
                {
                    while (stream.Position < stream.Length)
                    {
                        byte[] bits = new byte[stream.Length];
                        stream.Read(bits, 0, (int)stream.Length);
                        bw.Write(bits, 0, (int)stream.Length);
                    }
                }

                System.IO.Compression.ZipFile.ExtractToDirectory(appdata + @"XCI-Explorer.zip", appdata + @"XCI-Explorer");
                File.Delete(appdata + @"XCI-Explorer.zip");
            }            
        }

        private void ShortCutes_Shown(object sender, EventArgs e)
        {
            EZ_Updater.CheckUpdate(this, AskForUpdate, "Haruki1707/ShortCutes");
        }

        private void AskForUpdate()
        {
            if(Success("There is a new version, wanna update?"))
            {
                var FD = new MessageForm("", 4);
                FD.ShowDialog();
            }
        }

        private void Emulatorcb_SelectedIndexChanged(object sender, EventArgs e)
        {
            Edirbox.Text = Gdirbox.Text = null;
            label6.Text = SelectedEmu.Description;
            label6.ForeColor = SelectedEmu.Cdesc;
            Edirbox.Text = SelectedEmu.Path();

            Shortcutbox.Focus();
        }

        private void CreateShortCute_Click(object sender, EventArgs e)
        {
            string emulatorpath = Edirbox.Text;
            if (!emulatorpath.EndsWith("\\"))
                emulatorpath += @"\";

            string code = Emulator(Gdirbox.Text, emulatorpath);

            if (code == "false")
            {
                Shortcutbox.Focus();
                return;
            }
            if (!Image)
            {
                Error("Select a picture to continue");
                return;
            }
            if (string.IsNullOrWhiteSpace(Shortcutbox.Text))
            {
                Error("Shortcut name cannot be empty");
                return;
            }

            Compile(code, emulatorpath, Shortcutbox.Text);
            if (OpenShortFolderCheck.Checked)
                Process.Start("explorer.exe", emulatorpath + @"ShortCutes");

            Image = false;
            ICOpic.Image = null;
            Gdirbox.Text = Shortcutbox.Text = null;
            Edirbox.Text = emulatorpath;
            Shortcutbox.Focus();
            return;
        }

        private void Compile(string code, string emupath, string Filename)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();

            emupath += @"ShortCutes";
            if (!Directory.Exists(emupath))
                Directory.CreateDirectory(emupath);
            emupath += @"\";

            string Output = emupath + Filename + ".exe";

            CompilerParameters parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.dll", "System.Windows.Forms.dll", "System.Drawing.dll" })
            {
                CompilerOptions = "-win32icon:" + temppath + "temp.ico \n -target:winexe " +
                    "\n -resource:" + temppath + @"temp.png" +
                    "\n -resource:" + temppath + @"loading.gif",
                GenerateExecutable = true,
                OutputAssembly = Output
            };
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.Count > 0)
            {
                string errors = null;
                foreach (CompilerError CompErr in results.Errors)
                {
                    errors = errors +
                                "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine;
                }
                Error(errors);
                return;
            }

            if (DesktopCheck.Checked)
            {
                object shDesktop = (object)"Desktop";
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\" + Filename + ".lnk";
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutAddress);
                shortcut.Description = "ShortCute for " + Filename;
                shortcut.TargetPath = Output;
                shortcut.WorkingDirectory = emupath;
                shortcut.Save();
            }

            if (Success("Shortcut created!\nExecute shortcut?"))
            {
                var starto = new Process();
                starto.StartInfo.FileName = Output;
                starto.StartInfo.WorkingDirectory = emupath;
                starto.Start();
            }
        }

        private string Emulator(string gamedir, string emulatordir)
        {
            string gamechecker = gamedir;

            if (gamedir.Contains(emulatordir, StringComparison.OrdinalIgnoreCase))
            {
                gamedir = gamedir.Replace(emulatordir, @"");
                gamedir = gamedir.Replace(@"\", @"\\");
            }
            else
            {
                if (!System.IO.File.Exists(emulatordir + SelectedEmu.Exe))
                {
                    Error("Emulator doesn't exist in the specified path\nCheck if path or selected emulator is correct");
                    return "false";
                }
                else if (!System.IO.File.Exists(gamechecker))
                {
                    Error("Game file doesn't exist in the specified path");
                    return "false";
                }
                else if (Success("Emulator and games folder must be on the same path for better working.\n\n" +
                        "Wanna continue without the same path? (still works)"))
                    gamedir = gamedir.Replace(@"\", @"\\");
                else
                    return "false";
            }

            string code = null;
            using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ShortCutes.Roslyn Form Code.cs"))
            using (StreamReader reader = new StreamReader(stream))
                code = reader.ReadToEnd();

            string size = "256";
            if (RectangularDesign == true)
                size = "322";

            code = code.Replace("%HEIGHT%", size);
            code = code.Replace("%WAITCHANGE%", SelectedEmu.WaitWindowChange.ToString().ToLower());
            code = code.Replace("%EMUNAME%", SelectedEmu.Name);
            code = code.Replace("%GAME%", Shortcutbox.Text);
            code = code.Replace("%EMULATOR%", SelectedEmu.Exe);
            code = code.Replace("%ARGUMENTS%", SelectedEmu.Arguments(gamedir));

            return code;
        }

        private void EmuBrow_Click(object sender, EventArgs e)
        {
            string EmuDir = "C:\\";
            if (Edirbox.Text != "")
                EmuDir = Edirbox.Text;

            var File = FileDialog(EmuDir, "Executable File (*.exe)|*.exe");

            if(File != null)
            {
                bool exists = false;
                foreach (var emu in EmulatorsList)
                {
                    if (emu.Exe.ToLower() == Path.GetFileName(File).ToLower())
                    {
                        emu.Path(Path.GetDirectoryName(File) + @"\");
                        emulatorcb.SelectedIndex = EmulatorsList.IndexOf(emu);
                        Emulatorcb_SelectedIndexChanged(null, null);
                        exists = true;
                        break;
                    }
                }

                if (exists == false)
                {
                    Info("The emulator is not yet supported. You can contribute to make it compatible on GitHub (Haruki1707/ShortCutes repo)" +
                        "\n\n!!!Also, this could appear because you changed the emulator executable name." +
                        "Make sure you are using the original emulator name!!!");
                }
            }

            Shortcutbox.Focus();
        }

        private void GameBrow_Click(object sender, EventArgs e)
        {
            string GamesPath = "C:\\";

            if (SelectedEmu.Games() != "" && SelectedEmu.Games(true) != null)
                GamesPath = SelectedEmu.Games(true);
            else if (Edirbox.Text != "")
                GamesPath = Edirbox.Text;

            var File = FileDialog(GamesPath, SelectedEmu.Gamesfilters);

            if (File != null)
                Gdirbox.Text = File;

            Shortcutbox.Focus();
        }

        private static bool Image = false;
        private void ICOpic_Click(object sender, EventArgs e)
        {
            var File = FileDialog(Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads"), "PNG/JPG Image (*.png; *.jpg; *.jpeg)|*.png;*.jpg;*.jpeg");
            
            if (File != null)
            {
                ImagingHelper.ConvertToIcon(File, temppath + @"temp.ico");
                ICOpic.Image = ImagingHelper.ICONbox;
                ICOpic.Image.Save(temppath + @"temp.png");
                Image = true;
            }

            Shortcutbox.Focus();
        }

        private void ICOurl_TextChanged(object sender, EventArgs e)
        {
            if(urltext != null && !string.IsNullOrWhiteSpace(ICOurl.Text))
            {
                try
                {
                    using (Stream stream = new WebClient().OpenRead(ICOurl.Text))
                    using (Bitmap bitmap = new Bitmap(stream))
                    {
                        if (bitmap != null)
                        {
                            bitmap.Save(temppath + @"temp.png");
                            ImagingHelper.ConvertToIcon(temppath + @"temp.png", temppath + @"temp.ico");
                            ICOpic.Image = ImagingHelper.ICONbox;
                            ICOpic.Image.Save(temppath + @"temp.png");
                            Image = true;
                        }
                    }
                }
                catch
                {
                    Error("URL is not an image...");
                }
                Shortcutbox.Focus();
            }
        }

        private string FileDialog(string InitialDir, string Filter)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = InitialDir;
                dialog.Filter = Filter;
                dialog.FilterIndex = 1;

                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
            }
        }

        //UI things not that important
        bool InputIsCommand = false;
        private void ICOurl_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !InputIsCommand;
        }

        private void ICOurl_KeyDown(object sender, KeyEventArgs e)
        {
            InputIsCommand = e.Control == true && (e.KeyCode == Keys.V);
        }

        string urltext;
        private void ICOurl_Click(object sender, EventArgs e)
        {
            urltext = ICOurl.Text;
            ICOurl.Text = null;
        }
        private void ICOurl_Leave(object sender, EventArgs e)
        {
            var text = urltext;
            urltext = null;
            ICOurl.Text = text;
        }

        private void ShortCutes_Paint(object sender, PaintEventArgs e)
        {
            Program.ToDraw(this.Controls, e);
        }
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void MiniBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        bool RectangularDesign = true;
        private void ConfigBtn_Click(object sender, EventArgs e)
        {
            using (var design = new MessageForm(RectangularDesign.ToString(), 3))
            {
                design.ShowDialog();

                if(design.DialogResult == DialogResult.No)
                {
                    if (File.Exists(appdata + @"squaredesign"))
                        File.Delete(appdata + @"squaredesign");
                    RectangularDesign = true;
                }
                else if(design.DialogResult == DialogResult.Yes)
                {
                    File.Create(appdata + @"squaredesign").Close();
                    RectangularDesign = false;
                }
            }
        }

        private void Shortcutbox_Focus(object sender, EventArgs e)
        {
            Shortcutbox.Focus();
        }
        private void Shortcutbox_TextChanged(object sender, EventArgs e)
        {
            if (TextRenderer.MeasureText(Shortcutbox.Text, Shortcutbox.Font).Width > Shortcutbox.Width)
                Shortcutbox.Font = new Font(Shortcutbox.Font.FontFamily, Shortcutbox.Font.Size - 1);
            else if (Shortcutbox.Font.Size < 12 && TextRenderer.MeasureText(Shortcutbox.Text, new Font(Shortcutbox.Font.FontFamily, Shortcutbox.Font.Size + 1)).Width < Shortcutbox.Width)
                Shortcutbox.Font = new Font(Shortcutbox.Font.FontFamily, Shortcutbox.Font.Size + 1);
        }
        private void Shortcutbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (containsABadCharacter.IsMatch(e.KeyChar.ToString()) && !Char.IsControl(e.KeyChar))
            {
                Error("Invalid filename!\n Cannot contain: " + InvalidFileNameChars);
                e.Handled = true;
            }
        }

        //Let the form to be moved
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void FormDisp_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Info(string message)
        {
            using(var info = new MessageForm(message, 0))
                info.ShowDialog();
        }
        private void Error(string message)
        {
            using (var error = new MessageForm(message, 1))
                error.ShowDialog();
        }
        private bool Success(string message)
        {
            using (var success = new MessageForm(message, 2))
            {
                success.ShowDialog();
                return success.DialogResult == DialogResult.Yes;
            }
        }
    }

    public static class StringExtensions
    {
        public static bool Contains(this String str, String substr, StringComparison cmp)
        {
            if (substr == null)
                throw new ArgumentNullException("substring substring", " cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), cmp))
                throw new ArgumentException("comp is not a member of", "StringComparison, comp");

            return str.IndexOf(substr, cmp) >= 0;
        }
    }
}