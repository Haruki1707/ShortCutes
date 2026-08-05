using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shortcutes.src
{
	public partial class CuteLauncher : Form
	{
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

		protected override void OnFormClosed(FormClosedEventArgs e)
        {
			// Unhook the WinEventHook
			if (KeepLauncherOpen && KeepLauncherActive)
			{
            	UnhookWinEvent(hWinEventHook);
			}
            base.OnFormClosed(e);
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

		//Let the form to be moved
		[DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
		private extern static void ReleaseCapture();

		[DllImport("user32.DLL", EntryPoint = "SendMessage")]
		private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

		//For making left click up, so it doesnt make trouble with ShrinkForm
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
		private const int MOUSEEVENTF_LEFTDOWN = 0x02;
		private const int MOUSEEVENTF_LEFTUP = 0x04;
		private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		private const int MOUSEEVENTF_RIGHTUP = 0x10;

        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);
	}
}
