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
	public partial class CuteLauncher : Form
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
		private bool KeepLauncherOpen = %KEEPOPEN%; 
		private bool KeepLauncherActive = %KEEPACTIVE%; 
        private int ActiveDuration = %KEEPACTIVEDURATION%; 
        private Color avgColor = Color.FromArgb(%avgR%, %avgG%, %avgB%);
		System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

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
