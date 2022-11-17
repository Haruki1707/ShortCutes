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

        bool SelfConfig = false;
        string ConfigPath = null;
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
                            string Directory;
                            if (ConfigSection == null)
                                Directory = new NoSectionIniFile(ConfigPath).Read(ConfigElement).Replace('/', '\\');
                            else
                                Directory = new IniFile(ConfigPath).Read(ConfigElement, ConfigSection).Replace('/', '\\');

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

        public void SetConfigPath(string dir, string File, string Section, string Element)
        {
            ConfigPath = $"{dir}\\{File}";
            ConfigSection = Section;
            ConfigElement = Element;

            if (!System.IO.File.Exists(ConfigPath))
            {
                SelfConfig = true;
                ConfigPath = File;
            }
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
