using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace ShortCutes
{
    static class Emulators
    {
        public static readonly List<Emulator> EmulatorsList = new List<Emulator>();
        private static readonly List<string> Shortcuts = new List<string>();

        static Emulators()
        {
            //To add a emulator call 'new Emulator();'
            //Params: "Name of emulator", "name of exe.exe", "Game files that emulator supports", "Parameters before game file path", "Parameters in file path", "Parameters after game file path"

            //CEMU
            //Works as expected
            var CEMU = new Emulator("CEMU", "cemu.exe", "WiiU Games (*.rpx; *.wud; *.wux; *.elf; *.iso)", "-g", "", "-f", true);
            CEMU.SetConfigPath(@"settings.xml", "GamePaths", "Entry");
            EmulatorsList.Add(CEMU);

            //Dolphin
            //Works as expected
            var Dolphin = new Emulator("Dolphin", "dolphin.exe", "Wii/GC Games (*.iso; *.wbfs; *.ciso; *.gcz; *.rvz; *.gcm; *.tgc; *.wia; *.wad)", "-e", "", "");
            Dolphin.SetConfigPath(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Dolphin Emulator\Config\Dolphin.ini", "General", "ISOPath0");
            EmulatorsList.Add(Dolphin);

            //SNES9X
            //Works as expected
            EmulatorsList.Add(new Emulator("SNES9X", "snes9x.exe", "SNES Games (*.smc; *.sfc; *.swc; *.zip)", "", "..\\\\", "-fullscreen"));

            //PJ64
            //Need to activate fullscreen through Emulator GUI
            var PJ64 = new Emulator("PJ64", "Project64.exe", "Nintendo 64 Games (*.n64; *.z64; *.v64; *.u64; *.zip; *.7z; *.rar; *.rom; *.jap; *.pal; *.usa; *.bin; *.ndd; *.d64)");
            PJ64.DescriptionChange("Activate fullscreen through PJ64 GUI");
            PJ64.SetConfigPath(@"\Config\Project64.cfg", "Game Directory", "Directory");
            EmulatorsList.Add(PJ64);

            //YUZU
            //Works as expected
            var YUZU = new Emulator("YUZU", "yuzu.exe", "Switch Games (*.xci; *.nsp; *.nso; *.nro; *.nca; *.kip)", "-f -g", "", "", true);
            YUZU.SetConfigPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\yuzu\config\qt-config.ini", "UI", @"Paths\gamedirs\4\path");
            var Appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (System.IO.File.Exists(Appdata + @"\Yuzu\yuzu-windows-msvc-early-access\" + YUZU.Exe))
                YUZU.Path(Appdata + @"\Yuzu\yuzu-windows-msvc-early-access\");
            else
                YUZU.Path(Appdata + @"\Yuzu\yuzu-windows-msvc\");
            EmulatorsList.Add(YUZU);

            //RYUJINX
            //Works as expected
            //Deppending on the computer could be low performance
            var Ryujinx = new Emulator("RYUJINX", "Ryujinx.exe", "Switch Games (*.xci; *.nsp; *.nso; *.nro; *.nca; *.pfs0)","-f", "", "");
            Ryujinx.SetConfigPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ryujinx\Config.json", "game_dirs", "");
            EmulatorsList.Add(Ryujinx);


            //VBA-M
            //Need to enable fullscreen on UI
            var VBA_M = new Emulator("VBA-M", "visualboyadvance-m.exe", "GB-GBC-GBA Games (*.gba; *.gbc; *.gb; *.zip; *.agb; *.7z; *.rar; *.mb; *.bin)", "/f", "", "");
            VBA_M.DescriptionChange("Enable start in fullscreen. Don't exit fullscreen, do ALT+F4", true);
            EmulatorsList.Add(VBA_M);

            //RPCS3
            //Need to activate close when process finishes and fullscreen
            var RPCS3 = new Emulator("RPCS3", "rpcs3.exe", "PS3 Games (*.bin)");
            RPCS3.DescriptionChange("Activate RPCS3 close when process finish and fullscreen");
            EmulatorsList.Add(RPCS3);

            //PCSX2
            //Works as expected
            EmulatorsList.Add(new Emulator("PCSX2", "pcsx2.exe", "PS2 Games (*.iso; *.mdf; *.nrg; *.bin; *.img; *.dump; *.gz; *.csp)", "", "", "--fullscreen --nogui"));

            //To find if emulator shortcut exist for easy use of Shortcutes
            ShortcutsFinder();
        }

        public static void ShortcutsFinder(Emulator emu = null)
        {
            List<Emulator> emulist = EmulatorsList;
            if (emu != null)
                emulist = new List<Emulator> { emu };

            GetLnkFiles(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            GetLnkFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) + @"\Programs");
            GetLnkFiles(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + @"\Programs");
            GetLnkFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar");

            foreach (var shortcut in Shortcuts)
            {
                IWshShortcut lnk = new WshShell().CreateShortcut(shortcut);
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
        readonly string argumentsP1;
        readonly string argumentsPmid;
        readonly string argumentsP2;
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

        public Emulator(string Name, string Exe, string Filters, string ArgumentsP1, string ArgumentsPmid, string ArgumentsP2, bool waitWindowChange = false)
        {
            name = Name;
            exe = Exe;
            argumentsP1 = ArgumentsP1 + " \\\"";
            argumentsPmid = ArgumentsPmid;
            argumentsP2 = "\\\" " + ArgumentsP2;
            gamesfilters = Filters + "|" + Filters.Split('(', ')')[1];
            WaitWindowChangeP = waitWindowChange;
        }

        public Emulator(string Name, string Exe, string Filters)
        {
            name = Name;
            exe = Exe;
            argumentsP1 = "\\\"";
            argumentsP2 = "\\\"";
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

        public virtual string Path(string path = null)
        {
            if (path != null & System.IO.File.Exists(path + exe))
                InstallPath = path;
            else if (string.IsNullOrWhiteSpace(path) && !System.IO.File.Exists(InstallPath + exe))
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
                if (!System.IO.File.Exists(ConfigPath))
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
                        if(Exe == "Ryujinx.exe")
                        {
                            try
                            {
                                List<string> File = System.IO.File.ReadAllText(ConfigPath).Split(',').ToList();
                                string value = File.Where(t => t.Contains("\"" + ConfigSection + "\":")).FirstOrDefault();
                                value = ShittyJSONvalue(value, new string[] { "\"" + ConfigSection + "\": [", "]," });
                                value = value.Substring(value.IndexOf(',') + 1);
                                value = value.Substring(value.IndexOf('"'));
                                value = value.Substring(value.IndexOf('"'));
                                value = ShittyJSONvalue(value, new string[] { ",", "]", Environment.NewLine, "\"" });
                                value = value.Replace(@"\\", @"\");

                                gamesPath = value;
                            }
                            catch { }
                        }
                        break;
                    default:
                        break;
                }
            }
            Debug.WriteLine(gamesPath);
            return gamesPath;
        }

        private string ShittyJSONvalue(string text, string[] concurrences, string replacement = "")
        {
            foreach (var item in concurrences)
                text = text.Replace(item, replacement);
            return text;
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
            return argumentsP1 + argumentsPmid + gamedir.Replace(@"\", @"\\") + argumentsP2;
        }
    }
}
