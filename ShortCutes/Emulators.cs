using System;
using System.Collections.Generic;
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
            emus.Add(new Emulator("CEMU", "Cemu.exe", "\"-g \\\"", "\\\" -f\"", "WiiU Games (*.rpx; *.wud; *.wux)|*.rpx;*.wud;*.wux"));
            emus.Add(new Emulator("Dolphin", "Dolphin.exe", "\"-b -e \\\"", "\\\"\"", "Wii/GC Games (*.iso; *.wbfs; *.ciso; *.gcz; *.gcm)|*.iso;*.wbfs;*.ciso;*.gcz;*.gcm"));
            emus.Add(new Emulator("PJ64", "Project64.exe", "\"\\\"", "\\\"\"", 
                "Nintendo 64 Games (*.n64; *.z64; *.v64; *.u64; *.zip; *.rar; *.rom; *.jap; *.pal; *.usa)|*.n64;*.z64;*.v64;*.u64;*.zip;*.rar;*.rom;*.jap;*.pal;*.usa"));
            emus.Add(new Emulator("SNES9X", "snes9x.exe", "\"\\\"..\\\\", "\\\" -fullscreen\"", "SNES Games (*.smc; *.sfc; *.swc; *.zip)|*.smc;*.sfc;*.swc;*.zip"));
        }
    }

    class Emulator
    {
        string name;
        string exe;
        string argumentsP1;
        string argumentsP2;
        string gamesfilters;

        public string Name { get => name;}
        public string Exe { get => exe;}
        public string Gamesfilters { get => gamesfilters; }

        public Emulator(string Name, string Exe, string ArgumentsP1, string ArgumentsP2, string Filters)
        {
            name = Name;
            exe = Exe;
            argumentsP1 = ArgumentsP1;
            argumentsP2 = ArgumentsP2;
            gamesfilters = Filters;
        }

        public string Arguments(string gamedir)
        {
            return argumentsP1 + gamedir + argumentsP2;
        }
    }
}
