using EZ_Updater;
using ShortCutes.src;
using ShortCutes.src.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortCutes
{
    public partial class ShortCutes : Form
    {
        private bool ComboBox_HasSelectedItem => EmulatorsCbB.SelectedItem != null;
        private Emulator SelectedEmu => Emulators.EmulatorsList[EmulatorsCbB.SelectedIndex];
        private Task<IList<Color>> tImageColors;
        private int HistoryIndex = -1;
        private int SelectedHistoryIndex
        {
            get { return HistoryIndex; }
            set
            {
                HistoryIndex = value; ShortCuteNameTxB.Focus();
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

            foreach (var emu in Emulators.EmulatorsList)
                EmulatorsCbB.Items.Add(emu.Name);

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
            Updater.GUI_Context = SynchronizationContext.Current;
            if (await Updater.CheckUpdateAsync("Haruki1707", "ShortCutes"))
            {
                if (Updater.CannotWriteOnDir)
                    MessageForm.Error(Updater.Message);
                else
                    new MessageForm("", 4).ShowDialog();
            }
        }

        private void Emulatorcb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tempedir = EmuDirTxB.Text;
            EmuDirTxB.Text = null;
            label6.Text = SelectedEmu.Description;
            label6.ForeColor = SelectedEmu.Cdesc;
            EmuDirTxB.Text = SelectedEmu.GetPath();
            if (tempedir != EmuDirTxB.Text)
                GameDirTxB.Text = null;

            if (!string.IsNullOrWhiteSpace(EmuDirTxB.Text) && File.Exists(EmuDirTxB.Text) && !Directory.Exists(Utils.GetDirectoryName(EmuDirTxB.Text) + @"ShortCutes"))
            {
                Directory.CreateDirectory(Utils.GetDirectoryName(EmuDirTxB.Text) + @"ShortCutes");
                MessageForm.Info("To avoid Anti-Virus problems with ShortCutes please exclude this path folder:\n\n" +
                    Utils.GetDirectoryName(EmuDirTxB.Text) + "ShortCutes\n\nDouble click on this text to copy path folder to clipboard", Utils.GetDirectoryName(EmuDirTxB.Text) + "ShortCutes");
            }

            if (Directory.Exists(Utils.GetDirectoryName(EmuDirTxB.Text) + "ShortCutes"))
                OpenFolder.Show();
            else
                OpenFolder.Hide();

            ShortCuteNameTxB.Focus();
        }

        private void CreateShortCute_Click(object sender, EventArgs e)
        {
            if (!ComboBox_HasSelectedItem)
                MessageForm.Error("Emulator must be selected!");
            else if (string.IsNullOrWhiteSpace(ShortCuteNameTxB.Text))
                MessageForm.Error("Shortcut name cannot be empty");
            else if (!File.Exists(EmuDirTxB.Text))
                MessageForm.Error("Emulator doesn't exist in the specified path\nCheck if the path or the selected emulator is correct");
            else if (!File.Exists(GameDirTxB.Text))
                MessageForm.Error("Game file doesn't exist in the specified path");
            else if (ICOpic.Image == null)
                MessageForm.Error("Select a picture to continue");
            else
            {
                StartCompilation();

                ICOpic.Image = null;
                GameDirTxB.Text = ShortCuteNameTxB.Text = null;
                ShortCuteNameTxB.Focus();
            }
        }

        private async void StartCompilation()
        {
            Cursor = Cursors.WaitCursor;
            Application.UseWaitCursor = true;
            string message = "Shortcut created!\nExecute shortcut?";

            // For Armoury Crate on ROG Ally set keepLauncherOpen and keepLauncherActive to true and keepActiveDuration to 5000 ms
            string codeToCompile = Utils.Roslyn_FormCode(SelectedEmu, ShortCuteNameTxB.Text, GameDirTxB.Text.Replace(Utils.GetDirectoryName(EmuDirTxB.Text), @""),
                await GetImageColor(), forceWindowToNotWait.Checked, true, true, 5000);

            string emuPath = Utils.GetDirectoryName(EmuDirTxB.Text) + "ShortCutes";
            if (!Directory.Exists(emuPath))
                Directory.CreateDirectory(emuPath);
            emuPath += @"\";

            string Output = $"{emuPath}{ShortCuteNameTxB.Text}.exe";

            if (!Compiler.Compile(codeToCompile, Output, ShortCuteNameTxB.Text, SelectedEmu.Name))
                return;

            if (insertInHistory(emuPath))
                message = message.Replace("created", "modified");

            if (DesktopCheck.Checked)
                Utils.CreateShortcut(ShortCuteNameTxB.Text, Output, emuPath);

            Cursor = Cursors.Default;
            Application.UseWaitCursor = false;

            if (MessageForm.Success(message))
            {
                var starto = new Process();
                starto.StartInfo.FileName = Output;
                starto.StartInfo.WorkingDirectory = emuPath;
                starto.Start();
            }
        }

        private bool insertInHistory(string emuPath)
        {
            bool alreadyInHistory = false;

            if (!Directory.Exists(Utils.MyAppData + SelectedEmu.Name))
                Directory.CreateDirectory(Utils.MyAppData + SelectedEmu.Name);

            ShortCute shortCute;

            if (SelectedHistoryIndex == -1)
                shortCute = new ShortCute();
            else
            {
                shortCute = XmlDocSC.ShortCutes[SelectedHistoryIndex];
                if (shortCute.Name != ShortCuteNameTxB.Text)
                {
                    File.Delete(emuPath + shortCute.Name + ".exe");
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{shortCute.Name}.lnk");
                    File.Delete(shortCute.Image);
                }

                alreadyInHistory = true;
                SelectedHistoryIndex = -1;
            }

            shortCute.Name = ShortCuteNameTxB.Text;
            shortCute.EmuPath = EmuDirTxB.Text;
            shortCute.GamePath = GameDirTxB.Text;
            shortCute.Image = Utils.MyAppData + SelectedEmu.Name + @"\" + $"{ShortCuteNameTxB.Text}.png";
            File.Copy(Utils.MyTempPath + "tempORIGINAL.png", Utils.MyAppData + SelectedEmu.Name + @"\" + $"{ShortCuteNameTxB.Text}.png", true);

            Thread order = new Thread(XmlDocSC.SortList);
            order.Start();

            return alreadyInHistory;
        }

        private async Task<Color> GetImageColor()
        {
            IList<Color> MostUsed = await tImageColors;
            Trace.WriteLine(MostUsed.Count);
            Color Average = (await tImageColors)[MostUsed.Count() - 1];
            MostUsed.RemoveAt(MostUsed.Count() - 1);

            if (ModifierKeys.HasFlag(Keys.Control))
            {
                Application.UseWaitCursor = false;
                int indexColor = MessageForm.Colors(ControlPaint.Dark(Utils.GetAverageColor(new Color[] { MostUsed[0], Average, Average })),
                    ControlPaint.Dark(Utils.GetAverageColor(new Color[] { MostUsed[1], Average, Average })),
                    ControlPaint.Dark(Utils.GetAverageColor(new Color[] { MostUsed[2], Average, Average })), SelectedEmu.Name);
                Application.UseWaitCursor = true;

                return ControlPaint.Dark(Utils.GetAverageColor(new Color[] { MostUsed[indexColor], Average, Average }));
            }
            else
            {
                Color FinalColor = MostUsed[0];
                for (int i = 1; i < MostUsed.Count(); i++)
                {
                    if (FinalColor.GetSaturation() < MostUsed[i].GetSaturation() && FinalColor.GetBrightness() < MostUsed[i].GetBrightness())
                        FinalColor = MostUsed[i];
                }

                return ControlPaint.Dark(Utils.GetAverageColor(new Color[] { FinalColor, Average, Average }));
            }
        }

        string TempString = null;
        private void EmuBrow_Click(object sender, EventArgs e)
        {
            string EmuDir = "C:\\";
            if (!String.IsNullOrWhiteSpace(EmuDirTxB.Text))
                EmuDir = Utils.GetDirectoryName(EmuDirTxB.Text);

            var file = TempString != null ? TempString : FileDialog(EmuDir, "Executable File (*.exe)|*.exe");
            TempString = null;

            if (file != null)
            {
                bool exists = false;
                foreach (var emu in Emulators.EmulatorsList)
                {
                    if (emu.CheckPath(Path.GetFullPath(file)))
                    {
                        EmulatorsCbB.SelectedIndex = Emulators.EmulatorsList.IndexOf(emu);
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

            ShortCuteNameTxB.Focus();
        }

        private void GameBrow_Click(object sender, EventArgs e)
        {
            string GamesPath = "C:\\";

            if (ComboBox_HasSelectedItem)
            {
                if (SelectedEmu.TryGetGamesPath() != "" && SelectedEmu.GamesPath != null)
                    GamesPath = SelectedEmu.GamesPath;
                else if (!string.IsNullOrWhiteSpace(GameDirTxB.Text) && Directory.Exists(Utils.GetDirectoryName(GameDirTxB.Text)))
                    GamesPath = Utils.GetDirectoryName(GameDirTxB.Text);
                else if (String.IsNullOrWhiteSpace(EmuDirTxB.Text))
                    GamesPath = Utils.GetDirectoryName(EmuDirTxB.Text);

                var file = TempString != null ? TempString : FileDialog(GamesPath, SelectedEmu.Gamesfilters);
                TempString = null;

                if (file != null && File.Exists(file))
                    GameDirTxB.Text = file;
            }
            else
                MessageForm.Info("Emulator must be selected!");

            ShortCuteNameTxB.Focus();
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
                File.Copy(file, Utils.MyTempPath + "tempORIGINAL.png", true);
                ImagingHelper.ConvertToIcon(Utils.MyTempPath + "tempORIGINAL.png", Utils.MyTempPath + @"temp.ico");
                ICOpic.Image = ImagingHelper.ICONbox;
                ICOpic.Image.Save(Utils.MyTempPath + @"temp.png");
            }

            tImageColors = Task.Run(() =>
            {
                IList<Color> colors = Utils.GetMostUsedColors(ICOpic.Image);
                colors.Add(Utils.GetAverageColor(ICOpic.Image));
                return colors;
            });

            ShortCuteNameTxB.Focus();
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
                        ImagingHelper.ConvertToIcon(Utils.MyTempPath + "temp.png", Utils.MyTempPath + @"temp.ico");
                        ICOpic.Image = ImagingHelper.ICONbox;
                        ICOpic.Image.Save(Utils.MyTempPath + @"temp.png");
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
                            bitmap.Save(Utils.MyTempPath + @"tempCLIP.png");
                            TempString = Utils.MyTempPath + "tempCLIP.png";
                            ICOpic_MouseClick(null, null);
                        }
                    }
                }
                catch
                {
                    MessageForm.Error("URL provided isn't an image...");
                }
                ShortCuteNameTxB.Focus();
            }
        }

        bool InputIsCommand = false;
        private void ICOurl_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Clipboard.ContainsImage())
                {
                    Clipboard.GetImage().Save(Utils.MyTempPath + "tempCLIP.png");
                    TempString = Utils.MyTempPath + "tempCLIP.png";
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

        private string FileDialog(string InitialDir, string Filter)
        {
            GameDirTxB.Enabled = EmuDirTxB.Enabled = false;
            string file = Utils.FileDialog(InitialDir, Filter);
            Timer.Start();
            return file;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Stop();
            GameDirTxB.Enabled = EmuDirTxB.Enabled = true;
        }

        // UI things not that important
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
            if (ComboBox_HasSelectedItem && File.Exists(EmuDirTxB.Text))
                Process.Start("explorer.exe", Utils.GetDirectoryName(EmuDirTxB.Text) + @"ShortCutes");
        }

        private void ShortCutes_Paint(object sender, PaintEventArgs e)
        {
            Utils.DrawLine(this.Controls, e);
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
            using (var design = new MessageForm("", 3))
            {
                design.ShowDialog();

                if (design.DialogResult == DialogResult.No)
                    Utils.RectangularDesign = true;
                else if (design.DialogResult == DialogResult.Yes)
                    Utils.RectangularDesign = false;
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
                    EmuDirTxB.Text = GameDirTxB.Text = ShortCuteNameTxB.Text = null;

                    var ShortCute = XmlDocSC.ShortCutes[History.ShortCuteIndex];
                    SelectedHistoryIndex = History.ShortCuteIndex;
                    History.Dispose();

                    ShortCuteNameTxB.Text = ShortCute.Name;
                    TempString = ShortCute.Image;
                    ICOpic_MouseClick(null, null);
                    TempString = ShortCute.EmuPath;
                    EmuBrow_Click(null, null);
                    TempString = ShortCute.GamePath;
                    GameBrow_Click(null, null);
                }

                createshortbtn.Enabled = true;
            }

            ShortCuteNameTxB.Focus();
            ShortCuteNameTxB.SelectionStart = ShortCuteNameTxB.Text.Length;
        }

        private void InfoButton_Click(object sender, EventArgs e)
        {
            using (var info = new MessageForm("ShortCutes  v" + Updater.ProgramFileVersion + "\n\nDeveloped by: Haruki1707\nGitHub: https://github.com/Haruki1707/ShortCutes", 5, "https://github.com/Haruki1707/ShortCutes"))
                info.ShowDialog();
        }

        private void ClearSCSelected_Click(object sender, EventArgs e)
        {
            SelectedHistoryIndex = -1;
            ShortCuteNameTxB.Text = null;
            GameDirTxB.Text = null;
            ICOpic.Image = null;

            ShortCuteNameTxB.Focus();
        }

        private void Shortcutbox_Focus(object sender, EventArgs e)
        {
            ShortCuteNameTxB.Focus();
        }

        private void EmuGameDirbox_TextChanged(object sender, EventArgs e)
        {
            ChangeTextBoxFontSize(sender as TextBox, 9);
        }

        private void ShortCuteName_TextChanged(object sender, EventArgs e)
        {
            if (InputIsCommand && Utils.InvalidFileNameCharsRegex.IsMatch(ShortCuteNameTxB.Text))
            {
                MessageForm.Error("Invalid filename!\n Cannot contain: " + Utils.InvalidFileNameChars);
                ShortCuteNameTxB.Text = Regex.Replace(ShortCuteNameTxB.Text, Utils.InvalidFileNameCharsRegex.ToString(), "");
            }

            ChangeTextBoxFontSize(ShortCuteNameTxB);
        }

        private void ChangeTextBoxFontSize(TextBox TxB, int MaxFontSize = 12)
        {
            if (String.IsNullOrWhiteSpace(TxB.Text))
                return;

            // Measure the text Width against the TextBox Width
            do
            {
                Size ActualFont = TextRenderer.MeasureText(TxB.Text, TxB.Font);
                Size IncreasedFont = TextRenderer.MeasureText(TxB.Text, new Font(TxB.Font.FontFamily, TxB.Font.Size + 0.5F));

                if (ActualFont.Width > (TxB.Width * (TxB.Height / ActualFont.Height)) - 5)
                    TxB.Font = new Font(TxB.Font.FontFamily, TxB.Font.Size - 0.5F);
                else if (TxB.Font.Size < MaxFontSize && IncreasedFont.Width < TxB.Width * (TxB.Height / IncreasedFont.Height) - 5)
                    TxB.Font = new Font(TxB.Font.FontFamily, TxB.Font.Size + 0.5F);
                else
                    break;
            } while (true);
        }

        private void ShortCuteName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Utils.InvalidFileNameCharsRegex.IsMatch(e.KeyChar.ToString()) && !char.IsControl(e.KeyChar))
            {
                MessageForm.Error("Invalid filename!\n Cannot contain: " + Utils.InvalidFileNameChars);
                e.Handled = true;
            }
        }

        private void MoveForm_MouseDown(object sender, MouseEventArgs e)
        {
            this.MoveForm();
        }

        //Code to be executed on UIThreadException
        public static void UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            try
            {
                MessageBox.Show("An application error occurred. Please contact the developer with the following information:\n\n" + t.Exception.Message +
                    "\n\nStack Trace:\n" + t.Exception.StackTrace, "Notify about this error on GitHub repository Haruki1707/ShortCutes", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                if (Updater.CheckUpdate("Haruki1707", "ShortCutes"))
                    new MessageForm("", 4).ShowDialog();
            }
            catch
            {
                try
                {
                    MessageBox.Show("Fatal Windows Forms Error", "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }
    }
}