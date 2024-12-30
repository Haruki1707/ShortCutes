using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Shortcutes.src
{
	public class CuteLauncher : Form
	{
		private Button CLOSEbutton;
		private PictureBox PictureBoxImage;
		private PictureBox PictureBoxSC;
		private PictureBox PBFade;
		private Timer TimerSC = new Timer();
		private bool isMouseDown = false;

        private Image TextImage;

		private int GrowInt = 1;
		Process ShortCute = new Process();
		private static string ExtraArgs = "";
		private string Emulator = "%EMULATOR%";
		private string EmuName = "%EMUNAME%";
		private string GameFile = @"%GAMEFILE%";
		private string GameName = "%GAME%";
		private static int standarHeight = %HEIGHT%;
		private bool WaitForWindowChange = %WAITCHANGE%;
		private bool KeepLauncherOpen = %KEEPOPEN%; // Keep launcher running in background
		private bool KeepLauncherActive = %KEEPACTIVE%; // Focus the launcher window when the emulator is focused (requires KeepLauncherOpen)
        private int ActiveDuration = %KEEPACTIVEDURATION%; // Duration in milliseconds for which the launcher will be focused (requires KeepLauncherOpen and KeepLauncherActive)
        private Color avgColor = Color.FromArgb(%avgR%, %avgG%, %avgB%);
		System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

		// ---------------------------------------------------------------
		// Use WinEventHook to get notified when the foreground window changes
		private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        private WinEventDelegate procDelegate;
        private IntPtr hWinEventHook;

        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern Int32 SetForegroundWindow(IntPtr hWnd);

        private const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
        private const uint WINEVENT_OUTOFCONTEXT = 0;
		private bool emulatorIsForeground = true;
		private readonly object emulatorIsForegroundLock = new object();
		// ---------------------------------------------------------------

		public CuteLauncher()
		{
			//standarHeight = 256 || 322;
			FormBorderStyle = FormBorderStyle.None;
			ClientSize = new Size(256, 72);
			BackColor = avgColor;
			StartPosition = FormStartPosition.CenterScreen;
			Text = GameName + " ShortCute";
			DoubleBuffered = true;
			ShowInTaskbar = false;
			TopMost = true;

			PictureBoxImage = new PictureBox()
			{
				Size = new Size(256, standarHeight),
				Location = new Point(0, 0),
				SizeMode = PictureBoxSizeMode.CenterImage,
				BorderStyle = BorderStyle.None,
				Image = new Bitmap(assembly.GetManifestResourceStream("temp.png")),
				Padding = new Padding(0, 0, 0, standarHeight - 256),
				BackColor = Color.Transparent,
				AutoSize = true
			};

			PictureBoxSC = new PictureBox()
			{
				Size = new Size(256, standarHeight),
				Location = new Point(0, 0),
				SizeMode = PictureBoxSizeMode.CenterImage,
				BorderStyle = BorderStyle.None,
				Padding = new Padding(192, 0, 0, standarHeight - 68),
				Image = new Bitmap(assembly.GetManifestResourceStream("loading.gif")),
				BackColor = avgColor
			};

			PBFade = new PictureBox()
			{
				Size = new Size(256, standarHeight),
				Location = new Point(0, 0),
				BorderStyle = BorderStyle.None,
				BackColor = avgColor
			};

			if (standarHeight == 256)
				GrowInt = 8;
            else
				GrowInt = 10;

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
			PictureBoxSC.Controls.Add(PBFade);
			PictureBoxImage.Controls.Add(CLOSEbutton);
			PictureBoxImage.Controls.Add(PictureBoxSC);
			Controls.Add(PictureBoxImage);

			//PictureBox Background
			TextImage = new Bitmap(256, standarHeight);
			using (Graphics graph = Graphics.FromImage(TextImage))
            {
				graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				graph.DrawString("Opening:", new Font("Bahnschrift SemiCondensed", 12F), Brushes.White, 0, 3);
				graph.DrawString("   " + EmuName, new Font("Bahnschrift SemiCondensed", 22F), Brushes.White, 0, 23);
				graph.DrawString("Created by Haruki1707", new Font("Bahnschrift SemiCondensed", 6F), Brushes.DimGray, 0, 56);
				PictureBoxSC.BackgroundImage = TextImage;
			}

			TimerSC.Interval = 1;
			TimerSC.Tick += GrowForm;
			TimerSC.Start();
		}

		private void GrowForm(object sender, EventArgs e)
		{
			ClientSize = new Size(256, ClientSize.Height + GrowInt);
			Top -= GrowInt / 2;

			if (ClientSize.Height >= standarHeight)
			{
				TimerSC.Stop();

				PBFade.MouseDown += FormDisp_MouseDown;
				PBFade.MouseUp += FormDisp_MouseUp;
                PictureBoxSC.MouseDown += FormDisp_MouseDown;
                PictureBoxSC.MouseUp += FormDisp_MouseUp;
                PictureBoxImage.MouseDown += FormDisp_MouseDown;
                PictureBoxImage.MouseUp += FormDisp_MouseUp;

                TimerSC.Interval = 1;
				TimerSC.Tick -= GrowForm;
				TimerSC.Tick += MovePB;
				TimerSC.Start();
			}
		}

		private static int PBHeight = standarHeight - 72;
		private double PBH25 = PBHeight * .25;
		private double PBH75 = PBHeight * .75;
		private void MovePB(object sender, EventArgs e)
		{
			PictureBoxSC.Location = new Point(0, PictureBoxSC.Location.Y + GrowInt);
			if(PictureBoxSC.Location.Y >= PBH25)
				PBFade.BackColor = Color.FromArgb((int)Math.Round(255-(float)((float)( (float)(PictureBoxSC.Location.Y - PBH25) /(float)PBH75 ) *255)), avgColor);
			if (standarHeight == 256)
				PictureBoxSC.BackColor = Color.FromArgb((int)Math.Round(255 - (float)((float)((float)PictureBoxSC.Location.Y / (float)435) * 255)), avgColor);


			if (PictureBoxSC.Location.Y >= PBHeight)
			{
				TimerSC.Stop();
				PictureBoxSC.Location = new Point(0, PictureBoxSC.Location.Y + GrowInt/2);
				PBFade.BackColor = Color.Transparent;
				if(standarHeight == 256)
					PictureBoxSC.BackColor = Color.FromArgb(150, avgColor);
				TimerSC.Interval = 500;
				TimerSC.Tick -= MovePB;
				TimerSC.Tick += ExecuteEmu_Tick;
				TimerSC.Start();
				GrowInt = -GrowInt;
			}
		}

		private void ShrinkForm(object sender, EventArgs e)
		{
			ClientSize = new Size(256, ClientSize.Height + GrowInt);
			PictureBoxSC.Location = new Point(0, PictureBoxSC.Location.Y + GrowInt);
			Top -= GrowInt / 2;
			CLOSEbutton.Hide();

			if ((ClientSize.Height <= 72 + Math.Abs(GrowInt)*2) && hWinEventHook != IntPtr.Zero)
				PictureBoxImage.Image = null;

			if (ClientSize.Height < 72 / 1.5)
			{
				TimerSC.Stop();
				SetForegroundWindow(ShortCute.Handle.ToInt32());

                // If KeepLauncherOpen is enabled, we minimize instead of closing the launcher
                if (KeepLauncherOpen)
				{
					this.WindowState = FormWindowState.Minimized;
                    this.Hide();

					// Register a WineventHook to get notified on foreground window change
					if (KeepLauncherActive)
					{
						ClientSize = new Size(256, standarHeight);
                        PictureBoxSC.Location = new Point(0, PBHeight);
						Top = ((Screen.PrimaryScreen.WorkingArea.Height - Height) / 2) - (standarHeight / 2);

                        TextImage = new Bitmap(256, standarHeight);
                        using (Graphics graph = Graphics.FromImage(TextImage))
                        {
                            graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                            graph.DrawString("Armory Crate:", new Font("Bahnschrift SemiCondensed", 12F), Brushes.White, 0, 3);
                            graph.DrawString(" " + "Applying profile", new Font("Bahnschrift SemiCondensed", 20F), Brushes.White, 0, 23);
                            graph.DrawString("Created by Haruki1707", new Font("Bahnschrift SemiCondensed", 6F), Brushes.DimGray, 0, 56);
                            PictureBoxSC.BackgroundImage = TextImage;
                        }

                        procDelegate = new WinEventDelegate(WinEventProc);
						hWinEventHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
					}
				}
				else
				{
					Close();
				}
                SetForegroundWindow(ShortCute.Handle.ToInt32());
            }
        }

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

			string arguments = "%ARGUMENTS%";	
			if (arguments.Contains("%USERARGS%"))
				arguments = arguments.Replace("%USERARGS%", ExtraArgs);
			else
				arguments += ExtraArgs;

			//Emulator execution
			ShortCute.StartInfo.WorkingDirectory = "..\\";
			ShortCute.StartInfo.FileName = "..\\" + Emulator;
			ShortCute.StartInfo.Arguments = arguments;
			ShortCute.EnableRaisingEvents = true; // Enable process to raise events
            ShortCute.Exited += Emulator_Exited; // Get notified when the emulator process exits
            ShortCute.Start();

			TimerSC.Interval = 250;
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
			try
			{
				TimerSC.Interval = 100;
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
						CloseForm();
				}
				ShortCute.Refresh();
			}
			catch { }
		}

		int WaitingLoop = 0;
		private void WaitEmuToLoad_Tick(object sender, EventArgs e)
        {
			if(ShortCute.MainWindowTitle != EMainWindowTitle)
				CloseForm();
			if (WaitingLoop == 40)
				CLOSEbutton.Show();
			WaitingLoop++;
			ShortCute.Refresh();
		}

		private void CloseForm()
        {
			if (isMouseDown)
			{
				mouse_event(MOUSEEVENTF_LEFTUP, (uint)MousePosition.X, (uint)MousePosition.Y, 0, 0);
				mouse_event(MOUSEEVENTF_RIGHTUP, (uint)MousePosition.X, (uint)MousePosition.Y, 0, 0);
			}
            PBFade.MouseDown -= FormDisp_MouseDown;
			PictureBoxSC.MouseDown -= FormDisp_MouseDown;
			PictureBoxImage.MouseDown -= FormDisp_MouseDown;

			TimerSC.Stop();
			TimerSC.Interval = 1;
			TimerSC.Tick -= WaitEmuToLoad_Tick;
			TimerSC.Tick -= WaitEmuToBeOpen_Tick;
			TimerSC.Tick += ShrinkForm;
			TimerSC.Start();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
        {
			// Unhook the WinEventHook
			if (KeepLauncherOpen && KeepLauncherActive)
			{
            	UnhookWinEvent(hWinEventHook);
			}
            base.OnFormClosed(e);
        }

		private void Emulator_Exited(object sender, EventArgs e)
        {
            Close();
        }

		// Callback on foreground window change
		private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            // Check if the foreground window is the ShortCute process
			bool shouldInvoke = false;
			IntPtr launcherWindowHandle = this.Handle;
			lock (emulatorIsForegroundLock)
			{
                // If foreground window is the emulator ->  Show CuteLauncher by the specified duration
                // If foreground window is the launcher -> Do nothing
                // If foreground window is any other window -> Reset the emulatorIsForeground variable
                IntPtr foregroundWindow = GetForegroundWindow();
				if (foregroundWindow == ShortCute.MainWindowHandle)
				{
                    // Avoid multiple invocations of the CuteLauncher window when the emulator is the foreground window
                    if (!emulatorIsForeground)
					{
						emulatorIsForeground = true;
						shouldInvoke = true;
					}
				}
				else if (foregroundWindow != launcherWindowHandle)
				{
                    // Reset the emulatorIsForeground variable when the emulator is not the foreground window
                    emulatorIsForeground = false;
				}
			}

			if (shouldInvoke)
			{
				// Execute in GUI thread
				this.Invoke(new Func<Task>(async () =>
				{
					try
					{
						// Show the CuteLauncher as the foreground window and make it the active window
						// s.t. third-party tools can detect it as the active window
						this.Show();
						this.WindowState = FormWindowState.Normal;
						this.BringToFront();
						SetForegroundWindow(launcherWindowHandle.ToInt32());

						// Wait for the specified delay duration before hiding the window again
						await Task.Delay(ActiveDuration);

						// Hide the CuteLauncher window again and make sure the emulator is the foreground window
						this.WindowState = FormWindowState.Minimized;
						this.Hide();
						SetForegroundWindow(ShortCute.Handle.ToInt32());
					}
					catch (Exception ex)
					{
						// Handle any exceptions that occur
						MessageBox.Show("Error in WinEventProc: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}));
			}
        }

		//Tools
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
			isMouseDown = true;
			ReleaseCapture();
			SendMessage(this.Handle, 0x112, 0xf012, 0);
		}

		private void FormDisp_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
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

		//For making left click up, so it doesnt make trouble with ShrinkForm
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
		private const int MOUSEEVENTF_LEFTDOWN = 0x02;
		private const int MOUSEEVENTF_LEFTUP = 0x04;
		private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		private const int MOUSEEVENTF_RIGHTUP = 0x10;

        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);

        //MAIN
        [STAThread]
		static void Main(string[] args)
		{
			if (args.Length > 0)
				foreach (string arg in args)
					ExtraArgs += " " + arg;

			Application.EnableVisualStyles();
			Application.Run(new CuteLauncher());
		}
	}
}