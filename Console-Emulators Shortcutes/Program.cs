using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Console_Emulators_Shortcutes
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
            Application.Run(new Form1());
        }
    }

    public class EmuPath
    {
        private string emu;
        private string path;
        private bool dbhaspath = false;

        public string Emu { get => emu; set => emu = value; }
        public string Path { get => path; set => path = value; }

        private SqlConnection cnn()
        {
            try
            {
                var cnn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\emupath.mdf;Integrated Security=True";
                var connection = new SqlConnection(cnn);
                return connection;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }

        public EmuPath(string Emulador)
        {
            try
            {
                Emu = Emulador;
                string query = "SELECT path FROM emupath WHERE emulador = '" + Emu + "'";
                string result = null;

                using (var connection = cnn())
                {
                    var command = new SqlCommand(query, connection);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = reader[0].ToString();
                            dbhaspath = true;
                        }
                    }
                }

                Path = result;
            }
            catch (Exception)
            {
                path = "";
            }
        }

        public void UpdatePath(string RPath)
        {
            try
            {
                Path = RPath;
                if (!dbhaspath)
                {
                    string insertdata = "INSERT INTO emupath(emulador, path) ";
                    insertdata += "VALUES('" + Emu + "', '" + Path + "')";
                    using (var connection = cnn())
                    {
                        connection.Open();
                        var insertcommand = new SqlCommand(insertdata, connection);
                        insertcommand.ExecuteNonQuery();
                    }
                    dbhaspath = true;
                }
                else
                {
                    string updatedata = "UPDATE emupath ";
                    updatedata += "SET path = '" + Path + "' WHERE emulador = '" + Emu + "'";
                    using (var connection = cnn())
                    {
                        connection.Open();
                        var updatecommand = new SqlCommand(updatedata, connection);
                        updatecommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
