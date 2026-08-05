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
		private int GrowInt = InitialGrowIncrement;
		Process ShortCute = new Process();
		private Timer ShowCloseTimer;
		private bool isClosing;
		private bool closeAnimationStarted;
		private bool emulatorStarted;
		private bool emulatorWindowDetected;
		private int WindowDetectionLoop;
		private static string[] ExtraArgs = new string[0];
		System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

        [STAThread]
		static void Main(string[] args)
		{
			ExtraArgs = args ?? new string[0];

			Application.EnableVisualStyles();
			Application.Run(new CuteLauncher());
		}
	}
}
