using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortCutes
{
    public partial class ShortCutes : Form
    {
        readonly private string temppath = Path.GetTempPath() + @"\ShortCutes\";
        readonly private string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Shortcutes\";
        readonly private Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]");
        readonly private string InvalidFileNameChars = "";
        //Emuthings
        private List<Emulator> EmulatorsList => Emulators.EmulatorsList;
        private Emulator SelectedEmu => EmulatorsList[emulatorcb.SelectedIndex];

        public ShortCutes()
        {
            InitializeComponent();

            foreach (var emu in EmulatorsList)
                emulatorcb.Items.Add(emu.Name);

            foreach (char ch in Path.GetInvalidFileNameChars())
                if (!char.IsWhiteSpace(ch) && !char.IsControl(ch))
                    InvalidFileNameChars += ch + " ";

            if (!Directory.Exists(temppath.Remove(temppath.Length - 1)))
                Directory.CreateDirectory(temppath.Remove(temppath.Length - 1));
            if (!Directory.Exists(appdata.Remove(appdata.Length - 1)))
                Directory.CreateDirectory(appdata.Remove(appdata.Length - 1));

            if (File.Exists(appdata + @"squaredesign"))
                RectangularDesign = false;

            Properties.Resources.loading.Save(temppath + @"loading.gif");

            //Extracting XCI-Explorer, which I don't own and only has a little modification || Original Reporsitory: https://github.com/StudentBlake/XCI-Explorer
            //Still thinking if necessary to implement
            /*if (!File.Exists(appdata + @"XCI-Explorer\XCI-Explorer.exe"))
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
            }*/
            //MessageBox.Show("");
            /*Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            MessageBox.Show((string)Resources.GetObject("Prueba"));*/
        }

        private void ShortCutes_Shown(object sender, EventArgs e)
        {
            EZ_Updater.CheckUpdate(this, AskForUpdate, "Haruki1707/ShortCutes");
        }

        private void AskForUpdate()
        {
            if (Success("New version available\nDo you want to update?"))
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

            if (!string.IsNullOrWhiteSpace(Edirbox.Text) && Directory.Exists(Edirbox.Text) && !Directory.Exists(Edirbox.Text + @"ShortCutes"))
            {
                Directory.CreateDirectory(Edirbox.Text + @"ShortCutes");
                Info("To avoid Anti-Virus problems with ShortCutes please exclude this path folder: " + Edirbox.Text + "ShortCutes");
            }

            Shortcutbox.Focus();
        }

        private bool Emulatorcb_HasSelectedItem()
        {
            return emulatorcb.SelectedItem != null;
        }

        private void CreateShortCute_Click(object sender, EventArgs e)
        {
            string code;

            if (!Edirbox.Text.EndsWith(@"\") && !string.IsNullOrWhiteSpace(Edirbox.Text))
                Edirbox.Text += @"\";

            if (!Emulatorcb_HasSelectedItem())
                Error("Emulator must be selected!");
            else if (string.IsNullOrWhiteSpace(Shortcutbox.Text))
                Error("Shortcut name cannot be empty");
            else if (!File.Exists(Edirbox.Text + SelectedEmu.Exe))
                Error("Emulator doesn't exist in the specified path\nCheck if the path or the selected emulator is correct");
            else if (!File.Exists(Gdirbox.Text))
                Error("Game file doesn't exist in the specified path");
            else if (!Image)
                Error("Select a picture to continue");
            else
            {
                if (Gdirbox.Text.Contains(Edirbox.Text, StringComparison.OrdinalIgnoreCase))
                    code = Roslyn_FormCode(Gdirbox.Text.Replace(Edirbox.Text, @""));
                else if (Success("Emulator and games' folder must be on the same path to avoid issues.\n\nWant to continue without the same path? (still works)"))
                    code = Roslyn_FormCode(Gdirbox.Text);
                else
                    return;

                Compile(code, Edirbox.Text, Shortcutbox.Text);
                if (OpenShortFolderCheck.Checked)
                    Process.Start("explorer.exe", Edirbox.Text + @"ShortCutes");

                Image = false;
                ICOpic.Image = null;
                Gdirbox.Text = Shortcutbox.Text = null;
            }
            Shortcutbox.Focus();
        }

        private void Compile(string code, string emupath, string Filename)
        {
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
            CompilerResults results = new CSharpCodeProvider().CompileAssemblyFromSource(parameters, code);

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

        private string Roslyn_FormCode(string gamedir)
        {
            string code = Properties.Resources.Roslyn_Form_Code;

            string size = "256";
            if (RectangularDesign)
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

            if (File != null)
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
                    Info("The emulator isn't supported yet. You can contribute to make it compatible on GitHub (Haruki1707/ShortCutes repo)" +
                        "\n\n!!!This also may occur because you changed the emulator executable name." +
                        "Make sure you are using the original emulator name!!!");
                }
            }

            Shortcutbox.Focus();
        }

        private void GameBrow_Click(object sender, EventArgs e)
        {
            string GamesPath = "C:\\";

            if (Emulatorcb_HasSelectedItem())
            {
                if (SelectedEmu.TryGetGamesPath() != "" && SelectedEmu.GamesPath != null)
                    GamesPath = SelectedEmu.GamesPath;
                else if (!string.IsNullOrWhiteSpace(Gdirbox.Text) && Directory.Exists(Path.GetDirectoryName(Gdirbox.Text)))
                    GamesPath = Path.GetDirectoryName(Gdirbox.Text);
                else if (Edirbox.Text != "")
                    GamesPath = Edirbox.Text;
                
                var File = FileDialog(GamesPath, SelectedEmu.Gamesfilters);

                if (File != null)
                    Gdirbox.Text = File;
            }
            else
                Info("Emulator must be selected!");

            Shortcutbox.Focus();
        }

        private static bool Image = false;
        bool clicked;
        private async void ICOpic_MouseClick(object sender, MouseEventArgs e)
        {
            if (clicked) return;
            clicked = true;
            await Task.Delay(SystemInformation.DoubleClickTime);
            if (!clicked) return;
            clicked = false;

            //Process click
            var file = FileDialog(Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads"), "PNG/JPG Image (*.png; *.jpg; *.jpeg)|*.png;*.jpg;*.jpeg");

            if (file != null)
            {
                File.Copy(file, temppath + "tempORIGINAL.png", true);
                ImagingHelper.ConvertToIcon(file, temppath + @"temp.ico");
                ICOpic.Image = ImagingHelper.ICONbox;
                ICOpic.Image.Save(temppath + @"temp.png");
                Image = true;
            }

            Shortcutbox.Focus();
        }

        private void ICOpic_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clicked = false;

            //Process click
            if (Image)
                using (var CI = new CropImage_Tool())
                {
                    CI.ShowDialog();
                    if(CI.DialogResult1 == DialogResult.Yes)
                    {
                        ImagingHelper.ConvertToIcon(temppath + "temp.png", temppath + @"temp.ico");
                        ICOpic.Image = ImagingHelper.ICONbox;
                    }
                }
            else
                Info("First select a picture to crop");
        }

        private void ICOurl_TextChanged(object sender, EventArgs e)
        {
            if (urltext != null && !string.IsNullOrWhiteSpace(ICOurl.Text))
            {
                try
                {
                    using (Stream stream = new WebClient().OpenRead(ICOurl.Text))
                    using (Bitmap bitmap = new Bitmap(stream))
                    {
                        if (bitmap != null)
                        {
                            bitmap.Save(temppath + @"temp.png");
                            File.Copy(temppath + "temp.png", temppath + "tempORIGINAL.png", true);
                            ImagingHelper.ConvertToIcon(temppath + @"temp.png", temppath + @"temp.ico");
                            ICOpic.Image = ImagingHelper.ICONbox;
                            ICOpic.Image.Save(temppath + @"temp.png");
                            Image = true;
                        }
                    }
                }
                catch
                {
                    Error("URL provided isn't an image...");
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
            InputIsCommand = e.KeyCode == Keys.V && e.Modifiers == Keys.Control;
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

                if (design.DialogResult == DialogResult.No)
                {
                    if (File.Exists(appdata + @"squaredesign"))
                        File.Delete(appdata + @"squaredesign");
                    RectangularDesign = true;
                }
                else if (design.DialogResult == DialogResult.Yes)
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
            if (InputIsCommand && containsABadCharacter.IsMatch(Shortcutbox.Text))
            {
                Error("Invalid filename!\n Cannot contain: " + InvalidFileNameChars);
                Shortcutbox.Text = Regex.Replace(Shortcutbox.Text, containsABadCharacter.ToString(), "");
            }
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
            using (var info = new MessageForm(message, 0))
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

        private void InfoButton_Click(object sender, EventArgs e)
        {
            using (var info = new MessageForm("ShortCutes  v" + EZ_Updater.ActualVersion + "\n\nDeveloped by: Haruki1707\nGitHub: https://github.com/Haruki1707/ShortCutes", 5))
                info.ShowDialog();
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