using EZ_Updater;
using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Threading;
using Microsoft.CSharp;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ShortCutes
{
    public partial class ShortCutes : Form
    {
        readonly private string temppath = Path.GetTempPath() + @"\ShortCutes\";
        readonly private string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ShortCutes\";
        readonly private Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]");
        readonly private string InvalidFileNameChars = "";
        private bool RectangularDesign = true;
        private bool Emulatorcb_HasSelectedItem => emulatorcb.SelectedItem != null;
        private List<Emulator> EmulatorsList => Emulators.EmulatorsList;
        private Emulator SelectedEmu => EmulatorsList[emulatorcb.SelectedIndex];
        private int SSCH = -1;
        private int SelectedShortCuteHis
        {
            get { return SSCH; }
            set
            {
                SSCH = value; Shortcutbox.Focus();
                if (value == -1)
                {
                    ClearSCSelected.Visible = false;
                    createshortbtn.Text = "Create ShortCute";
                }
                else
                {
                    ClearSCSelected.Visible = true;
                    createshortbtn.Text = "Modify ShortCute";
                }
            }
        }

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

            Bitmap flag = new Bitmap(ICOpic.Width, ICOpic.Height);
            using (Graphics flagGraphics = Graphics.FromImage(flag))
            {
                flagGraphics.DrawString("Click here to select an image", new Font("Bahnschrift SemiBold SemiConden", 18F), Brushes.White, 10, (ICOpic.Height / 2) - (35F));
                flagGraphics.DrawString("(PNG, JPG, JPEG, BMP, TIFF)", new Font("Bahnschrift SemiBold SemiConden", 12F), Brushes.White, 55, (ICOpic.Height / 2) - (22F / 2));
                flagGraphics.DrawString("Double click to crop selected image", new Font("Bahnschrift SemiBold SemiConden", 15F), Brushes.White, 10, (ICOpic.Height / 2) + (22F));
            }
            ICOpic.BackgroundImage = flag;

            OpenFolder.Hide();
        }

        private async void ShortCutes_Shown(object sender, EventArgs e)
        {
            if (await Updater.CheckUpdateAsync("Haruki1707", "ShortCutes")) new MessageForm("", 4).ShowDialog();
        }

        public static void Form1_UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            try
            {
                MessageBox.Show("An application error occurred. Please contact the adminstrator with the following information:\n\n" + t.Exception.Message + "\n\nStack Trace:\n" + t.Exception.StackTrace,
                        "Notify about this error on GitHub repository Haruki1707/ShortCutes", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                if (Updater.CheckUpdate("Haruki1707", "ShortCutes")) new MessageForm("", 4).ShowDialog();
            }
            catch
            {
                try
                {
                    MessageBox.Show("Fatal Windows Forms Error",
                        "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }

        private void Emulatorcb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tempedir = Edirbox.Text;
            Edirbox.Text = null;
            label6.Text = SelectedEmu.Description;
            label6.ForeColor = SelectedEmu.Cdesc;
            Edirbox.Text = SelectedEmu.Path();
            if (tempedir != Edirbox.Text)
                Gdirbox.Text = null;

            if (!string.IsNullOrWhiteSpace(Edirbox.Text) && Directory.Exists(Edirbox.Text) && !Directory.Exists(Edirbox.Text + @"ShortCutes"))
            {
                Directory.CreateDirectory(Edirbox.Text + @"ShortCutes");
                MessageForm.Info("To avoid Anti-Virus problems with ShortCutes please exclude this path folder:\n\n" +
                    Edirbox.Text + "ShortCutes\n\nDouble click on this text to copy path folder to clipboard", Edirbox.Text + "ShortCutes");
            }

            if (Directory.Exists(Edirbox.Text + "ShortCutes"))
                OpenFolder.Show();
            else
                OpenFolder.Hide();

            Shortcutbox.Focus();
        }

        private void CreateShortCute_Click(object sender, EventArgs e)
        {
            string code;

            if (!Edirbox.Text.EndsWith(@"\") && !string.IsNullOrWhiteSpace(Edirbox.Text))
                Edirbox.Text += @"\";

            if (!Emulatorcb_HasSelectedItem)
                MessageForm.Error("Emulator must be selected!");
            else if (string.IsNullOrWhiteSpace(Shortcutbox.Text))
                MessageForm.Error("Shortcut name cannot be empty");
            else if (!File.Exists(Edirbox.Text + SelectedEmu.Exe))
                MessageForm.Error("Emulator doesn't exist in the specified path\nCheck if the path or the selected emulator is correct");
            else if (!File.Exists(Gdirbox.Text))
                MessageForm.Error("Game file doesn't exist in the specified path");
            else if (ICOpic.Image == null)
                MessageForm.Error("Select a picture to continue");
            else
            {
                if (Gdirbox.Text.Contains(Edirbox.Text, StringComparison.OrdinalIgnoreCase))
                    code = Roslyn_FormCode(Gdirbox.Text.Replace(Edirbox.Text, @""));
                else
                    code = Roslyn_FormCode(Gdirbox.Text);

                Compile(code);

                ICOpic.Image = null;
                Gdirbox.Text = Shortcutbox.Text = null;
            }
            Shortcutbox.Focus();
        }

        private void Compile(string code)
        {
            Cursor = Cursors.WaitCursor;
            Application.UseWaitCursor = true;
            string message = "Shortcut created!\nExecute shortcut?";
            string Filename = Shortcutbox.Text;
            string emupath = Edirbox.Text + "ShortCutes";
            if (!Directory.Exists(emupath))
                Directory.CreateDirectory(emupath);
            emupath += @"\";

            string Output = emupath + Filename + ".exe";

            CompilerParameters parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.dll", "System.Windows.Forms.dll", "System.Drawing.dll", "System.Runtime.InteropServices.dll" })
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
                                Environment.NewLine;
                }
                MessageForm.Error(errors);
                Cursor = Cursors.Default;
                Application.UseWaitCursor = false;
                return;
            }

            if (!Directory.Exists(appdata + SelectedEmu.Name))
                Directory.CreateDirectory(appdata + SelectedEmu.Name);

            if (SelectedShortCuteHis == -1)
                new ShortCute(Filename, Edirbox.Text + SelectedEmu.Exe, Gdirbox.Text, appdata + SelectedEmu.Name + @"\" + $"{Filename}.png");
            else
            {
                var shortcute = XmlDocSC.ShortCutes[SelectedShortCuteHis];
                if (shortcute.Name != Filename)
                {
                    File.Delete(emupath + shortcute.Name + ".exe");
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{shortcute.Name}.lnk");
                    File.Delete(shortcute.Image);
                }
                shortcute.Name = Filename;
                shortcute.EmuPath = Edirbox.Text + SelectedEmu.Exe;
                shortcute.GamePath = Gdirbox.Text;
                shortcute.Image = appdata + SelectedEmu.Name + @"\" + $"{Filename}.png";

                message = message.Replace("created", "modified");
                SelectedShortCuteHis = -1;
                Thread order = new Thread(XmlDocSC.SortList);
                order.Start();
            }
            File.Copy(temppath + "tempORIGINAL.png", appdata + SelectedEmu.Name + @"\" + $"{Filename}.png", true);

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

            Cursor = Cursors.Default;
            Application.UseWaitCursor = false;

            if (MessageForm.Success(message))
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
            code = code.Replace("%GAMEFILE%", gamedir);
            code = code.Replace("%ARGUMENTS%", SelectedEmu.Arguments(gamedir));

            return code;
        }

        string TempString = null;
        private void EmuBrow_Click(object sender, EventArgs e)
        {
            string EmuDir = "C:\\";
            if (Edirbox.Text != "")
                EmuDir = Edirbox.Text;

            var file = TempString != null ? TempString : FileDialog(EmuDir, "Executable File (*.exe)|*.exe");
            TempString = null;

            if (file != null)
            {
                bool exists = false;
                foreach (var emu in EmulatorsList)
                {
                    if (emu.Exe.ToLower() == Path.GetFileName(file).ToLower())
                    {
                        if (File.Exists(file))
                            emu.Path(Path.GetDirectoryName(file) + @"\");
                        emulatorcb.SelectedIndex = EmulatorsList.IndexOf(emu);
                        Emulatorcb_SelectedIndexChanged(null, null);
                        exists = true;
                        break;
                    }
                }

                if (exists == false)
                {
                    MessageForm.Info("The emulator isn't supported yet. You can contribute to make it compatible on GitHub (Haruki1707/ShortCutes repo)" +
                        "\n\n!!!This also may occur because you changed the emulator executable name." +
                        "Make sure you are using the original emulator name!!!");
                }
            }

            Shortcutbox.Focus();
        }

        private void GameBrow_Click(object sender, EventArgs e)
        {
            string GamesPath = "C:\\";

            if (Emulatorcb_HasSelectedItem)
            {
                if (SelectedEmu.TryGetGamesPath() != "" && SelectedEmu.GamesPath != null)
                    GamesPath = SelectedEmu.GamesPath;
                else if (!string.IsNullOrWhiteSpace(Gdirbox.Text) && Directory.Exists(Path.GetDirectoryName(Gdirbox.Text)))
                    GamesPath = Path.GetDirectoryName(Gdirbox.Text);
                else if (Edirbox.Text != "")
                    GamesPath = Edirbox.Text;

                var file = TempString != null ? TempString : FileDialog(GamesPath, SelectedEmu.Gamesfilters);
                TempString = null;

                if (file != null && File.Exists(file))
                    Gdirbox.Text = file;
            }
            else
                MessageForm.Info("Emulator must be selected!");

            Shortcutbox.Focus();
        }

        private bool clicked;
        private async void ICOpic_MouseClick(object sender, MouseEventArgs e)
        {
            if (TempString == null)
            {
                if (clicked) return;
                clicked = true;
                await Task.Delay(SystemInformation.DoubleClickTime);
                if (!clicked) return;
                clicked = false;
            }

            //Process click
            var file = TempString != null ? TempString : FileDialog(Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads"), "PNG/JPG Image (*.png; *.jpg; *.jpeg *.tiff *.bmp)|*.png;*.jpg;*.jpeg;*.tiff;*.bmp");
            TempString = null;

            if (file != null && File.Exists(file))
            {
                File.Copy(file, temppath + "tempORIGINAL.png", true);
                ImagingHelper.ConvertToIcon(temppath + "tempORIGINAL.png", temppath + @"temp.ico");
                ICOpic.Image = ImagingHelper.ICONbox;
                ICOpic.Image.Save(temppath + @"temp.png");
            }

            Shortcutbox.Focus();
        }

        private void ICOpic_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clicked = false;

            //Process click
            if (ICOpic.Image != null)
                using (var CI = new CropImage_Tool())
                {
                    CI.ShowDialog();
                    if (CI.DialogResult1 == DialogResult.Yes)
                    {
                        ImagingHelper.ConvertToIcon(temppath + "temp.png", temppath + @"temp.ico");
                        ICOpic.Image = ImagingHelper.ICONbox;
                        ICOpic.Image.Save(temppath + @"temp.png");
                    }
                }
            else
                MessageForm.Info("First select a picture to crop");
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
                            bitmap.Save(temppath + @"tempORIGINAL.png");
                            ImagingHelper.ConvertToIcon(temppath + @"tempORIGINAL.png", temppath + @"temp.ico");
                            ICOpic.Image = ImagingHelper.ICONbox;
                            ICOpic.Image.Save(temppath + @"temp.png");
                        }
                    }
                }
                catch
                {
                    MessageForm.Error("URL provided isn't an image...");
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

        bool InputIsCommand = false;
        private void ICOurl_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Clipboard.ContainsImage())
                {
                    Clipboard.GetImage().Save(temppath + "tempCLIP.png");
                    TempString = temppath + "tempCLIP.png";
                    ICOpic_MouseClick(null, null);
                }
                else if (Clipboard.ContainsFileDropList())
                {
                    var extension = Path.GetExtension(Clipboard.GetFileDropList()[0].ToString());
                    switch (extension)
                    {
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                        case ".bmp":
                        case ".tiff":
                            TempString = Clipboard.GetFileDropList()[0].ToString();
                            ICOpic_MouseClick(null, null);
                            break;
                    }
                }
                else
                    e.Handled = !InputIsCommand;
            }
            catch { }
        }

        //UI things not that important
        private void ICOurl_KeyDown(object sender, KeyEventArgs e)
        {
            InputIsCommand = e.KeyCode == Keys.V && e.Modifiers == Keys.Control;
        }

        string urltext;
        private void ICOurl_Enter(object sender, EventArgs e)
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

        private void OpenFolder_Click(object sender, EventArgs e)
        {
            if (Emulatorcb_HasSelectedItem && File.Exists(Edirbox.Text + SelectedEmu.Exe))
                Process.Start("explorer.exe", Edirbox.Text + @"ShortCutes");
        }

        private void ShortCutes_Paint(object sender, PaintEventArgs e)
        {
            DrawLine(this.Controls, e);
        }

        public static void DrawLine(Control.ControlCollection control, PaintEventArgs g)
        {
            var color = Color.White;
            Pen pen = new Pen(color, 3);
            foreach (Control current in control)
            {
                if (current is TextBox || current is MaskedTextBox)
                {
                    if (current is TextBox)
                        ((TextBox)current).BorderStyle = BorderStyle.None;
                    else
                        ((MaskedTextBox)current).BorderStyle = BorderStyle.None;

                    var LX = current.Location.X;
                    var W = current.Width;
                    var Y = current.Location.Y + current.Height;

                    g.Graphics.DrawLine(pen, new PointF(LX, Y), new PointF(LX + W, Y));
                }
            }
            pen.Dispose();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MiniBtn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

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

        private void HistoryBtn_Click(object sender, EventArgs e)
        {
            createshortbtn.Enabled = false;
            using (var History = new HistoryForm())
            {
                History.ShowDialog();

                if (History.ShortCuteIndex != -1)
                {
                    var ShortCute = XmlDocSC.ShortCutes[History.ShortCuteIndex];
                    SelectedShortCuteHis = History.ShortCuteIndex;
                    History.Dispose();

                    Shortcutbox.Text = ShortCute.Name;
                    TempString = ShortCute.Image;
                    ICOpic_MouseClick(null, null);
                    TempString = ShortCute.EmuPath;
                    EmuBrow_Click(null, null);
                    TempString = ShortCute.GamePath;
                    GameBrow_Click(null, null);
                }

                createshortbtn.Enabled = true;
            }

            Shortcutbox.Focus();
            Shortcutbox.SelectionStart = Shortcutbox.Text.Length;
        }

        private void InfoButton_Click(object sender, EventArgs e)
        {
            using (var info = new MessageForm("ShortCutes  v" + Updater.ProgramFileVersion + "\n\nDeveloped by: Haruki1707\nGitHub: https://github.com/Haruki1707/ShortCutes", 5))
                info.ShowDialog();
        }

        private void ClearSCSelected_Click(object sender, EventArgs e)
        {
            SelectedShortCuteHis = -1;
            Shortcutbox.Text = null;
            Gdirbox.Text = null;
            ICOpic.Image = null;

            Shortcutbox.Focus();
        }

        private void Shortcutbox_Focus(object sender, EventArgs e)
        {
            Shortcutbox.Focus();
        }

        private void Shortcutbox_TextChanged(object sender, EventArgs e)
        {
            if (InputIsCommand && containsABadCharacter.IsMatch(Shortcutbox.Text))
            {
                MessageForm.Error("Invalid filename!\n Cannot contain: " + InvalidFileNameChars);
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
                MessageForm.Error("Invalid filename!\n Cannot contain: " + InvalidFileNameChars);
                e.Handled = true;
            }
        }

        //Let the form to be moved
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void FormDisp_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
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