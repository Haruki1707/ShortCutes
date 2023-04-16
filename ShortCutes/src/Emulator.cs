using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShortCutes.src.Utils;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ShortCutes.src
{
    class Emulator
    {
        readonly string name;
        readonly string exe;
        readonly string arguments;
        readonly string gamesfilters;
        string file;
        string description = "Works as expected";
        string InstallPath = null;
        string gamesPath = null;
        Color cdesc = Color.LightGreen;

        string[] ConfigPaths = null;
        string ConfigFile = null;
        string ConfigSection = null;
        string ConfigElement = null;
        readonly bool WaitWindowChangeP = false;

        public string Name { get => name; }
        public string Exe { get => exe; }
        public string emuFile { get => file; }
        public string Gamesfilters { get => gamesfilters; }
        public string Description { get => description; }
        public string GamesPath { get => gamesPath; }
        public Color Cdesc { get => cdesc; }
        public bool WaitWindowChange { get => WaitWindowChangeP; }
        public string[] DefaultLocations { get; set; }

        public Emulator(string Name, string Exe, string Filters, string Arguments, bool waitWindowChange = false)
        {
            name = Name;
            exe = Exe;
            arguments = Arguments;
            gamesfilters = Filters + "|" + Filters.Split('(', ')')[1];
            WaitWindowChangeP = waitWindowChange;
        }

        public Emulator(string Name, string Exe, string Filters)
        {
            name = Name;
            exe = Exe;
            arguments = "%GAME% %USERARGS%";
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

        public string GetPath()
        {
            if (!File.Exists(InstallPath + emuFile))
                Emulators.ShortcutsFinder(this);
            return InstallPath + emuFile;
        }

        private void SetPath(string path = null)
        {

            string fileName = Path.GetFileName(path);
            if (path != null & File.Exists(path))
            {
                InstallPath = path.Replace(fileName, "");
                file = fileName;
            }
            else if (string.IsNullOrWhiteSpace(path) && !File.Exists(InstallPath))
                Emulators.ShortcutsFinder(this);
        }

        public bool CheckPath(string path, bool setPath = true)
        {
            string emuExePattern = Exe.ToLower().Replace(".", @".*\.");
            Regex rg = new Regex(emuExePattern);
            MatchCollection matchedEmu = rg.Matches(Path.GetFileName(path).ToLower());

            if (matchedEmu.Count > 0)
            {
                if (setPath)
                    SetPath(Path.GetFullPath(path));
                return true;
            }

            return false;
        }

        internal string getInstallPath()
        {
            return InstallPath;
        }

        public string TryGetGamesPath()
        {
            if (ConfigFile != null)
            {
                string finalPath = getConfigFinalPath();

                switch (Path.GetExtension(finalPath))
                {
                    case ".xml":
                        try
                        {
                            XDocument doc = XDocument.Load(finalPath);
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
                            string Directory;
                            if (ConfigSection == null)
                                Directory = new NoSectionIniFile(finalPath).Read(ConfigElement).Replace('/', '\\');
                            else
                                Directory = new IniFile(finalPath).Read(ConfigElement, ConfigSection).Replace('/', '\\');

                            if (Directory != null)
                                gamesPath = Directory;
                        }
                        catch { }
                        break;
                    case ".json":
                        try
                        {
                            var json = JsonConvert.DeserializeObject<JToken>(File.ReadAllText(finalPath));
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

        public string getConfigFinalPath()
        {
            string finalPath = null;

            if (ConfigPaths != null)
                foreach (string path in ConfigPaths)
                {
                    string subpath = path.Replace("%SELF%", InstallPath);
                    if (File.Exists($"{subpath}\\{ConfigFile}"))
                    {
                        finalPath = $"{subpath}\\{ConfigFile}";
                        break;
                    }
                }

            if (finalPath == null && File.Exists(InstallPath + ConfigFile))
                finalPath = InstallPath + ConfigFile;

            if (finalPath == null || !File.Exists(finalPath))
                return null;

            return finalPath;
        }

        public void SetConfigPath(string[] dirs, string File, string Section, string Element)
        {
            ConfigPaths = dirs;
            ConfigFile = File;
            ConfigSection = Section;
            ConfigElement = Element;
        }

        public string Arguments(string gamedir)
        {
            string args = arguments.Replace("%USERARGS%", "*USERARGS*");
            args = args.Replace("%", "\\\"");
            args = args.Replace("*", "%");
            return args.Replace("GAME", gamedir.Replace(@"\", @"\\"));
        }
    }
}
