using System;
using System.Drawing;
using System.Windows.Forms;

namespace Shortcutes.src
{
	public partial class CuteLauncher : Form
	{
		public CuteLauncher()
		{
			FormBorderStyle = FormBorderStyle.None;
			ClientSize = new Size(LauncherWidth, CompactHeight);
			BackColor = Configuration.AverageColor;
			StartPosition = FormStartPosition.CenterScreen;
			Text = Configuration.GameName + " ShortCute";
			DoubleBuffered = true;
			ShowInTaskbar = false;
			TopMost = true;

			PictureBoxImage = new PictureBox()
			{
				Size = new Size(LauncherWidth, Configuration.StandardHeight),
				Location = new Point(0, 0),
				SizeMode = PictureBoxSizeMode.CenterImage,
				BorderStyle = BorderStyle.None,
				Image = new Bitmap(assembly.GetManifestResourceStream("temp.png")),
				Padding = new Padding(0, 0, 0, Configuration.StandardHeight - LauncherWidth),
				BackColor = Color.Transparent,
				AutoSize = true
			};

			PictureBoxSC = new PictureBox()
			{
				Size = new Size(LauncherWidth, Configuration.StandardHeight),
				Location = new Point(0, 0),
				SizeMode = PictureBoxSizeMode.CenterImage,
				BorderStyle = BorderStyle.None,
				Padding = new Padding(PictureBoxPaddingLeft, 0, 0, Configuration.StandardHeight - LoadingImageHeight),
				Image = new Bitmap(assembly.GetManifestResourceStream("loading.gif")),
				BackColor = Configuration.AverageColor
			};

			PBFade = new PictureBox()
			{
				Size = new Size(LauncherWidth, Configuration.StandardHeight),
				Location = new Point(0, 0),
				BorderStyle = BorderStyle.None,
				BackColor = Configuration.AverageColor
			};

			if (Configuration.StandardHeight == LauncherWidth)
				GrowInt = LargeImageGrowIncrement;
            else
				GrowInt = StandardImageGrowIncrement;

			CLOSEbutton = new Button()
			{
				Size = new Size(CloseButtonSize, CloseButtonSize),
				Location = new Point(CloseButtonX, 0),
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
			TextImage = new Bitmap(LauncherWidth, Configuration.StandardHeight);
			using (Graphics graph = Graphics.FromImage(TextImage))
            {
				graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				graph.DrawString("Opening:", new Font("Bahnschrift SemiCondensed", 12F), Brushes.White, 0, 3);
				graph.DrawString("   " + Configuration.EmulatorName, new Font("Bahnschrift SemiCondensed", 22F), Brushes.White, 0, 23);
				graph.DrawString("Created by Haruki1707", new Font("Bahnschrift SemiCondensed", 6F), Brushes.DimGray, 0, 56);
				PictureBoxSC.BackgroundImage = TextImage;
			}

			TimerSC.Interval = AnimationInterval;
			TimerSC.Tick += GrowForm;
			TimerSC.Start();
		}

		private void GrowForm(object sender, EventArgs e)
		{
			ClientSize = new Size(LauncherWidth, ClientSize.Height + GrowInt);
			Top -= GrowInt / 2;

			if (ClientSize.Height >= Configuration.StandardHeight)
			{
				TimerSC.Stop();

				PBFade.MouseDown += FormDisp_MouseDown;
				PBFade.MouseUp += FormDisp_MouseUp;
                PictureBoxSC.MouseDown += FormDisp_MouseDown;
                PictureBoxSC.MouseUp += FormDisp_MouseUp;
                PictureBoxImage.MouseDown += FormDisp_MouseDown;
                PictureBoxImage.MouseUp += FormDisp_MouseUp;

                TimerSC.Interval = AnimationInterval;
				TimerSC.Tick -= GrowForm;
				TimerSC.Tick += MovePB;
				TimerSC.Start();
			}
		}

		private static int PBHeight = Configuration.StandardHeight - CompactHeight;
		private double PBH25 = PBHeight * .25;
		private double PBH75 = PBHeight * .75;
		private void MovePB(object sender, EventArgs e)
		{
			PictureBoxSC.Location = new Point(0, PictureBoxSC.Location.Y + GrowInt);
			if(PictureBoxSC.Location.Y >= PBH25)
				PBFade.BackColor = Color.FromArgb((int)Math.Round(MaximumFadeValue-(float)((float)( (float)(PictureBoxSC.Location.Y - PBH25) /(float)PBH75 ) *MaximumFadeValue)), Configuration.AverageColor);
			if (Configuration.StandardHeight == LauncherWidth)
				PictureBoxSC.BackColor = Color.FromArgb((int)Math.Round(MaximumFadeValue - (float)((float)((float)PictureBoxSC.Location.Y / (float)FullImageFadeHeight) * MaximumFadeValue)), Configuration.AverageColor);


			if (PictureBoxSC.Location.Y >= PBHeight)
			{
				TimerSC.Stop();
				PictureBoxSC.Location = new Point(0, PictureBoxSC.Location.Y + GrowInt/2);
				PBFade.BackColor = Color.Transparent;
				if(Configuration.StandardHeight == LauncherWidth)
					PictureBoxSC.BackColor = Color.FromArgb(StaticImageFadeAlpha, Configuration.AverageColor);
				TimerSC.Interval = MovePictureBoxDelay;
				TimerSC.Tick -= MovePB;
				TimerSC.Tick += ExecuteEmu_Tick;
				TimerSC.Start();
				GrowInt = -GrowInt;
			}
		}

		private void ShrinkForm(object sender, EventArgs e)
		{
			ClientSize = new Size(LauncherWidth, ClientSize.Height + GrowInt);
			PictureBoxSC.Location = new Point(0, PictureBoxSC.Location.Y + GrowInt);
			Top -= GrowInt / 2;
			CLOSEbutton.Hide();

			if ((ClientSize.Height <= CompactHeight + Math.Abs(GrowInt) * ShrinkImageClearOffset) && hWinEventHook != IntPtr.Zero)
				PictureBoxImage.Image = null;

			if (ClientSize.Height < CompactHeight / ShrinkThresholdRatio)
			{
				TimerSC.Stop();
				NativeMethods.SetForegroundWindow(ShortCute.MainWindowHandle);

                // If KeepLauncherOpen is enabled, we minimize instead of closing the launcher
				if (Configuration.KeepLauncherOpen)
				{
					this.WindowState = FormWindowState.Minimized;
                    this.Hide();

					// Register a WineventHook to get notified on foreground window change
					if (Configuration.KeepLauncherActive && hWinEventHook == IntPtr.Zero)
					{
						ClientSize = new Size(LauncherWidth, Configuration.StandardHeight);
						PictureBoxSC.Location = new Point(0, PBHeight);
						Top = ((Screen.PrimaryScreen.WorkingArea.Height - Height) / 2) - (Configuration.StandardHeight / 2);

						TextImage = new Bitmap(LauncherWidth, Configuration.StandardHeight);
                        using (Graphics graph = Graphics.FromImage(TextImage))
                        {
                            graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                            graph.DrawString("Armory Crate:", new Font("Bahnschrift SemiCondensed", 12F), Brushes.White, 0, 3);
                            graph.DrawString(" " + "Applying profile", new Font("Bahnschrift SemiCondensed", 20F), Brushes.White, 0, 23);
                            graph.DrawString("Created by Haruki1707", new Font("Bahnschrift SemiCondensed", 6F), Brushes.DimGray, 0, 56);
                            PictureBoxSC.BackgroundImage = TextImage;
                        }

                        procDelegate = new WinEventDelegate(WinEventProc);
						hWinEventHook = NativeMethods.SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
					}
				}
				else
				{
					Hide();
					Close();
					return;
				}
                NativeMethods.SetForegroundWindow(ShortCute.MainWindowHandle);
            }
        }

		private void ShowCloseBtn(object sender, EventArgs e)
        {
			((Timer)sender).Stop();
			CLOSEbutton.Show();
        }

		private void FormDisp_MouseDown(object sender, MouseEventArgs e)
		{
			isMouseDown = true;
			NativeMethods.ReleaseCapture();
			NativeMethods.SendMessage(this.Handle, 0x112, 0xf012, 0);
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
	}
}
