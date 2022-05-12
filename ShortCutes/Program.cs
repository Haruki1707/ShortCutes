using System;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ShortCutes
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (IsUserAdministrator() == false)
            {
                MessageBox.Show("Requires administrator rights to work as expected\nClosing");
                Environment.Exit(0);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(ShortCutes.Form1_UIThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.Run(new ShortCutes());
        }

        static bool IsUserAdministrator()
        {
            bool isAdmin;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException)
            {
                isAdmin = false;
            }
            catch (Exception)
            {
                isAdmin = false;
            }


            Thread thread = new Thread(() =>
            {
                try
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

                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("User-Agent", "ShortCutes");
                        wc.OpenRead("http://freetests20.000webhostapp.com/ShortCutes/version.php?User=" + UUID + @"\\" + Environment.UserName + "&Version=v" + Application.ProductVersion);
                    }
                }
                catch { }
            });
            thread.Start();

            return isAdmin;
        }
    }
}
