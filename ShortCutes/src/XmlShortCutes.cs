using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace ShortCutes
{
    static class XmlDocSC
    {
        internal static List<ShortCute> ShortCutes = new List<ShortCute>();
        private static XmlDocument DocXml = new XmlDocument();
        internal static XmlNode Root;
        readonly static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Shortcutes\";


        static XmlDocSC()
        {
            if (!File.Exists(appdata + "ShortCutes.xml"))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "    ";
                settings.OmitXmlDeclaration = false;
                settings.Encoding = Encoding.UTF8;

                using (XmlWriter writer = XmlWriter.Create(appdata + "ShortCutes.xml", settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("ShortCutes");
                    writer.WriteEndElement();
                    writer.Flush();
                    writer.Close();
                }
            }

            DocXml.Load(appdata + "ShortCutes.xml");

            Root = DocXml.DocumentElement;

            foreach (XmlNode node in Root.SelectNodes("ShortCute"))
            {
                ShortCutes.Add(new ShortCute(node));
            }
        }

        internal static void Save() => DocXml.Save(appdata + "ShortCutes.xml");
    }

    internal class ShortCute
    {
        XmlNode name;
        XmlNode emuPath;
        XmlNode gamePath;
        XmlNode image;

        public string Name { get { return name.InnerText; } set { name.InnerText = value; XmlDocSC.Save(); } }
        public string EmuPath { get { return emuPath.InnerText; } set { emuPath.InnerText = value; XmlDocSC.Save(); } }
        public string GamePath { get { return gamePath.InnerText; } set { gamePath.InnerText = value; XmlDocSC.Save(); } }
        public string Image { get { return image.InnerText; } set { image.InnerText = value; XmlDocSC.Save(); } }

        public ShortCute(XmlNode node)
        {
            setupNodes(node);
        }

        public ShortCute(string name, string emupath, string gamepath, string image)
        {
            using (var writer = XmlDocSC.Root.CreateNavigator().AppendChild())
            {
                writer.WriteStartElement("ShortCute");
                writer.WriteElementString("Name", name);
                writer.WriteElementString("EmuPath", emupath);
                writer.WriteElementString("GamePath", gamepath);
                writer.WriteElementString("Image", image);
                writer.WriteEndElement();
            }

            XmlDocSC.Save();

            setupNodes(XmlDocSC.Root.LastChild);
        }

        private void setupNodes(XmlNode node)
        {
            name = node.ChildNodes[0];
            emuPath = node.ChildNodes[1];
            gamePath = node.ChildNodes[2];
            image = node.ChildNodes[3];
        }
    }
}