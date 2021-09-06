using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;

namespace Shortcute
{
	public class CuteLauncher : Form
	{
		private PictureBox BG;
		Timer Execute = new Timer();
		public CuteLauncher()
		{
			this.FormBorderStyle = FormBorderStyle.None;
			this.ClientSize = new System.Drawing.Size(250, 250);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "%GAME%" + " ShortCute";
			this.DoubleBuffered = true;
			this.ShowInTaskbar = false;
			var assembly = System.Reflection.Assembly.GetExecutingAssembly();

			Bitmap BGbit = new Bitmap(assembly.GetManifestResourceStream("temp.png"));
			Icon ico = Icon.FromHandle(BGbit.GetHicon());
			this.Icon = ico;

			Image BGImage = (Image)BGbit;
			using(Graphics g = Graphics.FromImage(BGImage))
            {
				g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), new Rectangle(0, 190, 256, 66));
			}

			BG = new PictureBox()
			{
				Size = new Size(250, 250),
				Location = new Point(0, 0),
				SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage,
				BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
				BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom,
				BackgroundImage = BGImage,
				Image = new Bitmap(assembly.GetManifestResourceStream("loading.gif")),
				Padding = new Padding(186,186,0,0),
				BackColor = Color.Black
			};
			BG.Paint += new PaintEventHandler((sender, e) =>
			{
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				e.Graphics.DrawString("Opening", new System.Drawing.Font("Bahnschrift SemiCondensed", 12F), Brushes.White, 0, 190);
				e.Graphics.DrawString("   %EMUNAME%", new System.Drawing.Font("Bahnschrift SemiCondensed", 22F), Brushes.White, 0, 210);
				e.Graphics.DrawString("Created by Haruki1707", new System.Drawing.Font("Bahnschrift SemiCondensed", 6F), Brushes.LightGray, 0, 240);
			});
			this.Controls.Add(BG);

			Screen s = Screen.FromPoint(Cursor.Position);
			Rectangle b = s.WorkingArea;
			Cursor.Position = new Point(b.Left + b.Width / 2,
								b.Top + b.Height / 2);

			Execute.Interval = 700;
			Execute.Tick += Execute_Tick;
			Execute.Enabled = true;
			Execute.Start();
		}
		Process ShortCute = new Process();
		private void Execute_Tick(object sender, EventArgs e)
		{
			Execute.Stop();
			//Emulator execution
			ShortCute.StartInfo.WorkingDirectory = "..\\";
			ShortCute.StartInfo.FileName = "..\\%EMULATOR%";
			ShortCute.StartInfo.Arguments = "%ARGUMENTS%";
			ShortCute.Start();

			var timer1 = new Timer();
			timer1.Interval = 500;
			timer1.Tick += timer1_Tick;
			timer1.Enabled = true;
			timer1.Start();
		}

		private void timer1_Tick(object sender, EventArgs e)
        {
			if(!string.IsNullOrEmpty(ShortCute.MainWindowTitle))
				Environment.Exit(0);
			ShortCute.Refresh();
		}

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.Run(new CuteLauncher());
		}
	}
}