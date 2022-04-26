using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ShortCutes
{
    static class Emulators
    {
        public static readonly List<Emulator> EmulatorsList = new List<Emulator>();
        private static readonly List<string> Shortcuts = new List<string>();
        private static string Appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        static Emulators()
        {
            //To add a emulator call 'new Emulator();'
            //Params: "Name of emulator", "name of exe.exe", "Game files that emulator supports", "Parameters before game file path", "Parameters in file path", "Parameters after game file path"

            //CEMU
            //Works as expected
            var CEMU = new Emulator("Cemu", "cemu.exe", "WiiU Games (*.rpx; *.wud; *.wux; *.elf; *.iso)", "-g %GAME% -f", true);
            CEMU.SetConfigPath(@"settings.xml", "GamePaths", "Entry");
            EmulatorsList.Add(CEMU);

            //Dolphin
            //Works as expected
            var Dolphin = new Emulator("Dolphin", "dolphin.exe", "Wii/GC Games (*.iso; *.wbfs; *.ciso; *.gcz; *.rvz; *.gcm; *.tgc; *.wia; *.wad)", "-e %GAME%");
            Dolphin.SetConfigPath(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Dolphin Emulator\Config\Dolphin.ini", "General", "ISOPath0");
            EmulatorsList.Add(Dolphin);

            //SNES9X
            //Works as expected
            EmulatorsList.Add(new Emulator("Snes9x", "snes9x.exe", "SNES Games (*.smc; *.sfc; *.swc; *.zip)", "%..\\\\GAME% -fullscreen"));

            //PJ64
            //Need to activate fullscreen through Emulator GUI
            var PJ64 = new Emulator("Project64", "Project64.exe", "Nintendo 64 Games (*.n64; *.z64; *.v64; *.u64; *.zip; *.7z; *.rar; *.rom; *.jap; *.pal; *.usa; *.bin; *.ndd; *.d64)");
            PJ64.DescriptionChange("Activate fullscreen through PJ64 GUI");
            PJ64.SetConfigPath(@"\Config\Project64.cfg", "Game Directory", "Directory");
            EmulatorsList.Add(PJ64);

            //YUZU
            //Works as expected
            var YUZU = new Emulator("yuzu", "yuzu.exe", "Switch Games (*.xci; *.nsp; *.nso; *.nro; *.nca; *.kip)", "-f -g %GAME%", true);
            YUZU.SetConfigPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\yuzu\config\qt-config.ini", "UI", @"Paths\gamedirs\4\path");
            EmulatorsList.Add(YUZU);

            //RYUJINX
            //Works as expected
            //Deppending on the computer could be low performance
            var Ryujinx = new Emulator("Ryujinx", "Ryujinx.exe", "Switch Games (*.xci; *.nsp; *.nso; *.nro; *.nca; *.pfs0)", "-f %GAME%");
            Ryujinx.SetConfigPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ryujinx\Config.json", "game_dirs", "");
            EmulatorsList.Add(Ryujinx);

            //CITRA
            //Need to enable fullscreen on GUI
            var Citra = new Emulator("Citra", "citra-qt.exe", "3DS Games (*.3ds; *.3dsx; *.elf; *.axf; *.cci; *.cxi; *.app)");
            Citra.SetConfigPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Citra\config\qt-config.ini", "UI", @"Paths\gamedirs\3\path");
            Citra.DescriptionChange("Need to activate fullscren in GUI");
            EmulatorsList.Add(Citra);

            //DeSmuME
            //Works as expected
            var DeSmuME = new Emulator("DeSmuME", "DeSmuME.exe", "DS Games (*.nds; *.ds.gba; +.srl; *.zip; *.7z; *.rar; *.gz)", "--windowed-fullscreen %GAME%");
            DeSmuME.DescriptionChange("Please check that emulator is named DeSmuME.exe");
            EmulatorsList.Add(DeSmuME);

            //mGBA
            //Works as expected
            EmulatorsList.Add(new Emulator("mGBA", "mGBA.exe", "GB-GBC-GBA Games (*.gba; *.zip; *.7z; *.elf; *.agb; *.mb; *.rom; *.bin; *.gb; *.gbc; *.sgb)", "-f %GAME%"));

            //VBA
            //Works as expected, deprecated
            var VBA = new Emulator("VBA", "VisualBoyAdvance.exe", "GB-GBC-GBA Games (*.gba; *.gbc; *.gb; *.sgb; *.cgb; *.agb; *.bin)", "-F %GAME%");
            VBA.DescriptionChange("Deprecated, if possible better use VBA-M");
            EmulatorsList.Add(VBA);

            //VBA-M
            //Need to enable fullscreen on UI
            //Fullscreen command /f
            var VBA_M = new Emulator("VBA-M", "visualboyadvance-m.exe", "GB-GBC-GBA Games (*.gba; *.gbc; *.gb; *.zip; *.agb; *.7z; *.rar; *.mb; *.bin; *.dmg; *-cgb; *.sgb)");
            VBA_M.DescriptionChange("Enable start in fullscreen. Don't exit fullscreen, do ALT+F4", true);
            EmulatorsList.Add(VBA_M);

            //RPCS3
            //Need to activate close when process finishes and fullscreen
            var RPCS3 = new Emulator("RPCS3", "rpcs3.exe", "PS3 Games (*.bin)");
            RPCS3.DescriptionChange("Activate RPCS3 close when process finish and fullscreen");
            EmulatorsList.Add(RPCS3);

            //PCSX2
            //Works as expected
            EmulatorsList.Add(new Emulator("PCSX2", "pcsx2.exe", "PS2 Games (*.iso; *.mdf; *.nrg; *.bin; *.img; *.dump; *.gz; *.csp)", "%GAME% --fullscreen --nogui"));

            //PPSSPP
            //Works as expected
            EmulatorsList.Add(new Emulator("PPSSPP", "PPSSPPWindows.exe", "PSP Games (*.iso; *.cso; *.pbp; *.elf; *.prx; ¨.zip; ¨.ppdmp)", "--fullscreen %GAME%"));

            //PPSSPP64
            //Works as expected
            EmulatorsList.Add(new Emulator("PPSSPP-64", "PPSSPPWindows64.exe", "PSP Games (*.iso; *.cso; *.pbp; *.elf; *.prx; ¨.zip; ¨.ppdmp)", "--fullscreen %GAME%"));

            //Xemu
            //Works as expected
            EmulatorsList.Add(new Emulator("xemu", "xemu.exe", "Xbox Games (*.iso)", "-full-screen -dvd_path %GAME%"));

            //Xenia
            //Works as expected
            EmulatorsList.Add(new Emulator("xenia", "xenia.exe", "Xbox360 Games (*.xex; *.iso)", "%GAME% --fullscreen"));

            //To find if emulator shortcut exist for easy use of Shortcutes
            ShortcutsFinder();
        }

        public static void ShortcutsFinder(Emulator emu = null)
        {
            List<Emulator> emulist = EmulatorsList;
            if (emu != null)
            {
                emulist = new List<Emulator> { emu };

                if (emu.Name == "yuzu" && File.Exists(Appdata + @"\Yuzu\yuzu-windows-msvc-early-access\" + emu.Exe))
                {
                    emu.Path(Appdata + @"\Yuzu\yuzu-windows-msvc-early-access\");
                    return;
                }
                else if (emu.Name == "yuzu" && File.Exists(Appdata + @"\Yuzu\yuzu-windows-msvc\" + emu.Exe))
                {
                    emu.Path(Appdata + @"\Yuzu\yuzu-windows-msvc\");
                    return;
                }
            }

            GetLnkFiles(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            GetLnkFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) + @"\Programs");
            GetLnkFiles(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + @"\Programs");
            GetLnkFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar");

            foreach (var shortcut in Shortcuts)
            {
                IWshRuntimeLibrary.IWshShortcut lnk = new IWshRuntimeLibrary.WshShell().CreateShortcut(shortcut);
                if (lnk != null)
                {
                    foreach (var emulator in EmulatorsList)
                    {
                        if (emulator.Exe.ToLower() == Path.GetFileName(lnk.TargetPath).ToLower())
                            emulator.Path(Path.GetDirectoryName(lnk.TargetPath) + @"\");
                    }
                }
            }
            emulist = null;
        }

        private static void GetLnkFiles(string Path)
        {
            var shortcuts = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories).Where(s => s.ToLower().EndsWith(".lnk"));
            foreach (var shortcut in shortcuts)
                if (!Shortcuts.Contains(shortcut))
                    Shortcuts.Add(shortcut);
            shortcuts = null;
        }
    }

    class Emulator
    {
        readonly string name;
        readonly string exe;
        readonly string arguments;
        readonly string gamesfilters;
        string description = "Works as expected";
        string InstallPath = null;
        string gamesPath = null;
        Color cdesc = Color.LightGreen;

        bool SelfConfig = false;
        string ConfigPath = null;
        string ConfigSection = null;
        string ConfigElement = null;
        readonly bool WaitWindowChangeP = false;

        public string Name { get => name; }
        public string Exe { get => exe; }
        public string Gamesfilters { get => gamesfilters; }
        public string Description { get => description; }
        public string GamesPath { get => gamesPath; }
        public Color Cdesc { get => cdesc; }
        public bool WaitWindowChange { get => WaitWindowChangeP; }

        public Emulator(string Name, string Exe, string Filters, string Arguments, bool waitWindowChange = false)
        {
            name = Name;
            exe = Exe;
            arguments = Arguments;
            /*argumentsP1 = ArgumentsP1 + " \\\"";
            argumentsPmid = ArgumentsPmid;
            argumentsP2 = "\\\" " + ArgumentsP2;*/
            gamesfilters = Filters + "|" + Filters.Split('(', ')')[1];
            WaitWindowChangeP = waitWindowChange;
        }

        public Emulator(string Name, string Exe, string Filters)
        {
            name = Name;
            exe = Exe;
            arguments = "%GAME%";
            gamesfilters = Filters + "|" + Filters.Split('(', ')')[1];
        }

        public void DescriptionChange(string desc, bool red = false)
        {
            description = desc;
            if (red == true)
                cdesc = Color.OrangeRed;
            else
                cdesc = Color.Yellow;
        }

        public string Path(string path = null)
        {
            if (path != null & File.Exists(path + exe))
                InstallPath = path;
            else if (string.IsNullOrWhiteSpace(path) && !File.Exists(InstallPath + exe))
                Emulators.ShortcutsFinder(this);

            return InstallPath;
        }

        public string TryGetGamesPath()
        {
            if (ConfigPath != null)
            {
                if (SelfConfig)
                {
                    ConfigPath = InstallPath + ConfigPath;
                    SelfConfig = false;
                }
                if (!File.Exists(ConfigPath))
                    return null;

                switch (System.IO.Path.GetExtension(ConfigPath))
                {
                    case ".xml":
                        try
                        {
                            XDocument doc = XDocument.Load(ConfigPath);
                            foreach (var itemElement in doc.Element("content").Elements(ConfigSection))
                            {
                                var properties = itemElement.Elements(ConfigElement).ToList();
                                gamesPath = (string)properties[0];
                            }
                        }
                        catch { }
                        break;
                    case ".ini":
                    case ".cfg":
                        try
                        {
                            string Directory = new IniFile(ConfigPath).Read(ConfigElement, ConfigSection).Replace('/', '\\'); ;
                            if (Directory != null)
                                gamesPath = Directory;
                        }
                        catch { }
                        break;
                    case ".json":
                        try
                        {
                            var json = JsonConvert.DeserializeObject<JToken>(File.ReadAllText(ConfigPath));
                            gamesPath = (string)json[ConfigSection][0];
                        }
                        catch { }
                        break;
                    default:
                        break;
                }
            }
            return gamesPath;
        }

        public void SetConfigPath(string File, string Section, string Element)
        {
            ConfigPath = File;
            ConfigSection = Section;
            ConfigElement = Element;
            if (!System.IO.File.Exists(File))
                SelfConfig = true;
        }

        public string Arguments(string gamedir)
        {
            string args = arguments.Replace("%", "\\\"");
            return args.Replace("GAME", gamedir.Replace(@"\", @"\\"));
        }
    }
}
