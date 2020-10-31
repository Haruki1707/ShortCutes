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
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

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
            Edirbox.Select();
            Shortcutbox.Select();
            if (emulatorcb.SelectedItem != null)
            {
                emu = emulatorcb.SelectedItem.ToString();
                foreach (var sEmu in selectedEmu)
                {
                    if (sEmu.Emu == emu)
                    {
                        isinlist = true;
                        Edirbox_textchange(sEmu.Path, sEmu.Emu);
                        actual = sEmu;
                    }
                }

                if (!isinlist)
                {
                    var insert = new EmuPath(emu);
                    selectedEmu.Add(insert);
                    Edirbox_textchange(insert.Path, insert.Emu);
                    actual = insert;
                }
            }
        }

        private void Edirbox_textchange(string text, string emu)
        {
            string emucompare = emu;
            if (actual != null)
                emucompare = actual.Emu;


            if (emu != emucompare)
                Edirbox.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string emuselected = null;
            if(emulatorcb.SelectedItem != null)
                emuselected = emulatorcb.SelectedItem.ToString();

            string emulatorpath = Edirbox.Text;
            if (!emulatorpath.EndsWith("\\"))
                emulatorpath = emulatorpath + @"\";

            string code = Emulator(emuselected, Gdirbox.Text, emulatorpath);

            if (code == "false")
                return;
            else
            {
                if (!Image)
                {
                    Error("Select a picture to continue.");
                    return;
                }
                Regex containsABadCharacter = new Regex("["
                + Regex.Escape(new string(Path.GetInvalidPathChars()))
                + "]");
                if (containsABadCharacter.IsMatch(Shortcutbox.Text))
                {
                    char[] invalidFileChars = Path.GetInvalidFileNameChars();

                    Error("Invalid filename!\n Cannot contain: " + string.Join(", ", invalidFileChars));
                    return;
                }
                Compile(code, emulatorpath, Shortcutbox.Text);
                ICOpic.Image = null;
                Image = false;
                actual.UpdatePath(emulatorpath);
                Edirbox.Text = emulatorpath;
                Gdirbox.Text = null;
                Shortcutbox.Text = null;
                return;
            }
        }

        private void Compile(string code, string emupath, string Filename)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();

            emupath += @"shortcuts";
            if (!Directory.Exists(emupath))
                Directory.CreateDirectory(emupath);
            emupath += @"\";

            string Output = emupath + Shortcutbox.Text +".exe";

            CompilerParameters parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.dll" });
            parameters.CompilerOptions = "-win32icon:temp.ico";
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
                using (var shellShortcut = new ShellLink(newShortcutPath)
                {
                    Path = path
                    WorkingDirectory = workingDir,
                    Arguments = args,
                    IconPath = iconPath,
                    IconIndex = iconIndex,
                    Description = description,
                })
                {
                    shellShortcut.Save();
                }

                if (Success("Shortcut created!\nExecute shortcut?") == DialogResult.Yes)
                {
                    Process.Start(Output);
                }
            }
        }

        private string Emulator(string option, string gamedir, string emulatordir)
        {
            string emulatorchecker = null;
            string gamechecker = null;

            StringComparison comp = StringComparison.OrdinalIgnoreCase;
            if (gamedir.Contains(emulatordir, comp))
            {
                emulatorchecker = emulatordir;
                gamechecker = gamedir;

                gamedir = gamedir.Replace(emulatordir, @"");
                gamedir = gamedir.Replace(@"\", @"\\");
                emulatordir = @"..\\";
            }
            else
            {
                Error("Rom (file or folder) must be in the same directory of the emulator\n" + 
                    "Example:\n" + 
                    "  Emulator: " + emulatordir + "\n" +
                    "  Rom: "  + emulatordir + "\\folder name\\name.file");
                return "false";
            }

            string code = "using System;\n" +
                          "using System.Diagnostics;\n" +
                          "namespace Emulator_Shortcuts\n" +
                          "{\n" + 
                            "class Program\n" +
                            "{\n" +
                                "static void Main()\n" +
                                "{\n" +
                                    "Console.WriteLine(\"Emulator ShortCutes UwU \\nDesign by Haruki1707.  \\nExecuting ShortCute...\");";
            
            switch (option)
            {
                case "CEMU":
                    emulatordir += "Cemu.exe";
                    emulatorchecker += "Cemu.exe";

                    code += "Process Cemugame = new Process();\n"+
                            "Cemugame.StartInfo.FileName = \"" + emulatordir + "\";\n" +
                            "Cemugame.StartInfo.Arguments = \"-g \\\""+gamedir+"\\\" -f\";\n" +
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

                if (!File.Exists(emulatorchecker))
                {
                    Error("Emulator don't exist in the specified path\nCheck if path or selected emulator is correct");
                    return "false";
                }

                if (!File.Exists(gamechecker))
                {
                    Error("Game file don't exist in the specified path");
                    return "false";
                }
            }

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
        private DialogResult Success(string message)
        {
            DialogResult result = MessageBox.Show(message, "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            return result;
        }

        private void emuBrow_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            if (Edirbox.Text != @"")
                dialog.InitialDirectory = Edirbox.Text;
            else
                dialog.InitialDirectory = "C:\\";
            dialog.IsFolderPicker = true;
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                Edirbox.Text = dialog.FileName;
        }

        private void gameBrow_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            if (Edirbox.Text != @"")
                dialog.InitialDirectory = Edirbox.Text;
            else
                dialog.InitialDirectory = "C:\\";
            dialog.IsFolderPicker = false;
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                Gdirbox.Text = dialog.FileName;
        }

        private static bool Image = false;
        private void ICOpic_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.IsFolderPicker = false;
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = Directory.GetCurrentDirectory();
                ImagingHelper.ConvertToIcon(dialog.FileName, path + @"\temp.ico");
                ICOpic.Image = ImagingHelper.ICONbox;
                Image = true;
            }
        }

        private void Edirbox_Click(object sender, EventArgs e)
        {
            emuBrow.PerformClick();
        }

        private void Gdirbox_Click(object sender, EventArgs e)
        {
            gameBrow.PerformClick();
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
