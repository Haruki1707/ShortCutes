using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shortcutes.src
{
	public partial class CuteLauncher : Form
	{
		private void BeginWindowDetection()
		{
			WindowDetectionLoop = 0;
			Task.Run(() =>
			{
				try
				{
					return ShortCute.WaitForInputIdle(InputIdleTimeout);
				}
				catch (ObjectDisposedException)
				{
					return false;
				}
				catch (NotSupportedException)
				{
					return false;
				}
				catch (InvalidOperationException)
				{
					return false;
				}
			}).ContinueWith(task =>
			{
				if (isClosing || closeAnimationStarted || IsDisposed || Disposing)
					return;

				try
				{
					BeginInvoke(new Action(StartWindowPolling));
				}
				catch (InvalidOperationException)
				{
					// The form can close while the idle wait is completing.
				}
			}, TaskScheduler.Default);
		}

		private void StartWindowPolling()
		{
			if (isClosing || closeAnimationStarted || IsDisposed || Disposing)
				return;

			TimerSC.Stop();
			TimerSC.Interval = WindowPollingInterval;
			TimerSC.Tick -= ExecuteEmu_Tick;
			TimerSC.Tick -= WaitEmuToBeOpen_Tick;
			TimerSC.Tick += WaitEmuToBeOpen_Tick;
			TimerSC.Start();
		}

		private string EMainWindowTitle = null;
		private void WaitEmuToBeOpen_Tick(object sender, EventArgs e)
		{
			try
			{
				if (isClosing)
				{
					return;
				}
				if (!string.IsNullOrEmpty(ShortCute.MainWindowTitle))
				{
					EMainWindowTitle = ShortCute.MainWindowTitle;
					emulatorWindowDetected = true;
					if (Configuration.WaitForWindowChange)
					{
						TimerSC.Interval = WindowTitlePollingInterval;
						TimerSC.Tick -= WaitEmuToBeOpen_Tick;
						TimerSC.Tick += WaitEmuToLoad_Tick;
					}
					else
						CloseForm();
				}
				else if (++WindowDetectionLoop >= WindowDetectionLimit)
				{
					TimerSC.Stop();
					CLOSEbutton.Show();
					return;
				}
				ShortCute.Refresh();
			}
			catch (InvalidOperationException)
			{
				if (!isClosing)
					CloseForm();
			}
		}

		private int WaitingLoop = 0;
		private void WaitEmuToLoad_Tick(object sender, EventArgs e)
		{
			if (isClosing)
			{
				return;
			}
			if (emulatorStarted && ShortCute.HasExited)
			{
				CloseForm();
				return;
			}
			if (ShortCute.MainWindowTitle != EMainWindowTitle)
				CloseForm();
			if (WaitingLoop == CloseButtonPollingLoop)
				CLOSEbutton.Show();
			WaitingLoop++;
			ShortCute.Refresh();
		}
	}
}