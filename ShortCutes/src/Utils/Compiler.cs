using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortCutes.src.Utils
{
    internal static class Compiler
    {
        private static CodeCompileUnit unit = new CodeCompileUnit();
        private static StringWriter assemblyInfo = new StringWriter();

        static Compiler()
        {
            AddAttribute(typeof(AssemblyTitleAttribute), "%GAME% ShortCute for %EMULATOR%");
            AddAttribute(typeof(AssemblyVersionAttribute), Application.ProductVersion);
            AddAttribute(typeof(AssemblyProductAttribute), "ShortCute");
            AddAttribute(typeof(AssemblyCompanyAttribute), "Haruki1707");
            AddAttribute(typeof(AssemblyCopyrightAttribute), "Haruki1707");
            new CSharpCodeProvider().GenerateCodeFromCompileUnit(unit, assemblyInfo, new CodeGeneratorOptions());
            assemblyInfo.Close();
        }

        private static void AddAttribute(Type type, object value)
        {
            var attr = new CodeTypeReference(type);
            var decl = new CodeAttributeDeclaration(attr, new CodeAttributeArgument(new CodePrimitiveExpression(value)));
            unit.AssemblyCustomAttributes.Add(decl);
        }

        internal static bool Compile(string code, string Output, string game = null, string emulator = null)
        {
            CompilerParameters parameters = new CompilerParameters(
                new[] { "mscorlib.dll", "System.Core.dll", "System.dll", "System.Windows.Forms.dll", "System.Drawing.dll", "System.Runtime.InteropServices.dll" }
            ){
                CompilerOptions = "-win32icon:" + $"\"{Utils.MyTempPath}temp.ico\"" +
                    "\n -target:winexe " +
                    "\n -resource:" + $"\"{Utils.MyTempPath}temp.png\"" +
                    "\n -resource:" + $"\"{Utils.MyTempPath}loading.gif\"" +
                    "\n /optimize",
                GenerateExecutable = true,
                OutputAssembly = Output
            };

            CompilerResults results = new CSharpCodeProvider().CompileAssemblyFromSource(parameters, 
                new[] { code, assemblyInfo.ToString().Replace("%GAME%", game).Replace("%EMULATOR%", emulator)});

            if (results.Errors.Count > 0)
            {
                string errors = null;
                foreach (CompilerError CompErr in results.Errors)
                {
                    errors = errors +
                                "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine;
                }
                MessageForm.Error(errors);
                return false;
            }

            return true;
        }
    }
}
