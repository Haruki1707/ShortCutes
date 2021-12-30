using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Shortcutes
{
	public class CuteLauncher : Form
	{
		private Button CLOSEbutton;
		private PictureBox PictureBoxSC;
		private Timer TimerSC = new Timer();
		private string Emulator = "%EMULATOR%";
		private string EmuName = "%EMUNAME%";
		private string GameFile = @"%GAMEFILE%";
		private string GameName = "%GAME%";
		private int standarHeight = %HEIGHT%;
		private bool WaitForWindowChange = %WAITCHANGE%;
		System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

		public CuteLauncher()
		{
			//standarHeight = 256 || 322;
			FormBorderStyle = FormBorderStyle.None;
			ClientSize = new Size(256, standarHeight);
			BackColor = Color.FromArgb(47, 49, 54);
			StartPosition = FormStartPosition.CenterScreen;
			Text = GameName + " ShortCute";
			DoubleBuffered = true;
			ShowInTaskbar = false;
			TopMost = true;

			PictureBoxSC = new PictureBox()
			{
				Size = new Size(256, standarHeight),
				Location = new Point(0, 0),
				SizeMode = PictureBoxSizeMode.CenterImage,
				BorderStyle = BorderStyle.None,
				Image = new Bitmap(assembly.GetManifestResourceStream("loading.gif")),
				Padding = new Padding(192, standarHeight - 66, 0, 0),
				BackColor = Color.Transparent
			};
			CLOSEbutton = new Button()
			{
				Size = new Size(20, 20),
				Location = new Point(236, 0),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(199, 80, 80),
				Text = "X",
				ForeColor = Color.White,
                FlatAppearance =
				{
					BorderSize = 0,
					MouseOverBackColor = Color.Red
                },
				Font = new Font("Bahnschrift Condensed", 11.25F, FontStyle.Bold)
			};
			CLOSEbutton.Hide();
			CLOSEbutton.Click += (object sender, EventArgs e) => { Close(); };
			PictureBoxSC.MouseDown += FormDisp_MouseDown;
			PictureBoxSC.Controls.Add(CLOSEbutton);
			Controls.Add(PictureBoxSC);

			//PictureBox Background
			Bitmap PBGImage = new Bitmap(256, standarHeight);
			using (Graphics graph = Graphics.FromImage(PBGImage))
            {
				Bitmap BGbit = new Bitmap(assembly.GetManifestResourceStream("temp.png"));
				Icon = Icon.FromHandle(BGbit.GetHicon());

				Image BGImage = (Image)BGbit;
				if (Height == 256)
					using (Graphics g = Graphics.FromImage(BGImage))
						g.FillRectangle(new SolidBrush(Color.FromArgb(150, 22, 28, 38)), new Rectangle(0, 190, 256, 72));
				else
					graph.FillRectangle(new SolidBrush(Color.FromArgb(47, 49, 54)), new Rectangle(0, 256, 256, 72));

				graph.DrawImage(BGImage, 0, 0, new Rectangle(0, 0, 256, 256), GraphicsUnit.Pixel);
				graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				graph.DrawString("Opening:", new Font("Bahnschrift SemiCondensed", 12F), Brushes.White, 0, standarHeight - 63);
				graph.DrawString("   " + EmuName, new Font("Bahnschrift SemiCondensed", 22F), Brushes.White, 0, standarHeight - 43);
				graph.DrawString("Created by Haruki1707", new Font("Bahnschrift SemiCondensed", 6F), Brushes.DimGray, 0, standarHeight - 10);
				PictureBoxSC.BackgroundImage = PBGImage;
            }

			TimerSC.Interval = 500;
			TimerSC.Tick += ExecuteEmu_Tick;
			TimerSC.Start();
		}

		Process ShortCute = new Process();
		private void ExecuteEmu_Tick(object sender, EventArgs e)
		{
			TimerSC.Stop();
			var emupath = AppContext.BaseDirectory.ToString();
				emupath = emupath.Remove(emupath.Length - 1);
				emupath = emupath.Substring(0, emupath.LastIndexOf(@"\") + 1);

			if (!File.Exists("..\\" + Emulator))
				MessageError("emulator", emupath + Emulator);
			else if (!Path.IsPathRooted(GameFile) && !File.Exists("..\\" + GameFile))
				MessageError("game", emupath + GameFile);
			else if (Path.IsPathRooted(GameFile) && !File.Exists(GameFile))
				MessageError("game", GameFile);

			//Emulator execution
			ShortCute.StartInfo.WorkingDirectory = "..\\";
			ShortCute.StartInfo.FileName = "..\\" + Emulator;
			ShortCute.StartInfo.Arguments = "%ARGUMENTS%";
			ShortCute.Start();

			TimerSC.Interval = 100;
			TimerSC.Tick -= ExecuteEmu_Tick;
			TimerSC.Tick += WaitEmuToBeOpen_Tick;
			TimerSC.Start();

			var ShowCloseTimer = new Timer();
			ShowCloseTimer.Interval = 60000;
			ShowCloseTimer.Tick += ShowCloseBtn;
			ShowCloseTimer.Start();
		}

		private void ShowCloseBtn(object sender, EventArgs e)
        {
			((Timer)sender).Stop();
			CLOSEbutton.Show();
        }

		string EMainWindowTitle = null;
		private void WaitEmuToBeOpen_Tick(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(ShortCute.MainWindowTitle))
            {
				EMainWindowTitle = ShortCute.MainWindowTitle;
				if (WaitForWindowChange)
				{
					TimerSC.Interval = 250;
					TimerSC.Tick -= WaitEmuToBeOpen_Tick;
					TimerSC.Tick += WaitEmuToLoad_Tick;
				}
				else
					Close();
            }
			ShortCute.Refresh();
		}

		int WaitingLoop = 0;
		private void WaitEmuToLoad_Tick(object sender, EventArgs e)
        {
			if(ShortCute.MainWindowTitle != EMainWindowTitle)
				Close();
			if (WaitingLoop == 40)
				CLOSEbutton.Show();
			WaitingLoop++;
			ShortCute.Refresh();
		}

		private void MessageError(string type, string path)
        {
			MessageBox.Show("Make sure that the " + type + " is located in:\n" +
				path + 
				"\n\nif you moved the " + type + ", re-doing the ShortCute could fix the problem" +
				"\n\nThis ShortCute will be closed");
			Environment.Exit(0);
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

		//Enable form over any other app
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x80;
				return cp;
			}
		}

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.Run(new CuteLauncher());
		}
	}
}