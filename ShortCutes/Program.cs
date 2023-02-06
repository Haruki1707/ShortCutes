using ShortCutes.src.Utils;
using System;
using System.Security.Principal;
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

            Utils.CheckLatestVersion();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(ShortCutes.UIThreadException);
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
    }
}
