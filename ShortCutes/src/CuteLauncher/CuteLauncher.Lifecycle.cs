using System;
using System.Windows.Forms;

namespace Shortcutes.src
{
	public partial class CuteLauncher : Form
	{
		private void CloseForm()
		{
			if (isClosing || closeAnimationStarted)
				return;

			closeAnimationStarted = true;
			if (isMouseDown)
			{
				NativeMethods.MouseEvent(MOUSEEVENTF_LEFTUP, (uint)MousePosition.X, (uint)MousePosition.Y, 0, 0);
				NativeMethods.MouseEvent(MOUSEEVENTF_RIGHTUP, (uint)MousePosition.X, (uint)MousePosition.Y, 0, 0);
			}
			PBFade.MouseDown -= FormDisp_MouseDown;
			PictureBoxSC.MouseDown -= FormDisp_MouseDown;
			PictureBoxImage.MouseDown -= FormDisp_MouseDown;

			TimerSC.Stop();
			TimerSC.Interval = AnimationInterval;
			TimerSC.Tick -= WaitEmuToLoad_Tick;
			TimerSC.Tick -= WaitEmuToBeOpen_Tick;
			TimerSC.Tick += ShrinkForm;
			TimerSC.Start();
		}

		private void Emulator_Exited(object sender, EventArgs e)
		{
			if (isClosing || closeAnimationStarted || !emulatorWindowDetected || IsDisposed || Disposing)
				return;

			try
			{
				if (IsHandleCreated)
					BeginInvoke(new Action(CloseForm));
			}
			catch (InvalidOperationException)
			{
				// The form can be closing while the process raises Exited.
			}
		}

		private void CleanupLauncherResources()
		{
			if (isClosing)
				return;

			isClosing = true;
			UnhookForegroundWindowEvent();
			TimerSC.Stop();
			TimerSC.Tick -= GrowForm;
			TimerSC.Tick -= MovePB;
			TimerSC.Tick -= ExecuteEmu_Tick;
			TimerSC.Tick -= WaitEmuToBeOpen_Tick;
			TimerSC.Tick -= WaitEmuToLoad_Tick;
			TimerSC.Tick -= ShrinkForm;
			TimerSC.Dispose();

			if (ShowCloseTimer != null)
			{
				ShowCloseTimer.Stop();
				ShowCloseTimer.Tick -= ShowCloseBtn;
				ShowCloseTimer.Dispose();
				ShowCloseTimer = null;
			}

			ShortCute.Exited -= Emulator_Exited;
			ShortCute.Dispose();
		}

		private void UnhookForegroundWindowEvent()
		{
			if (hWinEventHook == IntPtr.Zero)
				return;

			NativeMethods.UnhookWinEvent(hWinEventHook);
			hWinEventHook = IntPtr.Zero;
			procDelegate = null;
		}

		private void MessageError(string type, string path)
		{
			MessageBox.Show("Make sure that the " + type + " is located in:\n" +
				path +
				"\n\nif you moved the " + type + ", re-doing the ShortCute could fix the problem" +
				"\n\nThis ShortCute will be closed");
			Close();
		}
	}
}