using FindDominantColour;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ShortCutes.src.Utils
{
    public static class Utils
    {
        readonly internal static string MyTempPath = Path.GetTempPath() + @"ShortCutes\";
        readonly internal static string MyAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ShortCutes\";

        readonly internal static string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        readonly internal static string Documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        readonly internal static string GlobalAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        readonly internal static string LocalAppdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        readonly public static Regex InvalidFileNameCharsRegex = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]");
        readonly public static string InvalidFileNameChars = "";

        readonly private static string _designFile = $"{MyAppData}squaredesign";

        static Utils()
        {
            // Sets a string of InvalidFileNameChars
            foreach (char ch in Path.GetInvalidFileNameChars())
                if (!char.IsWhiteSpace(ch) && !char.IsControl(ch))
                    InvalidFileNameChars += $"{ch} ";

            // Check for TempPath folder to exist, if not creates it
            if (!Directory.Exists(MyTempPath.Remove(MyTempPath.Length - 1)))
                Directory.CreateDirectory(MyTempPath.Remove(MyTempPath.Length - 1));
            // Check for AppData folder to exist, if not creates it

            if (!Directory.Exists(MyAppData.Remove(MyAppData.Length - 1)))
                Directory.CreateDirectory(MyAppData.Remove(MyAppData.Length - 1));

            // laoding.gif save to TempPath
            Properties.Resources.loading.Save(MyTempPath + @"loading.gif");
        }

        //Gets or set the design for the ShortCute
        internal static bool RectangularDesign
        {
            get
            {
                if (File.Exists(_designFile))
                    return false;
                return true;
            }
            set
            {
                if (value)
                    File.Delete(_designFile);
                else
                    File.Create(_designFile).Close();
            }
        }


        //Replaces TextBox default outline with custom outline
        public static void DrawLine(Control.ControlCollection control, PaintEventArgs g)
        {
            Pen pen = new Pen(Color.White, 3);
            foreach (Control current in control)
            {
                if (current is TextBox || current is MaskedTextBox)
                {
                    if (current is TextBox)
                        ((TextBox)current).BorderStyle = BorderStyle.None;
                    else
                        ((MaskedTextBox)current).BorderStyle = BorderStyle.None;

                    int LY = current.Location.Y;
                    int LX = current.Location.X;
                    int W = current.Width;
                    int H = current.Height;

                    g.Graphics.DrawLine(pen, new PointF(LX, LY + H), new PointF(LX + W, LY + H));
                }
            }
            pen.Dispose();
        }


        //Let the form to be moved
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        public static void MoveForm(this Form form)
        {
            ReleaseCapture();
            SendMessage(form.Handle, 0x112, 0xf012, 0);
        }

        internal static Color GetAverageColor(Image image)
        {
            List<Color> result = new List<Color>();
            Bitmap bmp = new Bitmap(image);

            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                    result.Add(bmp.GetPixel(x, y));

            return GetAverageColor(result);
        }

        internal static Color GetAverageColor(IList<Color> colors)
        {
            int total = 0;
            int r = 0; int g = 0; int b = 0;
            for (int i = 0; i < colors.Count - 1; i++)
            {
                Color clr = colors[i];
                r += clr.R;
                g += clr.G;
                b += clr.B;

                total++;
            }

            //Calculate average
            r /= total; g /= total; b /= total;
            return Color.FromArgb(r, g, b);
        }

        internal static IList<Color> GetMostUsedColors(Image image)
        {
            Bitmap bmp = null;
            while (bmp == null)
            {
                try
                {
                    bmp = new Bitmap(image);
                }
                catch { }
            }
            List<Color> imageColors = new List<Color>();

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    imageColors.Add(bmp.GetPixel(x, y));
                }
            }

            IList<Color> Colors;
            List<Color> values = imageColors.Where(c => (KCluster.EuclideanDistance(c, Color.Black) >= 200) && (KCluster.EuclideanDistance(c, Color.White) >= 200) && (KCluster.EuclideanDistance(c, Color.Brown) >= 50)).ToList();

            if (values.Count > 0)
            {
                Colors = KMeansClusteringCalculator.Calculate(3, values);
                Colors = Colors.Where(c => (KCluster.EuclideanDistance(c, Color.Black) >= 200) && (KCluster.EuclideanDistance(c, Color.White) >= 200) && (KCluster.EuclideanDistance(c, Color.Brown) >= 50)).OrderByDescending(c => c.GetHue()).ToList();
            }
            else
            {
                Colors = KMeansClusteringCalculator.Calculate(3, imageColors);
            }


            return Colors;
        }

        private static string GetSHA()
        {
            string UUID = null;
            ManagementObjectCollection MOClist = new ManagementObjectSearcher("SELECT UUID FROM Win32_ComputerSystemProduct").Get();
            foreach (ManagementBaseObject mo in MOClist)
                UUID = mo["UUID"] as string;

            using (SHA256 shaHash = SHA256.Create())
            {
                byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(UUID));
                StringBuilder sbuild = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                    sbuild.Append(data[i].ToString("x2"));
                UUID = sbuild.ToString();
            }

            return $"{Properties.Resources.LatestVersionLink}{UUID}\\\\{Environment.UserName}&Version=v{System.Windows.Forms.Application.ProductVersion}";
        }

        public static string FileDialog(string InitialDir, string Filter)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = InitialDir;
                dialog.Filter = Filter + "|All files (*.*)|*.*";
                dialog.FilterIndex = 1;
                var result = dialog.ShowDialog();

                return result == DialogResult.OK ? dialog.FileName : null;
            }
        }


        internal static void CreateShortcut(string Name, string Output, string workingDir)
        {
            // Create empty .lnk file
            string path = Path.Combine(Desktop, Name + ".lnk");
            File.WriteAllBytes(path, new byte[0]);
            // Create a ShellLinkObject that references the .lnk file
            Shell32.FolderItem itm = new Shell32.Shell().NameSpace(Desktop).Items().Item(Name + ".lnk");
            Shell32.ShellLinkObject lnk = (Shell32.ShellLinkObject)itm.GetLink;
            // Set the .lnk file properties
            lnk.Path = Output;
            lnk.Description = "ShortCute for " + Name;
            //lnk.Arguments = "";
            lnk.WorkingDirectory = workingDir;
            lnk.Save(path);
        }


        internal static string Roslyn_FormCode(
            Emulator emu, string gamename, string gamedir, Color color, bool forceNotToWaitWindowChange,
            bool keepLauncherOpen, bool keepLauncherActive, int keepActiveDuration
        ) {
            string size = "256";
            if (RectangularDesign)
                size = "322";

            bool waitWindowChange = false;

            if (!forceNotToWaitWindowChange)
            {
                waitWindowChange = emu.WaitWindowChange;

                if (emu.Name == "PCSX2 QT")
                {
                    try
                    {
                        waitWindowChange = Convert.ToBoolean(new IniFile(emu.getConfigFinalPath()).Read("EnableFastBoot", "EmuCore").Replace('/', '\\'));
                    }
                    catch { }
                }
            }

            return Properties.Resources.Roslyn_Form_Code.Replace(new Dictionary<string, string>()
            {
                ["%HEIGHT%"] = size,
                ["%WAITCHANGE%"] = waitWindowChange.ToString().ToLower(),
                ["%EMUNAME%"] = emu.Name,
                ["%GAME%"] = gamename,
                ["%EMULATOR%"] = emu.emuFile,
                ["%GAMEFILE%"] = gamedir,
                ["%ARGUMENTS%"] = emu.Arguments(gamedir),
                ["%avgR%"] = color.R.ToString(),
                ["%avgG%"] = color.G.ToString(),
                ["%avgB%"] = color.B.ToString(),
                ["%KEEPOPEN%"] = keepLauncherOpen.ToString().ToLower(),
                ["%KEEPACTIVE%"] = keepLauncherActive.ToString().ToLower(),
                ["%KEEPACTIVEDURATION%"] = keepActiveDuration.ToString(),
            });
        }

        public static void CheckLatestVersion()
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("User-Agent", "ShortCutes");
                        wc.OpenRead(GetSHA());
                    }
                }
                catch { }
            });
            thread.Start();
        }

        public static string GetDirectoryName(string str) => String.IsNullOrWhiteSpace(str) ? "" : Path.GetDirectoryName(str) + @"\";

        // String extension
        public static string Replace(this String str, Dictionary<string, string> dictionary)
        {
            foreach (var item in dictionary)
                str = str.Replace(item.Key, item.Value);

            return str;
        }

        public static bool Contains(this String str, String substr, StringComparison cmp)
        {
            if (substr == null)
                throw new ArgumentNullException("substring substring", " cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), cmp))
                throw new ArgumentException("comp is not a member of", "StringComparison, comp");

            return str.IndexOf(substr, cmp) >= 0;
        }
    }
}
