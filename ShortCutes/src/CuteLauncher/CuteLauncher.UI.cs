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

		private void ShowCloseBtn(object sender, EventArgs e)
        {
			((Timer)sender).Stop();
			CLOSEbutton.Show();
        }

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
	}
}
