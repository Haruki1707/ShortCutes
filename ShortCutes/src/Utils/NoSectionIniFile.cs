using System.IO;

namespace ShortCutes.src.Utils
{
    internal class NoSectionIniFile
    {
        string Path;

        public NoSectionIniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath).FullName;
        }

        public string Read(string Key)
        {
            using (StreamReader sr = new StreamReader(Path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(new char[] { ',', '=' });
                    if (parts[0] == Key)
                        return parts[1];
                }
            }
            return null;
        }
    }
}
