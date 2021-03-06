﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Drawing;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ShortCutes());

            if(IsUserAdministrator() == false)
            {
                MessageBox.Show("Requires administrator rights to work as expected\nClosing");
                Environment.Exit(0);
            }
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
