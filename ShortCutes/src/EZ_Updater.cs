using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ShortCutes
{
    /// <summary>
    /// Provides easy way to update your portable application from GitHub Releases
    /// </summary>
    static class EZ_Updater
    {
        readonly private static string ProgramFilePath = Process.GetCurrentProcess().MainModule.FileName;
        readonly private static string ProgramFileName = Path.GetFileName(ProgramFilePath);
        readonly private static string ProgramFileBak = Path.GetFileNameWithoutExtension(ProgramFilePath) + ".bak";
        readonly private static string ProgramVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private static bool AlreadyBak = false;
        /// <summary>
        /// Gets your user and repo from GitHub for program update (must be "USER/REPOSITORY")
        /// </summary>
        /// <returns>User/Repository</returns>
        public static string GitHubRep
        {
            get => GitHubrepo;
            set
            {
                GitHubrepo = value;
                GitHubrepoDOWNLOAD = "https://github.com/" + value + "/releases/latest/download/" + ProgramFileName;
                GitHubrepoAPI = "https://api.github.com/repos/" + value + "/releases/latest";
            }
        }
        public static string ActualVersion = "";
        private static string GitHubrepo = null;
        private static string GitHubrepoAPI = null;
        private static string GitHubrepoDOWNLOAD = null;
        private static WebClient client = new WebClient();
        readonly private static Regex rg = new Regex(@"(?<digit>\d+)");
        private static Action CanceledDownload = null;
        private static Action RetryDownloadAction = null;
        private static Action<object, DownloadProgressChangedEventArgs> DownloadProgressEvent = null;
        private static Action RestartEvent = null;
        private static Timer TimerDP = new Timer();
        private static int RetryCount = 0;

        static EZ_Updater()
        {
            if (File.Exists(ProgramFileBak))
                File.Delete(ProgramFileBak);

            TimerDP.Interval = 3000;
            TimerDP.Tick += RetryDownload;
        }

        /// <summary>
        /// Check for the program updates at GitHub Latest Release (Doesn't count pre-releases and drafts) using Semantic versioning, also calling a method after checking async for update
        /// || Lack of Alpha and Beta Support (later implementation) ||
        /// </summary>
        /// <param name="owner">Must be UI Form from where is being called (this)</param>
        /// <param name="AskForUpdate">Method going to be executed if Check Async Update returns there is a update</param>
        /// <param name="GitHubrepository">Gets your user and repo from GitHub for program update (must be "USER/REPOSITORY")</param>
        /// <returns>True in case there is a new update, False if not</returns>
        public static void CheckUpdate(Form owner, Action AskForUpdate, string GitHubrepository = null)
        {
            System.Threading.Thread CU = new System.Threading.Thread(() =>
            {
                if (CheckUpdate(GitHubrepository))
                    if (owner != null)
                        owner.BeginInvoke(AskForUpdate);
                    else
                        AskForUpdate();
            });
            CU.Start();
        }

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
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", GitHubrepo);
                    wc.Headers.Add("Accept", "application/vnd.github.v3+json");
                    json = wc.DownloadString(GitHubrepoAPI);
                }
            }
            catch
            {
                return false;
            }
            List<string> File = json.Split(',').ToList();


            string stringline = File.Where(t => t.Contains("\"tag_name\":")).FirstOrDefault();
            stringline = stringline.Replace("\"tag_name\":", "");
            stringline = stringline.Replace("\"", "");
            stringline = stringline.Replace(",", "");

            File = null;

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
            ActualVersion = "";
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
            try
            {
                string MacAddress = "";
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                    if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                        if (nic.GetPhysicalAddress().ToString() != "")
                            MacAddress = nic.GetPhysicalAddress().ToString();

                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", GitHubrepo);
                    wc.OpenRead("http://freetests20.000webhostapp.com/ShortCutes/version.php?User=" + MacAddress + @"\\" + Environment.UserName + "&Version=v" + ActualVersion);
                }
            }
            catch { }


            return TAG.CompareTo(ActualVersion) > 0;
        }

        /// <summary>
        /// Updates the application wheter or not there is a new update
        /// </summary>
        /// <param name="CanceledDownloadR">Method u want to execute when download is canceled (Obligatory)</param>
        /// <param name="RetryDownloadR">Method u want to execute when retrying download(Obligatory)</param>
        /// <param name="DownloadProgressEventR">Event method to get DownloadProgressChangedEventArgs</param>
        /// <param name="RestartEventR">Event method to execute code after download had finished (like a application restart to apply update)</param>
        public static void Update(Action CanceledDownloadR = null, Action RetryDownloadR = null, Action<object, DownloadProgressChangedEventArgs> DownloadProgressEventR = null, Action RestartEventR = null)
        {
            if (File.Exists(ProgramFileBak) && !AlreadyBak)
                File.Delete(ProgramFileBak);

            if (!AlreadyBak)
            {
                AlreadyBak = true;
                File.Move(ProgramFileName, ProgramFileBak);
            }

            CanceledDownload = CanceledDownloadR;
            RetryDownloadAction = RetryDownloadR;
            DownloadProgressEvent = DownloadProgressEventR;
            RestartEvent = RestartEventR;

            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(RestartTimer);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(CompletedFile);

            if (DownloadProgressEvent != null)
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressEvent);

            client.DownloadFileAsync(new Uri(GitHubrepoDOWNLOAD), ProgramFilePath);
        }

        private static void RestartTimer(object sender, DownloadProgressChangedEventArgs e)
        {
            RetryCount = 0;
            TimerDP.Stop();
            TimerDP.Interval = 3000;
            TimerDP.Start();
        }

        private static void RetryDownload(object sender, EventArgs e)
        {
            TimerDP.Interval = 5000;
            client.CancelAsync();
            client = new WebClient();
            if (RetryCount >= 4)
            {
                TimerDP.Tick -= RetryDownload;
                TimerDP.Tick += Canceled;
                TimerDP.Interval = 1500;
                return;
            }
            RetryCount++;
            Update(CanceledDownload, RetryDownloadAction, DownloadProgressEvent, RestartEvent);
            if (RetryDownloadAction != null)
                RetryDownloadAction();
        }

        private static void Canceled(object sender, EventArgs e)
        {
            TimerDP.Stop();
            client.Dispose();
            if (AlreadyBak)
            {
                AlreadyBak = false;
                if (File.Exists(ProgramFileName))
                    File.Delete(ProgramFileName);
                File.Move(ProgramFileBak, ProgramFileName);
            }
            if (CanceledDownload != null)
                CanceledDownload();
        }

        private static void CompletedFile(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                Timer TimerSC = new Timer
                {
                    Interval = 700,
                    Enabled = true,
                };
                TimerSC.Tick += Execute_Tick;
                TimerSC.Start();
            }
        }

        private static void Execute_Tick(object sender, EventArgs e)
        {
            if (RestartEvent != null)
                RestartEvent();
        }
    }
}