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
using PhilLibX;
using PhilLibX.IO;
using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Husky
{
    /// <summary>
    /// Modern Warfare 2019 Logic
    /// </summary>
    public class ModernWarfare4
    {
        private static Dictionary<int, GfxMapTRZoneData> Zones { get; } = new Dictionary<int, GfxMapTRZoneData>();
        public class GfxMapTRZoneData : IDisposable
        {
            public string Name { get; set; }
            /// <summary>
            /// Gets or Sets the Position Buffer Reader
            /// </summary>
            public BinaryReader PositionBufferReader { get; set; }

            /// <summary>
            /// Gets or Sets the Draw Data Buffer Reader
            /// </summary>
            public BinaryReader DrawDataBufferReader { get; set; }

            /// <summary>
            /// Gets or Sets the Face Indices Buffer Reader
            /// </summary>
            public BinaryReader FaceIndicesBufferReader { get; set; }

            /// <summary>
            /// Creates a new TR Zone Info Object
            /// </summary>
            public GfxMapTRZoneData() { }

            /// <summary>
            /// Creates a new TR Zone Info Object
            /// </summary>
            public GfxMapTRZoneData(string name, byte[] posBuffer, byte[] drawDataBuffer, byte[] faceIndicesBuffer)
            {
                Name = name;
                PositionBufferReader = new BinaryReader(new MemoryStream(posBuffer));
                DrawDataBufferReader = new BinaryReader(new MemoryStream(drawDataBuffer));
                FaceIndicesBufferReader = new BinaryReader(new MemoryStream(faceIndicesBuffer));
            }

            /// <summary>
            /// Disposes of the TR Zone
            /// </summary>
            public void Dispose()
            {
                PositionBufferReader?.Dispose();
                DrawDataBufferReader?.Dispose();
                FaceIndicesBufferReader?.Dispose();
            }
        }

        private static long GetAssetPool(ProcessReader reader, Action<object> printCallback = null)
        {
            string dbPath = Path.Combine(
                Path.GetDirectoryName(reader.ActiveProcess.MainModule.FileName),
                "Data/CurrentHandler.parasyte_state_info");
            if (!File.Exists(dbPath))
            {
                printCallback?.Invoke("Parasyte Database not found");
                return 0;
            }

            using (BinaryReader dbReader = new BinaryReader(File.OpenRead(dbPath)))
            {
                var ID = dbReader.ReadUInt64();
                if (ID != 0x3931524157444f4d)
                {
                    printCallback?.Invoke("Parasyte has been found, but Modern Warfare 2019 Handler is not loaded.");
                    return 0;
                }
                return dbReader.ReadInt64();
            }
        }
        /// <summary>
        /// Reads BSP Data
        /// </summary>
        public static void ExportBSPData(ProcessReader reader, long assetPoolsAddress, long assetSizesAddress, string gameType, Action<object> printCallback = null)
        {

            // Get Asset Pool using Parasyte's DB file
            assetPoolsAddress = GetAssetPool(reader, printCallback);

            if (assetPoolsAddress == 0)
                return;

            // Found her
            printCallback?.Invoke("Found supported game: Call of Duty: Modern Warfare 2019 (Parasyte)");

            // Get GfxMap & TRZone pools
            var gfxMapPool = reader.ReadStruct<ParasyteXAssetPool64>(assetPoolsAddress + Marshal.SizeOf<ParasyteXAssetPool64>() * 31).FirstXAsset;
            var trZonePool = reader.ReadStruct<ParasyteXAssetPool64>(assetPoolsAddress + Marshal.SizeOf<ParasyteXAssetPool64>() * 32).FirstXAsset;

            MW4GfxMap gfxMapAsset = reader.ReadStruct<MW4GfxMap>(gfxMapPool);
            // Iterate over loaded Gfx Map assets and grab the last one
            for (var current = reader.ReadStruct<ParasyteXAsset64>(gfxMapPool); ; current = reader.ReadStruct<ParasyteXAsset64>(current.Next))
            {
                // Get GfxMap name
                string assetName = reader.ReadNullTerminatedString(reader.ReadInt64(current.Header));
                // Check if it's valid and is the last one
                if (!String.IsNullOrEmpty(assetName) && current.Next == 0)
                {
                    // Assign it
                    gfxMapAsset = reader.ReadStruct<MW4GfxMap>(current.Header);
                    break;
                }
                // Last one
                else if (current.Next == 0)
                    break;
            }

            // Name
            string gfxMapName = reader.ReadNullTerminatedString(gfxMapAsset.NamePointer);
            string mapName = reader.ReadNullTerminatedString(gfxMapAsset.MapNamePointer);

            // Grab all loaded TR Zones
            for (var current = reader.ReadStruct<ParasyteXAsset64>(trZonePool); ; current = reader.ReadStruct<ParasyteXAsset64>(current.Next))
            {
                // Read TR Zone
                var zone = reader.ReadStruct<MW4GfxWorldTRZone>(current.Header);
                // Check if it has vertex data
                if (zone.PositionsBufferPointer != 0)
                {
                    // Add it
                    Zones[zone.Index] = new GfxMapTRZoneData(
                        reader.ReadNullTerminatedString(zone.NamePointer),
                        reader.ReadBytes(zone.PositionsBufferPointer, zone.PositionsBufferSize),
                        reader.ReadBytes(zone.DrawDataBufferPointer, zone.DrawDataBufferSize),
                        reader.ReadBytes(zone.FaceIndicesBufferPointer, zone.FaceIndicesBufferSize * 2));
                }
                // Last one
                if (current.Next == 0)
                    break;
            }

            // New IW Map
            var mapFile = new IWMap();
            // Print Info
            printCallback?.Invoke("");
            printCallback?.Invoke($"Loaded GfxMap           {gfxMapName}");
            printCallback?.Invoke($"Loaded Map              {mapName}");
            printCallback?.Invoke($"Loaded Surfaces         {gfxMapAsset.SurfaceCount}");
            printCallback?.Invoke($"Loaded Surfaces Data    {gfxMapAsset.SurfaceDataCount}");
            printCallback?.Invoke($"Loaded TR Zones         {gfxMapAsset.TrZoneCount}");
            printCallback?.Invoke($"Loaded Unique Models    {gfxMapAsset.UniqueModelCount}");
            printCallback?.Invoke($"Loaded Static Models    {gfxMapAsset.StaticModelCount}");

            // Build output Folder
            string outputName = Path.Combine("exported_maps", "modern_warfare_4", gameType, mapName, mapName);
            Directory.CreateDirectory(Path.GetDirectoryName(outputName));

            // Stop watch
            var stopWatch = Stopwatch.StartNew();

            // Read Vertices
            printCallback?.Invoke("Parsing surface data....");
            var data = reader.ReadStructArray<MW4GfxTrZoneBufferInfo>(gfxMapAsset.SurfaceDataPointer, gfxMapAsset.SurfaceDataCount);
            printCallback?.Invoke(String.Format("Parsed vertex data in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));

            // Reset timer
            stopWatch.Restart();

            // Read Indices
            printCallback?.Invoke("Parsing surfaces....");
            var surfaces = ReadGfxSufaces(reader, gfxMapAsset.SurfacesPointer, gfxMapAsset.SurfaceCount);
            printCallback?.Invoke(String.Format("Parsed surfaces in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));

            // Reset timer
            stopWatch.Restart();

            // Write OBJ
            printCallback?.Invoke("Converting to OBJ....");

            // Create new OBJ
            var obj = new WavefrontOBJ();

            // Image Names (for Search String)
            HashSet<string> imageNames = new HashSet<string>();

            int vertexOffset = 0;
            var index = 0;
            foreach (var surf in surfaces)
            {
                var dataInfo = data[surf.DataIndex];
                var TRZone = Zones[dataInfo.TransientZone];
                var material = ReadMaterial(reader, surf.MaterialPointer);
                // Add to images
                imageNames.Add(material.DiffuseMap);
                // Add it
                obj.AddMaterial(material);

                // Load Vertex Positions
                TRZone.PositionBufferReader.BaseStream.Position = dataInfo.PositionsPosition;
                for (int i = 0; i < surf.VertexCount; i++)
                    obj.Vertices.Add(TRZone.PositionBufferReader.ReadStruct<GfxVertexPosition>().ToCentimeter());

                // Load Vertex Normals
                TRZone.DrawDataBufferReader.BaseStream.Position = dataInfo.NormalQuatPosition;
                for (int i = 0; i < surf.VertexCount; i++)
                {
                    var PackedTangentFrame = TRZone.DrawDataBufferReader.ReadUInt32();
                    var (tangent, bitangent, normal) = VertexNormalUnpacking.UnpackTangentFrame(PackedTangentFrame);
                    obj.Normals.Add(normal);
                }

                // Load vertex UVs
                TRZone.DrawDataBufferReader.BaseStream.Position = dataInfo.UVsPosition;
                for (int i = 0; i < surf.VertexCount; i++)
                {
                    var UV = TRZone.DrawDataBufferReader.ReadStruct<GfxVertexUV>();
                    obj.UVs.Add(new Vector2(UV.U, 1 - UV.V));
                    // Skip layer UVs
                    int LayerUVPadding = (dataInfo.LayerCount - 1) * 8;
                    TRZone.DrawDataBufferReader.BaseStream.Position += LayerUVPadding;
                }


                TRZone.FaceIndicesBufferReader.BaseStream.Position = surf.FaceIndex * 2;

                for (int i = 0; i < surf.FaceCount; i++)
                {
                    var faceIndex1 = TRZone.FaceIndicesBufferReader.ReadUInt16() + vertexOffset;
                    var faceIndex2 = TRZone.FaceIndicesBufferReader.ReadUInt16() + vertexOffset;
                    var faceIndex3 = TRZone.FaceIndicesBufferReader.ReadUInt16() + vertexOffset;

                    // new Obj Face
                    var objFace = new WavefrontOBJ.Face(material.Name);

                    // Add points
                    objFace.Vertices[0] = new WavefrontOBJ.Face.Vertex(faceIndex1, faceIndex1, faceIndex1);
                    objFace.Vertices[2] = new WavefrontOBJ.Face.Vertex(faceIndex2, faceIndex2, faceIndex2);
                    objFace.Vertices[1] = new WavefrontOBJ.Face.Vertex(faceIndex3, faceIndex3, faceIndex3);

                    // Add to OBJ
                    obj.Faces.Add(objFace);
                }
                vertexOffset += surf.VertexCount;
                index++;
            }


            // Save it
            obj.Save(outputName + ".obj");

            // Build search string
            string searchString = "";

            // Loop through images, and append each to the search string (for Wraith/Greyhound)
            foreach (string imageName in imageNames)
                searchString += String.Format("{0},", Path.GetFileNameWithoutExtension(imageName));

            // Dump it
            File.WriteAllText(outputName + "_search_string.txt", searchString);

            // Done
            printCallback?.Invoke(String.Format("Converted to OBJ in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));

            // Reset timer
            stopWatch.Restart();
            
            printCallback?.Invoke("Parsing static models....");
            // Read entities
            var mapEntities = ReadStaticModels(
                reader,
                gfxMapAsset.StaticModelPtr,
                gfxMapAsset.StaticModelCount,
                gfxMapAsset.UniqueModelsPtr,
                gfxMapAsset.UniqueModelCount,
                gfxMapAsset.ModelCollectionsPtr,
                gfxMapAsset.CollectionCount);

            // Add them to IWMap
            mapFile.Entities.AddRange(mapEntities);
            mapFile.DumpToMap(outputName + ".map");

            printCallback?.Invoke(String.Format("Parsed models in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));

            printCallback?.Invoke("Done.");

        }

        /// <summary>
        /// Reads Gfx Surfaces
        /// </summary>
        public static MW4GfxMapSurface[] ReadGfxSufaces(ProcessReader reader, long address, int count)
        {
            // Preallocate array
            MW4GfxMapSurface[] surfaces = new MW4GfxMapSurface[count];

            // Loop number of indices we have
            for (int i = 0; i < count; i++)
                // Add it
                surfaces[i] = reader.ReadStruct<MW4GfxMapSurface>(address + i * Marshal.SizeOf<MW4GfxMapSurface>());

            // Done
            return surfaces;
        }

        /// <summary>
        /// Reads a material for the given surface and its associated images
        /// </summary>
        public static WavefrontOBJ.Material ReadMaterial(ProcessReader reader, long address)
        {
            // Read Material
            var material = reader.ReadStruct<MW4Material>(address);
            // Name
            string materialName = reader.ReadNullTerminatedString(material.NamePointer);
            // Create new OBJ Image
            var objMaterial = new WavefrontOBJ.Material(Path.GetFileNameWithoutExtension(materialName).Replace("*", ""));
            // Loop over images
            for (byte i = 0; i < material.ImageCount; i++)
            {
                // Read Material Image
                var materialImageTable = reader.ReadStruct<MaterialImage64A>(material.ImageTablePointer + i * Marshal.SizeOf<MaterialImage64A>());
                var materialImage = reader.ReadStruct<MW4GfxImage>(materialImageTable.ImagePointer);
                // Image Name
                var imageName = reader.ReadNullTerminatedString(materialImage.NamePtr);
                // Check for color map for now
                if (materialImageTable.SemanticHash == 0)
                    objMaterial.DiffuseMap = "_images\\\\" + imageName + ".png";
            }
            // Done
            return objMaterial;
        }

        /// <summary>
        /// Parses Static Models into IWMap.Entity List
        /// </summary>
        /// <param name="reader">Process Memory Reader</param>
        /// <param name="instancesAddress">Model Instances Address</param>
        /// <param name="instancesCount">Model Instances Count</param>
        /// <param name="modelsAddress">Unique Models Address</param>
        /// <param name="modelCount">Unique Models Count</param>
        /// <param name="modelIndicesAddress">Model Indices Address</param>
        /// <param name="modelIndicesCount">Model Indices Count</param>
        /// <returns>List of IWMap Entities</returns>
        public unsafe static List<IWMap.Entity> ReadStaticModels(
            ProcessReader reader,
            long instancesAddress,
            int instancesCount,
            long modelsAddress,
            int modelCount,
            long modelIndicesAddress,
            int modelIndicesCount)
        {
            // Resulting Entities
            List<IWMap.Entity> entities = new List<IWMap.Entity>(instancesCount);
            // Read instances buffer
            var byteBuffer = reader.ReadBytes(instancesAddress, instancesCount * Marshal.SizeOf<MW4GfxStaticModelPlacement>());
            // Loop number of models we have
            for (int i = 0; i < instancesCount; i++)
            {
                // Read Struct
                var staticModel = ByteUtil.BytesToStruct<MW4GfxStaticModelPlacement>(byteBuffer, i * Marshal.SizeOf<MW4GfxStaticModelPlacement>());
                // Placeholder Name 
                var modelName = $"model_{i}";
                // Convert PackedQuat to Euler
                var euler = new Vector4(
                    (staticModel.PackedQuat[2] * 0.000015259022f * 2.0f) - 1.0f,
                    (staticModel.PackedQuat[0] * 0.000015259022f * 2.0f) - 1.0f,
                    (staticModel.PackedQuat[1] * 0.000015259022f * 2.0f) - 1.0f,
                    (staticModel.PackedQuat[3] * 0.000015259022f * 2.0f) - 1.0f).ToEuler();

                // Add it
                entities.Add(IWMap.Entity.CreateMiscModel(
                                modelName,
                                new Vector3(staticModel.PackedPosition[0] * 0.000244140625f,
                                staticModel.PackedPosition[1] * 0.000244140625f,
                                staticModel.PackedPosition[2] * 0.000244140625f),
                                Rotation.ToDegrees(euler),
                                staticModel.Scale));
            }
            // Array to store unique model names
            string[] modelNames = new string[modelCount];
            // Read unique models buffer
            byteBuffer = reader.ReadBytes(modelsAddress, modelCount * Marshal.SizeOf<MW4GfxStaticModel>());
            for (int i = 0; i < modelCount; i++)
            {
                var xmodel = ByteUtil.BytesToStruct<MW4GfxStaticModel>(byteBuffer, i * Marshal.SizeOf<MW4GfxStaticModel>());
                modelNames[i] = Path.GetFileNameWithoutExtension(reader.ReadNullTerminatedString(reader.ReadInt64(xmodel.XModelPointer)));
            }

            // Assign the correct name to each IWMap entity
            foreach (var modelData in reader.ReadStructArray<MW4GfxStaticModelIndices>(modelIndicesAddress, modelIndicesCount))
            {
                string modelName = modelNames[modelData.uniqueModelIndex];
                for (int i = modelData.firstInstance; i < modelData.firstInstance + modelData.instanceCount; i++)
                {
                    var modelInst = entities[i];
                    modelInst.KeyValuePairs["model"] = modelName;
                }
            }
            // Done
            return entities;
        }
    }
}
