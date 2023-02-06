using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ShortCutes.src
{
    struct EmulatorJSON
    {
        public string Name { get; set; }
        public string Executable { get; set; }
        public string GameFilters { get; set; }
        public string Arguments { get; set; }
        public string Description { get; set; }
        public bool Warning { get; set; }
        public bool WaitForIt { get; set; }

        public EmulatorConfig? Config { get; set; }
        public string[] DefaultLocations { get; set; }
    }

    struct EmulatorConfig
    {
        public string[] Paths { get; set; }
        public string File { get; set; }
        public string Section { get; set; }
        public string Element { get; set; }
    }

    struct LnkInfo
    {
        public IWshRuntimeLibrary.IWshShortcut lnk { get; set; }
        public FileVersionInfo fileInfo { get; set; }
    }

    static class Emulators
    {
        public static readonly List<Emulator> EmulatorsList = new List<Emulator>();
        private static readonly Dictionary<string, LnkInfo> Shortcuts = new Dictionary<string, LnkInfo>();
        private static readonly Dictionary<string, string> Replacements = new Dictionary<string, string>()
        {
            ["%DOCUMENTS%"] = Utils.Utils.Documents,
            ["%APPDATA%"] = Utils.Utils.GlobalAppData,
            ["%LOCALAPPDATA%"] = Utils.Utils.LocalAppdata
        };

        static Emulators()
        {
            var EmulatorsJSON = JsonConvert.DeserializeObject<EmulatorJSON[]>(Properties.Resources.Emulators);

            foreach (var item in EmulatorsJSON)
            {
                Emulator Emulator;
                if (item.Arguments == null)
                    Emulator = new Emulator(item.Name, item.Executable, item.GameFilters);
                else
                    Emulator = new Emulator(item.Name, item.Executable, item.GameFilters, item.Arguments, item.WaitForIt);

                if (item.Description != null)
                    Emulator.DescriptionChange(item.Description, item.Warning);

                if (item.Config.HasValue)
                {
                    var Config = item.Config.Value;
                    string[] Paths = Config.Paths;

                    Paths = ReplacePaths(Paths);

                    Emulator.SetConfigPath(Paths, Config.File, Config.Section, Config.Element);
                }

                if (item.DefaultLocations != null)
                    for (int i = 0; i < item.DefaultLocations.Count(); i++)
                        item.DefaultLocations[i] = ReplacePaths(item.DefaultLocations[i]);
                Emulator.DefaultLocations = item.DefaultLocations;

                EmulatorsList.Add(Emulator);
            }

            //To find if emulator shortcut exist for easy use of Shortcutes
            ShortcutsFinder();
        }

        public static void ShortcutsFinder(Emulator emu = null)
        {
            List<Emulator> emulist = EmulatorsList;
            if (emu != null)
            {
                emulist = new List<Emulator> { emu };

                if (emu.DefaultLocations != null)
                    foreach (string location in emu.DefaultLocations)
                        if (emu.CheckPath(location + @"\\" + emu.Exe))
                            return;
            }

            GetLnkFiles(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            GetLnkFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) + @"\Programs");
            GetLnkFiles(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + @"\Programs");
            GetLnkFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar");

            foreach (var emulator in emulist)
            {
                bool setted = false;
                if (emulator.DefaultLocations != null)
                    foreach (string location in emulator.DefaultLocations)
                        setted = emulator.CheckPath(location + @"\\" + emulator.Exe);

                foreach (var shortcut in Shortcuts)
                {
                    if (shortcut.Value.fileInfo.ProductName == "ShortCute")
                        continue;

                    if (shortcut.Value.lnk != null)
                        if (setted == false && String.IsNullOrWhiteSpace(emulator.getInstallPath()))
                            emulator.CheckPath(shortcut.Value.lnk.TargetPath);
                }
            }
        }

        private static void GetLnkFiles(string Path)
        {
            var shortcuts = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories).Where(s => s.ToLower().EndsWith(".lnk"));
            foreach (var shortcut in shortcuts)
                if (!Shortcuts.ContainsKey(shortcut))
                    Shortcuts.Add(shortcut, GenerateLnkInfo(shortcut));
            shortcuts = null;
        }

        private static LnkInfo GenerateLnkInfo(string path)
        {
            FileVersionInfo tempInfo;
            IWshRuntimeLibrary.WshShortcut tempLnk = new IWshRuntimeLibrary.WshShell().CreateShortcut(path);

            if (File.Exists(tempLnk.TargetPath))
                tempInfo = FileVersionInfo.GetVersionInfo(tempLnk.TargetPath);
            else
                tempInfo = FileVersionInfo.GetVersionInfo(path);

            return new LnkInfo()
            {
                lnk = tempLnk,
                fileInfo = tempInfo
            };
        }

        private static string ReplacePaths(string path)
        {
            if (!string.IsNullOrEmpty(path))
                path = Utils.Utils.Replace(path, Replacements);
            return path;
        }

        private static string[] ReplacePaths(string[] paths)
        {
            if (paths == null)
                return paths;

            for (int i = 0; i < paths.Length; i++)
                paths[i] = ReplacePaths(paths[i]);
            return paths;
        }
    }
}
