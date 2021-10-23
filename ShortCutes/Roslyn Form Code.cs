using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Windows.Forms;


namespace Shortcutes
{
	public class CuteLauncher : Form
	{
		private PictureBox BG;
		private Timer TimerSC = new Timer();
		private Button CLOSEbutton;
		int standarHeight = %HEIGHT%;
		System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
		public CuteLauncher()
		{
			//standarHeight = 256 || 322;

			this.FormBorderStyle = FormBorderStyle.None;
			this.ClientSize = new Size(256, standarHeight);
			this.BackColor = Color.Black;
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "%GAME%" + " ShortCute";
			this.DoubleBuffered = true;
			this.ShowInTaskbar = false;
			this.Paint += this.OnPaint;
			this.TopMost = true;

			BG = new PictureBox()
			{
				Size = new Size(256, standarHeight),
				Location = new Point(0, 0),
				SizeMode = PictureBoxSizeMode.CenterImage,
				BorderStyle = BorderStyle.FixedSingle,
				Image = new Bitmap(assembly.GetManifestResourceStream("loading.gif")),
				Padding = new Padding(192, standarHeight - 66, 0, 0),
				BackColor = Color.Transparent
			};
			CLOSEbutton = new Button()
			{
				Size = new Size(20, 20),
				Location = new Point(236, 0),
				FlatStyle = System.Windows.Forms.FlatStyle.Flat,
				BackColor = Color.FromArgb(199, 80, 80),
				Text = "X",
				ForeColor = Color.White,
                FlatAppearance =
				{
					BorderSize = 0,
					MouseOverBackColor = Color.Red
                },
				Font = new System.Drawing.Font("Bahnschrift Condensed", 11.25F, System.Drawing.FontStyle.Bold)
			};
			CLOSEbutton.Click += (object sender, EventArgs e) => { this.Close(); };
			this.Controls.Add(BG);

			Screen s = Screen.FromPoint(Cursor.Position);
			Rectangle b = s.WorkingArea;
			Cursor.Position = new Point(b.Left + b.Width / 2, b.Top + b.Height / 2);

			TimerSC.Interval = 700;
			TimerSC.Tick += Execute_Tick;
			TimerSC.Enabled = true;
			TimerSC.Start();
		}

		Process ShortCute = new Process();
		private void Execute_Tick(object sender, EventArgs e)
		{
			TimerSC.Stop();
			//Emulator execution
			ShortCute.StartInfo.WorkingDirectory = "..\\";
			ShortCute.StartInfo.FileName = "..\\%EMULATOR%";
			ShortCute.StartInfo.Arguments = "%ARGUMENTS%";
			ShortCute.Start();

			TimerSC.Interval = 500;
			TimerSC.Tick -= Execute_Tick;
			TimerSC.Tick += timer1_Tick;
			TimerSC.Start();
		}

		string EMainWindowTitle = null;
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(ShortCute.MainWindowTitle))
            {
				EMainWindowTitle = ShortCute.MainWindowTitle;
				TimerSC.Interval = 250;
				TimerSC.Tick -= timer1_Tick;
				TimerSC.Tick += timer2_Tick;
            }
			ShortCute.Refresh();
		}

		int timer2_times = 0;
		private void timer2_Tick(object sender, EventArgs e)
        {
			if(ShortCute.MainWindowTitle != EMainWindowTitle)
				Environment.Exit(0);
			if(timer2_times == 12)
				BG.Controls.Add(CLOSEbutton);
			timer2_times++;
			ShortCute.Refresh();
		}

		private void OnPaint(object sender, PaintEventArgs e)
		{
			Bitmap BGbit = new Bitmap(assembly.GetManifestResourceStream("temp.png"));
			this.Icon = Icon.FromHandle(BGbit.GetHicon());

			Image BGImage = (Image)BGbit;
			if (this.Height == 256)
			{
				using (Graphics g = Graphics.FromImage(BGImage))
					g.FillRectangle(new SolidBrush(Color.FromArgb(150, 22, 28, 38)), new Rectangle(0, 190, 256, 72));
			}
			else
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(47, 49, 54)), new Rectangle(0, 256, 256, 72));

			e.Graphics.DrawImage(BGImage, 0, 0, new Rectangle(0, 0, 256, 256), GraphicsUnit.Pixel);
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			e.Graphics.DrawString("Opening:", new Font("Bahnschrift SemiCondensed", 12F), Brushes.White, 0, standarHeight - 63);
			e.Graphics.DrawString("   %EMUNAME%", new Font("Bahnschrift SemiCondensed", 22F), Brushes.White, 0, standarHeight - 43);
			e.Graphics.DrawString("Created by Haruki1707", new Font("Bahnschrift SemiCondensed", 6F), Brushes.LightGray, 0, standarHeight - 10);
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				// turn on WS_EX_TOOLWINDOW style bit
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