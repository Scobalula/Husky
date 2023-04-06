// ------------------------------------------------------------------------
// Husky - Call of Duty BSP Extractor
// Copyright (C) 2018 Philip/Scobalula
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------
// TODO: Write converters for other formats (like SEModel) for now we're just writing OBJ
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using PhilLibX;
using PhilLibX.IO;

namespace Husky
{

    /// <summary>
    /// Main Program Class
    /// </summary>
    public class HuskyUtil
    {
        /// <summary>
        /// Game Addresses & Methods (Asset DB and Asset Pool Sizes) (Some are relative due to ASLR)
        /// </summary>
        static Dictionary<string, GameDefinition> Games = new Dictionary<string, GameDefinition>()
        {
            // Call of Duty: World At War
            { "CoDWaWmp",           new GameDefinition(0x8D0958,          0x8D06E8,       "mp",               WorldatWar.ExportBSPData) },
            { "CoDWaW",             new GameDefinition(0x8DC828,          0x8DC5D0,       "sp",               WorldatWar.ExportBSPData) },
            // Call of Duty: Modern Warfare
            { "iw3mp",              new GameDefinition(0x7265E0,          0x7263A0,       "mp",               ModernWarfare.ExportBSPData) },
            { "iw3sp",              new GameDefinition(0x7307F8,          0x730510,       "sp",               ModernWarfare.ExportBSPData) },
            // Call of Duty: Modern Warfare 2
            { "iw4mp",              new GameDefinition(0x6F81D0,          0x6F7F08,       "mp",               ModernWarfare2.ExportBSPData) },
            { "iw4sp",              new GameDefinition(0x7307F8,          0x730510,       "sp",               ModernWarfare2.ExportBSPData) },
            // Call of Duty: Modern Warfare 3
            { "iw5mp",              new GameDefinition(0x8AB258,          0x8AAF78,       "mp",               ModernWarfare3.ExportBSPData) },
            { "iw5sp",              new GameDefinition(0x92AD20,          0x92AA40,       "sp",               ModernWarfare3.ExportBSPData) },
            // Call of Duty: Black Ops
            { "BlackOps",           new GameDefinition(0xB741B8,          0xB73EF8,       "sp",               BlackOps.ExportBSPData) },
            { "BlackOpsMP",         new GameDefinition(0xBF2C30,          0xBF2970,       "mp",               BlackOps.ExportBSPData) },
            // Call of Duty: Black Ops 2
            { "t6zm",               new GameDefinition(0xD41240,          0xD40E80,       "zm",               BlackOps2.ExportBSPData) },
            { "t6mp",               new GameDefinition(0xD4B340,          0xD4AF80,       "mp",               BlackOps2.ExportBSPData) },
            { "t6sp",               new GameDefinition(0xBD46B8,          0xBD42F8,       "sp",               BlackOps2.ExportBSPData) },
            // Call of Duty: Black Ops III
            { "BlackOps3",          new GameDefinition(0x94073F0,         0,              "core",             BlackOps3.ExportBSPData) },
            // Call of Duty: Ghosts
            { "iw6mp64_ship",       new GameDefinition(0x1409E4F20,       0x1409E4E20,    "mp",               Ghosts.ExportBSPData) },
            { "iw6sp64_ship",       new GameDefinition(0x14086DCB0,       0x14086DBB0,    "sp",               Ghosts.ExportBSPData) },
            // Call of Duty: Infinite Warfare
            { "iw7_ship",           new GameDefinition(0x1414663D0,       0x141466290,    "core",             InfiniteWarfare.ExportBSPData) },
            // Parasyte (We use MW2019 as base)
            { "Parasyte.CLI",       new GameDefinition(0,                 0,              "core",             ModernWarfare4.ExportBSPData) },
            // Call of Duty: Advanced Warfare
            { "s1_mp64_ship",       new GameDefinition(0x1409B40D0,       0x1409B4B90,    "mp",               AdvancedWarfare.ExportBSPData) },
            { "s1_sp64_ship",       new GameDefinition(0x140804690,       0x140804140,    "sp",               AdvancedWarfare.ExportBSPData) },
            // Call of Duty: World War II
            { "s2_mp64_ship",       new GameDefinition(0xC08470,          0xEB0C30,       "mp",               WorldWarII.ExportBSPData) },
            { "s2_sp64_ship",       new GameDefinition(0x94FD10,          0xBD65E0,       "sp",               WorldWarII.ExportBSPData) },
            // Call of Duty: Modern Warfare Remastered
            { "h1_mp64_ship",       new GameDefinition(0x10B4460,         0x10B3C80,      "mp",               ModernWarfareRM.ExportBSPData) },
            { "h1_sp64_ship",       new GameDefinition(0xEC9FB0,          0xEC97D0,       "sp",               ModernWarfareRM.ExportBSPData) },
        };

        /// <summary>
        /// Looks for matching game and loads BSP from it
        /// </summary>
        public static void LoadGame(Action<object> printCallback = null)
        {
            try
            {
                // Get all processes
                var processes = Process.GetProcesses();

                // Loop through them, find match
                foreach (var process in processes)
                {
                    // Check for it in dictionary
                    if (Games.TryGetValue(process.ProcessName, out var game))
                    {
                        // If process is Parasyte, parse DB first
                        if (process.ProcessName == "Parasyte.CLI")
                            game.ParseParasyteDB(process, printCallback);

                        // Check for valid AssetPool Address
                        if (game.AssetDBAddress != 0)
                            // Export it
                            game.Export(new ProcessReader(process), printCallback);

                        // Done
                        return;
                    }
                }

                // Failed
                printCallback?.Invoke("Failed to find a supported game, please ensure one of them is running.");
            }
            catch(Exception e)
            {
                printCallback?.Invoke("An unhandled exception has occured:");
                printCallback?.Invoke(e);
            }
        }
    }
}
