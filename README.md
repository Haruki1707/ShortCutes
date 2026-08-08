<h1 align="center">
  <br>
  <a href="https://haruki1707.github.io/ShortCutes">
    <img src="https://user-images.githubusercontent.com/72423267/143326519-019f0465-3289-4b63-bcf2-922597769777.png" width="200">
  </a>
  <br>
  <b>ShortCutes</b>
  <br>
  <a href="https://github.com/Haruki1707/ShortCutes/releases"><img src="https://img.shields.io/github/v/release/haruki1707/shortcutes?style=for-the-badge&color=beeaff"></a>
  <img src="https://img.shields.io/github/downloads/haruki1707/shortcutes/total?color=e7c4af&style=for-the-badge">
  <a href="https://github.com/Haruki1707/ShortCutes/issues"><img src="https://img.shields.io/github/issues-raw/haruki1707/ShortCutes?style=for-the-badge&color=4f2b11"></a>
</h1>

<h3 align="center">
Creates executable ShortCutes for your emulated games.
<br>
<a href="https://haruki1707.github.io/ShortCutes">
    Website with more information clicking here.
</a>
<br><br>
</h3>

## **[Download ShortCutes](https://github.com/Haruki1707/ShortCutes/releases/latest/download/ShortCutes.exe)** here or **[in download page](https://haruki1707.github.io/ShortCutes/#Download)**
* Requires [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48) | *usually updated W10 and W11 already have it installed*
* Exclude ShortCutes folder from any Anti-Virus, ShortCutes folder is created on your Emulator path root

### **ShortCutes launcher design**
Can be selected on ShortCutes config (default: Rectangular)

Squared | Rectangular
:--------:|:----------:
![](https://raw.githubusercontent.com/Haruki1707/ShortCutes/main/ShortCutes/Resources/square.png) | ![](https://github.com/Haruki1707/ShortCutes/blob/main/ShortCutes/Resources/rectangular.png?raw=true)

### **Actual supported emulators**

Emulator | Version From | Version To | Description
---------|:------------:|:---------: | :---------:
[Cemu](https://cemu.info/) | 1.11.4 | Latest | Work as expected
[Dolphin](https://dolphin-emu.org) | 3.5 | Latest | Work as expected
[PJ64](https://www.pj64-emu.com) | 1.7 | Latest | Need to activate start in fullscreen manually
[Snes9x](https://www.snes9x.com) | 1.5 | Latest | Work as expected
[yuzu](https://yuzu-emu.org/) | Mainline 503 | Latest | Work as expected
[Ryujinx](https://ryujinx.org/) | 1.0.5888 | Latest | Work as expected
[Citra](https://citra-emu.org/) | Unknown | Latest | Need to activate start in fullscreen
[DeSmuME](http://desmume.org/) | Unknown | Latest | Works as expected
[mGBA](https://mgba.io/) | Unknown | Latest | Works as expected
[VBA](https://sourceforge.net/projects/vba/) | Unknown | Latest | Deprecated, if possible better use VBA-M
[VBA-M](https://vba-m.com/) | Unknown | Latest | Activate start in fullscreen. Don't exit fullscreen. Do ALT+F4
[PCSX2](https://pcsx2.net/) | Unknown | Latest | Works as expected
[RPCS3](https://rpcs3.net/) | 0.0.5 | Latest | Need to activate close when process finishes and fullscreen
[PPSSPP](https://www.ppsspp.org/) | Unknown | Latest | Works as expected
[xemu](https://xemu.app/) | Unknown | Latest | Works as expected
[xenia](https://xenia.jp/) | Unknown | Latest | Works as expected
[DuckStation](https://www.duckstation.org/) | Unknown | Latest | Works as expected
[MelonDS](https://melonds.kuribo64.net/) | Unknown | Latest | Works as expected
[Flycast](https://github.com/flyinghead/flycast) | Unknown | Latest | Works as expected
[suyu](https://suyu.dev/) | 0.0.1 | Latest | Works as expected
[sudachi](https://sudachiemu.com/home/) | 0.0.1 | Latest | Works as expected

### **Examples of ShortCutes**
* CEMU

https://user-images.githubusercontent.com/72423267/140080920-1a2c82e2-2958-42b6-b116-8b5e9f4f07e4.mp4

* YUZU

https://user-images.githubusercontent.com/72423267/140080768-30114c81-48d2-4dd3-a395-306de800a6c4.mp4

* DOLPHIN

https://user-images.githubusercontent.com/72423267/140080931-d30cc844-6427-4226-af6d-3a1999faf8f6.mp4

## **Armoury Crate**
You can use ShortCutes to add your emulated games to the ROG Ally's Armoury Crate launcher. This allows you to not only launch your games from Armoury Crate but also to take advantage of its active game detection and custom game profiles.

After following the instructions, you'll be able to:

* Create game-specific profiles to define settings like TDP (Thermal Design Power) and custom controller configurations.

* Have Armoury Crate detect when your game is running and automatically re-open Armoury Crate after you exit your game.

### Set-Up

Follow these steps to get your emulated games working perfectly with Armoury Crate:

1.  **Add Your Emulator to Armoury Crate:**
    * Open Armoury Crate.
    * Go to the menu and select "Add games to library."
    * Press `L` or `R` to browse for files.
    * Navigate to your emulator's `.exe` file and select it. Your emulator should now appear in your Armoury Crate library.

2.  **Configure Your Emulator Entry:**
    * In Armoury Crate, select your newly added emulator.
    * Press `X` and choose "Game Info."
    * Edit the tags and select **`PC Gaming Client`**.
    * Make sure the option to "Allow other games to run simultaneously with this one" is **enabled**.

3.  **Test Emulator Launch & Profile:**
    * Try launching your emulator through Armoury Crate.
    * Once it's open, bring up the **ROG Ally's Quick Menu**.
    * Select "Game Profile" (if "Game Profile" isn't there, add it to your quick access) and verify that it refers to your **emulator's profile**.
    * Ensure that **none of the settings in this profile diverge from default**.

4.  **Create Your Game ShortCutes:**
    * Use the ShortCutes application to create shortcuts for your individual emulated games.
    * **Crucially, make sure to select the "Armoury Crate Launcher" checkbox** in the ShortCutes form for each game.

5.  **Add Game ShortCutes to Armoury Crate:**
    * Add these newly created ShortCutes to Armoury Crate, just like you added your emulator's `.exe` file in step 1.

6.  **Set-Up Game-Specific Profiles:**
    * Start one of your emulated games from Armoury Crate.
    * While the "ShortCutes loading" pop-up is on screen, open the Quick Menu again and select "Game Profile."
    * **Verify that this profile refers to your *game* and not to the emulator.**
    * Now, feel free to adjust any settings you want for this specific game (e.g., TDP, controller mappings).
    * *Pro Tip:* Try setting a unique Aura Sync lighting configuration for the profile. This is a great visual cue to confirm your game's profile is correctly applied.

7.  **Finalize & Restart:**
    * Exit your game.
    * Restart Armoury Crate.
    * You're all set!

### How It Works

When you launch a game using a ShortCutes entry from Armoury Crate:

* The ShortCutes pop-up will briefly appear, triggering the application of your **game-specific settings**.
* If your game loses focus (e.g., you switch windows), your default settings will be reapplied. However, when your game regains focus, the ShortCutes pop-up will reappear, **re-applying your custom game settings**.
* As long as your game is running, Armoury Crate will show it as "running" in your library.
* Once you exit your game, Armoury Crate will detect this and automatically **re-open the launcher**, ready for your next gaming session!