using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortCutes
{
    static class Emulators
    {
        public static readonly List<Emulator> emus = new List<Emulator>();
        
        static Emulators()
        {
            //To add a emulator call 'new Emulator();'
            //Params: "Name of emulator", "name of exe.exe", "Game files that emulator supports", "Parameters before game file path", "Parameters in file path", "Parameters after game file path"

            //Works as expected
            emus.Add(new Emulator("CEMU", "Cemu.exe", "WiiU Games (*.rpx; *.wud; *.wux)|*.rpx;*.wud;*.wux", "-g", "", "-f"));
            //Works as expected
            emus.Add(new Emulator("Dolphin", "Dolphin.exe", "Wii/GC Games (*.iso; *.wbfs; *.ciso; *.gcz; *.gcm)|*.iso;*.wbfs;*.ciso;*.gcz;*.gcm", "-b -e", "", ""));
            //Works as expected
            emus.Add(new Emulator("SNES9X", "snes9x.exe", "SNES Games (*.smc; *.sfc; *.swc; *.zip)|*.smc;*.sfc;*.swc;*.zip", "", "..\\\\", "-fullscreen"));
            //Need to activate fullscreen through Emulator GUI
            emus.Add(new Emulator("PJ64", "Project64.exe", 
                "Nintendo 64 Games (*.n64; *.z64; *.v64; *.u64; *.zip; *.rar; *.rom; *.jap; *.pal; *.usa)|*.n64;*.z64;*.v64;*.u64;*.zip;*.rar;*.rom;*.jap;*.pal;*.usa"));
            emus.Last().DescriptionChange("Activate fullscreen through PJ64 GUI");
            //Need to activate fullscreen through Emulator GUI
            emus.Add(new Emulator("YUZU", "yuzu.exe","Switch Games (*.xci; *.nsp)| *.xci;*.nsp", "-f -g", "", ""));
            var Appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            emus.Last().Path(Appdata + @"\Yuzu\yuzu-windows-msvc\");
            //Only works on NightBuild
            emus.Add(new Emulator("VBA-M", "visualboyadvance-m.exe", "GB-GBC-GBA Games (*.gba; *.gbc; *.gb; *.zip)|*.gba;*.gbc;*.gb;*.zip", "/f", "", ""));
            emus.Last().DescriptionChange("Only works on nightbuild. Don't exit fullscreen, do ALT+F4", true);
            //Need to activate close when process finishes and fullscreen
            emus.Add(new Emulator("RPCS3", "rpcs3.exe", "PS3 Games (*.bin)|*.bin"));
            emus.Last().DescriptionChange("Activate RPCS3 close when process finish and fullscreen");
            //Works as expected
            emus.Add(new Emulator("PCSX2", "pcsx2.exe", "PS2 Games (*.iso)|*.iso", "","", "--fullscreen --nogui"));
        }
    }

    class Emulator
    {
        readonly string name;
        readonly string exe;
        readonly string argumentsP1;
        readonly string argumentsPmid;
        readonly string argumentsP2;
        readonly string gamesfilters;
        string description = "Works as expected";
        string InstallPath = null;
        Color cdesc = Color.LightGreen;

        public string Name { get => name;}
        public string Exe { get => exe;}
        public string Gamesfilters { get => gamesfilters; }
        public string Description { get => description; }
        public Color Cdesc { get => cdesc; }

        public Emulator(string Name, string Exe, string Filters, string ArgumentsP1, string ArgumentsPmid, string ArgumentsP2)
        {
            name = Name;
            exe = Exe;
            argumentsP1 = ArgumentsP1 + " \\\"";
            argumentsPmid = ArgumentsPmid;
            argumentsP2 = "\\\" " + ArgumentsP2;
            gamesfilters = Filters;
        }

        public Emulator(string Name, string Exe, string Filters)
        {
            name = Name;
            exe = Exe;
            argumentsP1 = "\\\"";
            argumentsP2 = "\\\"";
            gamesfilters = Filters;
        }

        public void DescriptionChange(string desc, bool red = false)
        {
            description = desc;
            if (red == true)
                cdesc = Color.OrangeRed;
            else
                cdesc = Color.Yellow;
        }

        public string Path(string path = null)
        {
            if (InstallPath == null)
            {
                if (System.IO.File.Exists(path + exe))
                {
                    InstallPath = path;
                }
                var Message = new MessageForm(path + exe, 1);
                Message.Show();
            }
            return InstallPath;
        }

        public string Arguments(string gamedir)
        {
            return argumentsP1 + argumentsPmid + gamedir + argumentsP2;
        }
    }
}
