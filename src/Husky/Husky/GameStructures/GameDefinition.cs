using PhilLibX.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Husky
{
    public class GameDefinition
    {
        public long AssetDBAddress { get; set; }
        public long AssetSizesAddress { get; set; }
        public string GameType { get; set; }
        public string GamePath { get; set; }
        public Action<ProcessReader, long, long, string, Action<object>> ExportMethod { get; set; }


        public GameDefinition(
            long assetDBAddress,
            long assetSizesAddress,
            string gameType,
            Action<ProcessReader, long, long, string, Action<object>> exportMethod)
        {
            AssetDBAddress = assetDBAddress;
            AssetSizesAddress = assetSizesAddress;
            GameType = gameType;
            ExportMethod = exportMethod;
        }

        public void ParseParasyteDB(Process proces, Action<object> printCallback)
        {
            string dbPath = Path.Combine(
                Path.GetDirectoryName(proces.MainModule.FileName),
                "Data/CurrentHandler.parasyte_state_info");
            if (!File.Exists(dbPath))
            {
                printCallback?.Invoke("Parasyte Database not found");
                return;
            }

            using (BinaryReader dbReader = new BinaryReader(File.OpenRead(dbPath)))
            {
                // Validate supported game
                var GameID = dbReader.ReadUInt64();
                switch (GameID)
                {
                    // Modern Warfare 2019
                    case 0x3931524157444f4d:
                        ExportMethod = ModernWarfare4.ExportBSPData;
                        break;

                    case 0x44524155474e4156:
                        ExportMethod = Vanguard.ExportBSPData;
                        break;

                    default:
                        printCallback?.Invoke("Parasyte game handler found but is not supported.");
                        return;
                }

                // Parse necessary offsets and paths
                AssetDBAddress = dbReader.ReadInt64();
                // StringTableAddress - we skip it
                dbReader.BaseStream.Position += 8;
                // Game directory
                int pathLength = dbReader.ReadInt32();
                GamePath = Encoding.UTF8.GetString(dbReader.ReadBytes(pathLength));
            }
        }
        public void Export(ProcessReader reader, Action<object> printCallback)
        {
            ExportMethod(reader, AssetDBAddress, AssetSizesAddress, GameType, printCallback);
        }
    }
}
