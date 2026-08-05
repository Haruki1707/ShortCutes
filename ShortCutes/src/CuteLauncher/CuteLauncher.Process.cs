using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Shortcutes.src
{
	public partial class CuteLauncher : Form
	{
		private void ExecuteEmu_Tick(object sender, EventArgs e)
		{
			TimerSC.Stop();
			if (isClosing)
			{
				return;
			}
			string emulatorPath = ResolvePath(AppContext.BaseDirectory, Configuration.EmulatorPath);
			string gamePath = ResolvePath(AppContext.BaseDirectory, Configuration.GameFilePath);

			if (!File.Exists(emulatorPath))
			{
				MessageError("emulator", emulatorPath);
				return;
			}

			if (!File.Exists(gamePath))
			{
				MessageError("game", gamePath);
				return;
			}

			string arguments = BuildArguments(Configuration.Arguments, gamePath);

			ShortCute.StartInfo.WorkingDirectory = Path.GetDirectoryName(emulatorPath);
			ShortCute.StartInfo.FileName = emulatorPath;
			ShortCute.StartInfo.Arguments = arguments;
			ShortCute.StartInfo.UseShellExecute = false;
			ShortCute.EnableRaisingEvents = true;
			ShortCute.Exited -= Emulator_Exited;
			ShortCute.Exited += Emulator_Exited;

			try
			{
				if (!ShortCute.Start())
				{
					MessageError("start the emulator", emulatorPath);
					return;
				}
				emulatorStarted = true;
			}
			catch (Win32Exception ex)
			{
				MessageError("start the emulator", emulatorPath + "\n\n" + ex.Message);
				return;
			}
			catch (InvalidOperationException ex)
			{
				MessageError("start the emulator", emulatorPath + "\n\n" + ex.Message);
				return;
			}

			if (isClosing)
				return;

			BeginWindowDetection();

			if (ShowCloseTimer != null)
				ShowCloseTimer.Dispose();
			ShowCloseTimer = new Timer();
			ShowCloseTimer.Interval = CloseButtonTimeout;
			ShowCloseTimer.Tick += ShowCloseBtn;
			ShowCloseTimer.Start();
		}

		private static string ResolvePath(string baseDirectory, string path)
		{
			if (Path.IsPathRooted(path))
				return Path.GetFullPath(path);

			string launcherDirectory = Path.GetFullPath(baseDirectory);
			DirectoryInfo launcherDirectoryInfo = new DirectoryInfo(launcherDirectory);
			string emulatorDirectory = launcherDirectoryInfo.Parent == null
				? launcherDirectory
				: launcherDirectoryInfo.Parent.FullName;
			return Path.GetFullPath(Path.Combine(emulatorDirectory, path));
		}

		private string BuildArguments(string argumentsTemplate, string gamePath)
		{
			argumentsTemplate = argumentsTemplate.Replace(Configuration.GameFilePath, gamePath);
			string userArguments = BuildUserArguments();
			if (argumentsTemplate.Contains("%USERARGS%"))
				return argumentsTemplate.Replace("%USERARGS%", userArguments);

			return string.IsNullOrEmpty(userArguments)
				? argumentsTemplate
				: argumentsTemplate + " " + userArguments;
		}

		private static string BuildUserArguments()
		{
			StringBuilder result = new StringBuilder();
			foreach (string argument in ExtraArgs)
			{
				if (result.Length > 0)
					result.Append(' ');
				result.Append(QuoteArgument(argument));
			}
			return result.ToString();
		}

		private static string QuoteArgument(string argument)
		{
			if (argument == null)
				return "\"\"";

			bool needsQuotes = argument.Length == 0 || argument.IndexOfAny(new[] { ' ', '\t', '\"' }) >= 0;
			if (!needsQuotes)
				return argument;

			StringBuilder result = new StringBuilder("\"");
			int backslashes = 0;
			foreach (char character in argument)
			{
				if (character == '\\')
				{
					backslashes++;
					continue;
				}

				if (character == '\"')
					result.Append('\\', backslashes * 2 + 1);
				else
					result.Append('\\', backslashes);

				result.Append(character);
				backslashes = 0;
			}

			result.Append('\\', backslashes * 2);
			return result.Append('\"').ToString();
		}
	}
}