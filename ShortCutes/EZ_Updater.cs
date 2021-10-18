using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortCutes
{
    /// <summary>
    /// Provides easy way to update your portable application from GitHub Releases
    /// </summary>
    static class EZ_Updater
    {
        static EZ_Updater()
        {
            if (File.Exists(ProgramFileBak))
                File.Delete(ProgramFileBak);
        }

        readonly private static string ProgramFilePath = Process.GetCurrentProcess().MainModule.FileName;
        readonly private static string ProgramFileName = Path.GetFileName(ProgramFilePath);
        readonly private static string ProgramFileBak = Path.GetFileNameWithoutExtension(ProgramFilePath) + ".bak";
        readonly private static string ProgramVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        /// <summary>
        /// Gets your user and repo from GitHub for program update (must be "USER/REPOSITORY")
        /// </summary>
        /// <returns>User/Repository</returns>
        public static string GitHubRep
        {
            get
            {
                return GitHubrepo;
            }
            set
            {
                GitHubrepo = value;
                GitHubrepoDOWNLOAD = "https://github.com/" + value + "/releases/latest/download/" + ProgramFileName;
                GitHubrepoAPI = "https://api.github.com/repos/" + value + "/releases/latest";
            }
        }
        private static string GitHubrepo = null;
        private static string GitHubrepoAPI = null;
        private static string GitHubrepoDOWNLOAD = null;
        readonly private static Regex rg = new Regex(@"(?<digit>\d+)");

        /// <summary>
        /// Check for the program updates at GitHub Latest Release (Doesn't count pre-releases and drafts) using Semantic versioning
        /// || Lack of Alpha and Beta Support (later implementation) ||
        /// </summary>
        /// <param name="GitHubrepository">Gets your user and repo from GitHub for program update (must be "USER/REPOSITORY")</param>
        /// <returns>True in case there is a new update, False if not</returns>
        public static bool CheckUpdate(string GitHubrepository = null)
        {
            if (GitHubrepository != null)
                GitHubRep = GitHubrepository;

            string json;
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("User-Agent", GitHubrepo);
                wc.Headers.Add("Accept", "application/vnd.github.v3+json");
                json = wc.DownloadString(GitHubrepoAPI);
            }

            List<string> File = json.Split(',').ToList();


            string stringline = File.Where(t => t.Contains("\"tag_name\":")).FirstOrDefault();
            stringline = stringline.Replace("\"tag_name\":", "");
            stringline = stringline.Replace("\"", "");
            stringline = stringline.Replace(",", "");

            string TAG = "";
            MatchCollection MC = rg.Matches(stringline);

            foreach (Match m in MC)
            {
                CaptureCollection cc = m.Groups["digit"].Captures;
                foreach (Capture c in cc)
                {
                    TAG += c.Value;

                    if (m != MC[MC.Count - 1])
                        TAG += ".";
                }
            }

            string[] ARVersion = ProgramVersion.Split('.');
            string ActualVersion = "";

            for (int i = 0; i < ARVersion.Length; i++)
            {
                if (int.Parse(ARVersion[i]) != 0)
                    ActualVersion += ARVersion[i] + ".";
                else
                {
                    bool ImportantZero = false;
                    for (int j = i + 1; j < ARVersion.Length; j++)
                    {
                        if (int.Parse(ARVersion[j]) != 0)
                            ImportantZero = true;
                    }

                    if (ImportantZero == true)
                        ActualVersion += ARVersion[i] + ".";
                }
            }
            ActualVersion = ActualVersion.Substring(0, ActualVersion.Length - 1);

            if (TAG.CompareTo(ActualVersion) > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Updates the application wheter or not there is a new update
        /// </summary>
        /// <param name="DownloadProgressEvent">Event method to get DownloadProgressChangedEventArgs</param>
        /// <param name="RestartEvent">Event method to execute code after download had finished (like a application restart to apply update)</param>
        public static void Update(Action<object, DownloadProgressChangedEventArgs> DownloadProgressEvent = null, Action<object, AsyncCompletedEventArgs> RestartEvent = null)
        {
            if (File.Exists(ProgramFileBak))
                File.Delete(ProgramFileBak);

            File.Move(ProgramFileName, ProgramFileBak);

            WebClient client = new WebClient();

            if (DownloadProgressEvent != null)
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressEvent);
            if (RestartEvent != null)
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(RestartEvent);

            client.DownloadFileAsync(new Uri(GitHubrepoDOWNLOAD), ProgramFilePath);
        }
    }
}