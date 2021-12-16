using System;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace ShortCutes
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (IsUserAdministrator() == false)
            {
                MessageBox.Show("Requires administrator rights to work as expected\nClosing");
                Environment.Exit(0);
            }

            try
            {
                string MacAddress = "";
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                    if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                        if (nic.GetPhysicalAddress().ToString() != "")
                            MacAddress = nic.GetPhysicalAddress().ToString();

                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", "ShortCutes");
                    wc.OpenRead("http://freetests20.000webhostapp.com/ShortCutes/version.php?User=" + MacAddress + @"\\" + Environment.UserName + "&Version=v" + EZ_Updater.Updater.ProgramFileVersion);
                }
            }
            catch { }

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
            return isAdmin;
        }

        public static void ToDraw(Control.ControlCollection control, PaintEventArgs g)
        {
            var color = Color.White;
            Pen pen = new Pen(color, 3);
            foreach (Control current in control)
            {
                if (current is TextBox || current is MaskedTextBox)
                {
                    var LX = current.Location.X;
                    var W = current.Width;
                    var Y = current.Location.Y + current.Height;

                    g.Graphics.DrawLine(pen, new PointF(LX, Y), new PointF(LX + W, Y));
                }
            }
            pen.Dispose();
        }
    }
}
