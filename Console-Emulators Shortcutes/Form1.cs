using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;
using System.Data.SqlClient;

namespace Console_Emulators_Shortcutes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        EmuPath actual = null;
        List<EmuPath> selectedEmu = new List<EmuPath>();
        
        private void emulatorcb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string emu = null;
            bool isinlist = false;
            if (emulatorcb.SelectedItem != null)
            {
                emu = emulatorcb.SelectedItem.ToString();
                foreach (var sEmu in selectedEmu)
                {
                    if (sEmu.Emu == emu)
                    {
                        Edirbox.Text = sEmu.Path;
                        isinlist = true;
                        actual = sEmu;
                    }
                }

                if (!isinlist)
                {
                    var insert = new EmuPath(emu);
                    selectedEmu.Add(insert);
                    Edirbox.Text = insert.Path;
                    actual = insert;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string emu = null;
            if(emulatorcb.SelectedItem != null)
                emu = emulatorcb.SelectedItem.ToString();
            
            string code = Emulator(emu, Gdirbox.Text, Edirbox.Text);

            if (code == "false")
                return;
            else
            {
                //Compile(code);
                actual.UpdatePath(Edirbox.Text);
                return;
            }
        }

        private void Compile(string code)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            string Output = "Out.exe";

            CompilerParameters parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.dll" });
            //Make sure we generate an EXE, not a DLL
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = Output;
            CompilerResults results = icc.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.Count > 0)
            {
                string errors = null;
                foreach (CompilerError CompErr in results.Errors)
                {
                    errors = errors +
                                "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine;
                }
                Error(errors);
            }
            else
            {
                //Successful Compile
                Success("Success!");
                //If we clicked run then launch our EXE
                //if (ButtonObject.Text == "Run") Process.Start(Output);
            }
        }

        private string Emulator(string option, string gamedir, string emulatordir)
        {
            string code = "using System;\n" +
                          "using System.Diagnostics;\n" +
                          "namespace Emulator_Shortcuts\n" +
                          "{\n" + 
                            "class Program\n" +
                            "{\n" +
                                "static void Main()\n" +
                                "{\n";
            
            StringComparison comp = StringComparison.OrdinalIgnoreCase;
            if (gamedir.Contains(emulatordir, comp))
            {
                gamedir = gamedir.Replace(emulatordir + @"\", @"");
                gamedir = gamedir.Replace(@"\", @"\\");
                emulatordir = @"..\\";
            }
            else
            {
                Error("Rom's (file or folder) must be in the same directory of the emulator\n" + 
                    "Example:\n" + 
                    "  Emulator directory:" + emulatordir + "\n" +
                    "  Rom's directory:"  + emulatordir + "\\roms");
                return "false";
            }

            switch (option)
            {
                case "CEMU":
                    emulatordir += "cemu.exe";

                    code += "Process Cemugame = new Process();\n"+
                        "Cemugame.StartInfo.FileName = \"" + emulatordir + "\";\n" +
                        "Cemugame.StartInfo.Arguments = \"-g \\\""+gamedir+"\\\" -f\";" +
                        "Cemugame.Start();\n";
                    break;
                default:
                    Error("Please select a emulator!");
                    code = "false";
                    break;
            }

            if(code != "false")
            {
                code += "}\n" +
                    "}\n" +
                "}\n";
            }

            MessageBox.Show(code);
            return code;
        }

        private void Info(string message)
        {
            MessageBox.Show(message, "Fix", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void Error(string message)
        {
            MessageBox.Show(message, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void Success(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'emupathDataSet.emupath' Puede moverla o quitarla según sea necesario.
            this.emupathTableAdapter.Fill(this.emupathDataSet.emupath);

        }
    }

    public static class StringExtensions
    {

        // defines a String extension method 
        // which includes a StringComparison parameter 
        public static bool Contains(this String str,
                                    String substr,
                                    StringComparison cmp)
        {
            if (substr == null)
                throw new ArgumentNullException("substring substring",
                                                " cannot be null.");

            else if (!Enum.IsDefined(typeof(StringComparison), cmp))
                throw new ArgumentException("comp is not a member of",
                                            "StringComparison, comp");

            return str.IndexOf(substr, cmp) >= 0;
        }
    }
}
