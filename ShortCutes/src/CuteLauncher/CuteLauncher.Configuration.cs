using System.Drawing;
using System.Windows.Forms;

namespace Shortcutes.src
{
	public partial class CuteLauncher : Form
	{
		private const int LauncherWidth = 256;
		private const int CompactHeight = 72;
		private const int PictureBoxPaddingLeft = 192;
		private const int LoadingImageHeight = 68;
		private const int CloseButtonSize = 20;
		private const int CloseButtonX = LauncherWidth - CloseButtonSize;
		private const int AnimationInterval = 1;
		private const int InitialGrowIncrement = 1;
		private const int MovePictureBoxDelay = 500;
		private const int FullImageFadeHeight = 435;
		private const int MaximumFadeValue = 255;
		private const int StaticImageFadeAlpha = 150;
		private const int ShrinkImageClearOffset = 2;
		private const double ShrinkThresholdRatio = 1.5;
		private const int WindowPollingInterval = 100;
		private const int WindowTitlePollingInterval = 250;
		private const int InputIdleTimeout = 10000;
		private const int WindowDetectionLimit = 300;
		private const int CloseButtonPollingLoop = 40;
		private const int CloseButtonTimeout = 60000;
		private const int LargeImageGrowIncrement = 8;
		private const int StandardImageGrowIncrement = 10;

		private sealed class LauncherConfiguration
		{
			internal readonly string EmulatorPath;
			internal readonly string EmulatorName;
			internal readonly string GameFilePath;
			internal readonly string GameName;
			internal readonly string Arguments;
			internal readonly int StandardHeight;
			internal readonly bool WaitForWindowChange;
			internal readonly bool KeepLauncherOpen;
			internal readonly bool KeepLauncherActive;
			internal readonly int ActiveDuration;
			internal readonly Color AverageColor;

			internal LauncherConfiguration(
				string emulatorPath, string emulatorName, string gameFilePath, string gameName,
				string arguments, int standardHeight, bool waitForWindowChange,
				bool keepLauncherOpen, bool keepLauncherActive, int activeDuration, Color averageColor)
			{
				EmulatorPath = emulatorPath;
				EmulatorName = emulatorName;
				GameFilePath = gameFilePath;
				GameName = gameName;
				Arguments = arguments;
				StandardHeight = standardHeight;
				WaitForWindowChange = waitForWindowChange;
				KeepLauncherOpen = keepLauncherOpen;
				KeepLauncherActive = keepLauncherActive;
				ActiveDuration = activeDuration;
				AverageColor = averageColor;
			}
		}

		private static readonly LauncherConfiguration Configuration = new LauncherConfiguration(
			"%EMULATOR%", "%EMUNAME%", @"%GAMEFILE%", "%GAME%", "%ARGUMENTS%",
			%HEIGHT%, %WAITCHANGE%, %KEEPOPEN%, %KEEPACTIVE%, %KEEPACTIVEDURATION%,
			Color.FromArgb(%avgR%, %avgG%, %avgB%));
	}
}