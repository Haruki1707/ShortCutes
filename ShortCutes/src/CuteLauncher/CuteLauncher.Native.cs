using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shortcutes.src
{
	public partial class CuteLauncher : Form
	{
		private static class NativeMethods
		{
			[DllImport("user32.dll")]
			internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

			[DllImport("user32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool UnhookWinEvent(IntPtr hWinEventHook);

			[DllImport("user32.dll")]
			internal static extern IntPtr GetForegroundWindow();

			[DllImport("user32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool SetForegroundWindow(IntPtr hWnd);

			[DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
			internal static extern void ReleaseCapture();

			[DllImport("user32.dll", EntryPoint = "SendMessage")]
			internal static extern void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

			[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			internal static extern void MouseEvent(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
		}

		// ---------------------------------------------------------------
		// Use WinEventHook to get notified when the foreground window changes
		private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        private WinEventDelegate procDelegate;
        private IntPtr hWinEventHook;

              private const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
              private const uint WINEVENT_OUTOFCONTEXT = 0;
		private bool emulatorIsForeground = true;
		private readonly object emulatorIsForegroundLock = new object();
		// ---------------------------------------------------------------

		protected override void OnFormClosed(FormClosedEventArgs e)
        {
			CleanupLauncherResources();
			if (TextImage != null)
			{
				TextImage.Dispose();
				TextImage = null;
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
				// If foreground window is the emulator -> Show CuteLauncher by the specified duration
				// If foreground window is the launcher -> Do nothing
				// If foreground window is any other window -> Reset the emulatorIsForeground variable
				IntPtr foregroundWindow = NativeMethods.GetForegroundWindow();
				if (foregroundWindow == ShortCute.MainWindowHandle)
				{
					// Avoid multiple invocations when the emulator is already foreground.
					if (!emulatorIsForeground)
					{
						emulatorIsForeground = true;
						shouldInvoke = true;
					}
				}
				else if (foregroundWindow != launcherWindowHandle)
				{
					// Reset the state when another application becomes foreground.
					emulatorIsForeground = false;
				}
			}

			if (shouldInvoke)
			{
				// Execute in the GUI thread.
				if (!IsHandleCreated || IsDisposed || Disposing)
					return;

				try
				{
					this.BeginInvoke(new Func<Task>(async () =>
					{
						try
						{
							if (isClosing || IsDisposed || Disposing)
								return;

							this.Show();
							this.WindowState = FormWindowState.Normal;
							this.BringToFront();
							NativeMethods.SetForegroundWindow(launcherWindowHandle);

							await Task.Delay(Configuration.ActiveDuration);

							this.WindowState = FormWindowState.Minimized;
							this.Hide();
							if (!ShortCute.HasExited)
								NativeMethods.SetForegroundWindow(ShortCute.MainWindowHandle);
						}
						catch (ObjectDisposedException)
						{
							// The form or emulator can be disposed while the callback is completing.
						}
						catch (InvalidOperationException)
						{
							// The form can close between the callback and UI invocation.
						}
						catch (Exception ex)
						{
							MessageBox.Show("Error in WinEventProc: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}));
				}
				catch (InvalidOperationException)
				{
					// The form can close between the handle check and invocation.
				}
			}
		}
		private const int MOUSEEVENTF_LEFTDOWN = 0x02;
		private const int MOUSEEVENTF_LEFTUP = 0x04;
		private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		private const int MOUSEEVENTF_RIGHTUP = 0x10;
	}
}
