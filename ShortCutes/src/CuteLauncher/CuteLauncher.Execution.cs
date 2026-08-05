using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Shortcutes.src
{
	public partial class CuteLauncher : Form
	{
		private void ExecuteEmu_Tick(object sender, EventArgs e)
		{
			TimerSC.Stop();
			var baseDir = AppContext.BaseDirectory;
			var emupath = Path.GetDirectoryName(baseDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
			if (string.IsNullOrEmpty(emupath)) emupath = baseDir;
			if (!emupath.EndsWith(Path.DirectorySeparatorChar.ToString())) emupath += Path.DirectorySeparatorChar;

			if (!File.Exists(emupath + Emulator))
				MessageError("emulator", emupath + Emulator);
			else if (!Path.IsPathRooted(GameFile) && !File.Exists(emupath + GameFile))
				MessageError("game", emupath + GameFile);
			else if (Path.IsPathRooted(GameFile) && !File.Exists(GameFile))
				MessageError("game", GameFile);

			string arguments = "%ARGUMENTS%";	
			if (arguments.Contains("%USERARGS%"))
				arguments = arguments.Replace("%USERARGS%", ExtraArgs);
			else
				arguments += ExtraArgs;

			//Emulator execution
			ShortCute.StartInfo.WorkingDirectory = emupath;
			ShortCute.StartInfo.FileName = emupath + Emulator;
			ShortCute.StartInfo.Arguments = arguments;
			ShortCute.EnableRaisingEvents = true; // Enable process to raise events
            ShortCute.Exited += Emulator_Exited; // Get notified when the emulator process exits
            ShortCute.Start();

			TimerSC.Interval = 250;
			TimerSC.Tick -= ExecuteEmu_Tick;
			TimerSC.Tick += WaitEmuToBeOpen_Tick;
			TimerSC.Start();

			var ShowCloseTimer = new Timer();
			ShowCloseTimer.Interval = 60000;
			ShowCloseTimer.Tick += ShowCloseBtn;
			ShowCloseTimer.Start();
		}

		string EMainWindowTitle = null;
		private void WaitEmuToBeOpen_Tick(object sender, EventArgs e)
		{
			try
			{
				TimerSC.Interval = 100;
				if (!string.IsNullOrEmpty(ShortCute.MainWindowTitle))
				{
					EMainWindowTitle = ShortCute.MainWindowTitle;
					if (WaitForWindowChange)
					{
						TimerSC.Interval = 250;
						TimerSC.Tick -= WaitEmuToBeOpen_Tick;
						TimerSC.Tick += WaitEmuToLoad_Tick;
					}
					else
						CloseForm();
				}
				ShortCute.Refresh();
			}
			catch { }
		}

		int WaitingLoop = 0;
		private void WaitEmuToLoad_Tick(object sender, EventArgs e)
        {
			if(ShortCute.MainWindowTitle != EMainWindowTitle)
				CloseForm();
			if (WaitingLoop == 40)
				CLOSEbutton.Show();
			WaitingLoop++;
			ShortCute.Refresh();
		}

		private void CloseForm()
        {
			if (isMouseDown)
			{
				mouse_event(MOUSEEVENTF_LEFTUP, (uint)MousePosition.X, (uint)MousePosition.Y, 0, 0);
				mouse_event(MOUSEEVENTF_RIGHTUP, (uint)MousePosition.X, (uint)MousePosition.Y, 0, 0);
			}
            PBFade.MouseDown -= FormDisp_MouseDown;
			PictureBoxSC.MouseDown -= FormDisp_MouseDown;
			PictureBoxImage.MouseDown -= FormDisp_MouseDown;

			TimerSC.Stop();
			TimerSC.Interval = 1;
			TimerSC.Tick -= WaitEmuToLoad_Tick;
			TimerSC.Tick -= WaitEmuToBeOpen_Tick;
			TimerSC.Tick += ShrinkForm;
			TimerSC.Start();
		}

		private void Emulator_Exited(object sender, EventArgs e)
        {
            Close();
        }

		private void MessageError(string type, string path)
        {
			MessageBox.Show("Make sure that the " + type + " is located in:\n" +
				path + 
				"\n\nif you moved the " + type + ", re-doing the ShortCute could fix the problem" +
				"\n\nThis ShortCute will be closed");
			Environment.Exit(0);
		}
	}
}
