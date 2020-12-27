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
            //Params: "Name of emulator", "name of exe.exe", "Parameters before game file path", "Parameters after game file path", "Game files that emulator supports"

            //Works as expected
            emus.Add(new Emulator("CEMU", "Cemu.exe", "-g \\\"", "\\\" -f", "WiiU Games (*.rpx; *.wud; *.wux)|*.rpx;*.wud;*.wux"));
            //Works as expected
            emus.Add(new Emulator("Dolphin", "Dolphin.exe", "-b -e \\\"", "\\\"", "Wii/GC Games (*.iso; *.wbfs; *.ciso; *.gcz; *.gcm)|*.iso;*.wbfs;*.ciso;*.gcz;*.gcm"));
            //Works as expected
            emus.Add(new Emulator("SNES9X", "snes9x.exe", "\\\"..\\\\", "\\\" -fullscreen", "SNES Games (*.smc; *.sfc; *.swc; *.zip)|*.smc;*.sfc;*.swc;*.zip"));
            //Need to activate fullscreen through Emulator GUI
            emus.Add(new Emulator("PJ64", "Project64.exe", "\\\"", "\\\"", 
                "Nintendo 64 Games (*.n64; *.z64; *.v64; *.u64; *.zip; *.rar; *.rom; *.jap; *.pal; *.usa)|*.n64;*.z64;*.v64;*.u64;*.zip;*.rar;*.rom;*.jap;*.pal;*.usa"));
            emus.Last().DescriptionChange("Activate fullscreen through PJ64 GUI");

            //Need to activate fullscreen through Emulator GUI
            emus.Add(new Emulator("YUZU", "yuzu.exe", "\\\"", "\\\"", "Switch Games (*.xci; *.nsp)| *.xci;*.nsp"));
            emus.Last().DescriptionChange("Activate fullscreen through YUZU GUI");
            //Only works on NightBuild
            emus.Add(new Emulator("VBA-M", "visualboyadvance-m.exe", "/f \\\"", "\\\"", "GB-GBC-GBA Games (*.gba; *.gbc; *.gb; *.zip)|*.gba;*.gbc;*.gb;*.zip"));
            emus.Last().DescriptionChange("Only works on nightbuild. Don't exit fullscreen, do ALT+F4", true);
            //Need to activate close when process finishes and fullscreen
            emus.Add(new Emulator("RPCS3", "rpcs3.exe", "\\\"", "\\\"", "PS3 Games (*.bin)|*.bin"));
            emus.Last().DescriptionChange("Activate RPCS3 close when process finish and fullscreen");
            //Works as expected
            emus.Add(new Emulator("PCSX2", "pcsx2.exe", "\\\"", "\\\" --fullscreen --nogui", "PS2 Games (*.iso)|*.iso"));
        }
    }

    class Emulator
    {
        readonly string name;
        readonly string exe;
        readonly string argumentsP1;
        readonly string argumentsP2;
        readonly string gamesfilters;
        string description = "Works as expected";
        Color cdesc = Color.LightGreen;

        public string Name { get => name;}
        public string Exe { get => exe;}
        public string Gamesfilters { get => gamesfilters; }
        public string Description { get => description; }
        public Color Cdesc { get => cdesc; }

        public Emulator(string Name, string Exe, string ArgumentsP1, string ArgumentsP2, string Filters)
        {
            name = Name;
            exe = Exe;
            argumentsP1 = ArgumentsP1;
            argumentsP2 = ArgumentsP2;
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

        public string Arguments(string gamedir)
        {
            return argumentsP1 + gamedir + argumentsP2;
        }
    }
}
