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
    /// Vanguard Logic
    /// </summary>
    public class Vanguard
    {
        private static Dictionary<int, GfxMapTRZoneData> Zones { get; } = new Dictionary<int, GfxMapTRZoneData>();
        public class GfxMapTRZoneData : IDisposable
        {
            public string Name { get; set; }
            /// <summary>
            /// Gets or Sets the Position Buffer Reader
            /// </summary>
            public BinaryReader VertexBufferReader { get; set; }


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
            public GfxMapTRZoneData(string name, byte[] posBuffer, byte[] faceIndicesBuffer)
            {
                Name = name;
                VertexBufferReader = new BinaryReader(new MemoryStream(posBuffer));
                FaceIndicesBufferReader = new BinaryReader(new MemoryStream(faceIndicesBuffer));
            }

            /// <summary>
            /// Disposes of the TR Zone
            /// </summary>
            public void Dispose()
            {
                VertexBufferReader?.Dispose();
                FaceIndicesBufferReader?.Dispose();
            }
        }

        /// <summary>
        /// Reads BSP Data
        /// </summary>
        public static void ExportBSPData(ProcessReader reader, long assetPoolsAddress, long assetSizesAddress, string gameType, Action<object> printCallback = null)
        {

            // Found her
            printCallback?.Invoke("Found supported game: Call of Duty: Vanguard (Parasyte)");

            // Get GfxMap & TRZone pools
            var gfxMapPool = reader.ReadStruct<ParasyteXAssetPool64>(assetPoolsAddress + Marshal.SizeOf<ParasyteXAssetPool64>() * 33).FirstXAsset;
            var trZonePool = reader.ReadStruct<ParasyteXAssetPool64>(assetPoolsAddress + Marshal.SizeOf<ParasyteXAssetPool64>() * 34).FirstXAsset;

            VGGfxMap gfxMapAsset = reader.ReadStruct<VGGfxMap>(gfxMapPool);
            // Iterate over loaded Gfx Map assets and grab the last one
            for (var current = reader.ReadStruct<ParasyteXAsset64>(gfxMapPool); ; current = reader.ReadStruct<ParasyteXAsset64>(current.Next))
            {
                // Get GfxMap name
                string assetName = reader.ReadNullTerminatedString(current.Name);
                // Check if it's valid and is the last one
                if (!String.IsNullOrEmpty(assetName) && current.Next == 0)
                {
                    // Assign it
                    gfxMapAsset = reader.ReadStruct<VGGfxMap>(current.Header);
                    break;
                }
                // Last one
                else if (current.Next == 0)
                    break;
            }

            // Name
            string gfxMapName = reader.ReadNullTerminatedString(gfxMapAsset.NamePointer);
            string mapName = reader.ReadNullTerminatedString(gfxMapAsset.MapNamePointer);
            if (!String.IsNullOrEmpty(gfxMapName))
            {
                // Grab all loaded TR Zones
                for (var current = reader.ReadStruct<ParasyteXAsset64>(trZonePool); ; current = reader.ReadStruct<ParasyteXAsset64>(current.Next))
                {
                    // Read TR Zone
                    var zone = reader.ReadStruct<VGGfxWorldTRZone>(current.Header);
                    // Check if it has vertex data
                    if (zone.VertexBufferPointer > 0 && !Zones.ContainsKey(zone.Index))
                    {
                        Zones[zone.Index] = new GfxMapTRZoneData(
                            reader.ReadNullTerminatedString(zone.NamePointer),
                            reader.ReadBytes(zone.VertexBufferPointer, zone.VertexBufferSize),
                            reader.ReadBytes(zone.IndicesBufferPointer, zone.IndexCount * 2));
                    }
                    // Last one
                    if (current.Next == 0)
                        break;
                }

                // New IW Map
                var mapFile = new IWMap();
                // Print Info
                printCallback?.Invoke("");
                printCallback?.Invoke($"Loaded Map              {mapName}");
                printCallback?.Invoke($"Loaded Surfaces         {gfxMapAsset.surfaceCount}");
                printCallback?.Invoke($"Loaded Surfaces Data    {gfxMapAsset.surfaceDataCount}");
                printCallback?.Invoke($"Loaded TR Zones         {Zones.Count}");
                printCallback?.Invoke($"Loaded Unique Models    {gfxMapAsset.UniqueModelCount}");
                printCallback?.Invoke($"Loaded Static Models    {gfxMapAsset.ModelInstCount}");

                // Build output Folder
                string outputName = Path.Combine("exported_maps", "vanguard", gameType, mapName, mapName);
                Directory.CreateDirectory(Path.GetDirectoryName(outputName));

                // Stop watch
                var stopWatch = Stopwatch.StartNew();

                // Read surface data
                printCallback?.Invoke("Parsing surface data....");
                var surfaces = reader.ReadStructArray<VGGfxMapSurface>(gfxMapAsset.surfacesPtr, gfxMapAsset.surfaceCount);
                var surfaceData = reader.ReadStructArray<VGGfxSurfaceData>(gfxMapAsset.surfaceDataPtr, gfxMapAsset.surfaceDataCount);
                var surfaceMaterials = reader.ReadStructArray<long>(gfxMapAsset.materialsPtr, gfxMapAsset.materialCount);
                printCallback?.Invoke(String.Format("Parsed surface data in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));

                // Reset timer
                stopWatch.Restart();

                // Write OBJ
                printCallback?.Invoke("Converting to OBJ....");

                // Create new OBJ
                var obj = new WavefrontOBJ();

                // Image Names (for Search String)
                HashSet<string> imageNames = new HashSet<string>();

                // Base vertex offset, update every surface
                int vertexOffset = 0;
                List<int> missingZones = new List<int>();
                foreach (var surf in surfaces)
                {
                    var dataInfo = surfaceData[surf.DataIndex];
                    if (Zones.TryGetValue(dataInfo.ZoneIndex, out var TRZone))
                    {
                        // Read material
                        var material = ReadMaterial(reader, surfaceMaterials[surf.MaterialIndex]);
                        // Merge object with same base material
                        if (dataInfo.LayerCount > 1)
                            material.Name = material.Name.Split('_')[0];
                        // Add to images
                        imageNames.Add(material.DiffuseMap);
                        // Add it
                        obj.AddMaterial(material);

                        // Load Vertex Positions
                        TRZone.VertexBufferReader.BaseStream.Position = dataInfo.VertexOffsets[1];
                        for (int i = 0; i < surf.VertexCount; i++)
                            obj.Vertices.Add(Vertex.UnpackVGVertex(TRZone.VertexBufferReader.ReadUInt64(), dataInfo.Scale, dataInfo.Offsets));

                        // Load Vertex Normals
                        TRZone.VertexBufferReader.BaseStream.Position = dataInfo.VertexOffsets[2];
                        for (int i = 0; i < surf.VertexCount; i++)
                        {
                            var PackedTangentFrame = TRZone.VertexBufferReader.ReadUInt32();
                            var (tangent, bitangent, normal) = VertexNormalUnpacking.UnpackTangentFrame(PackedTangentFrame);
                            obj.Normals.Add(normal);
                        }

                        // Load vertex UVs
                        TRZone.VertexBufferReader.BaseStream.Position = dataInfo.VertexOffsets[5];
                        for (int i = 0; i < surf.VertexCount; i++)
                        {
                            var UV = TRZone.VertexBufferReader.ReadStruct<GfxVertexUV>();
                            obj.UVs.Add(new Vector2(UV.U, 1 - UV.V));
                            // Skip layer UVs
                            int LayerUVPadding = (dataInfo.LayerCount - 1) * 8;
                            TRZone.VertexBufferReader.BaseStream.Position += LayerUVPadding;
                        }


                        // Load faces
                        TRZone.FaceIndicesBufferReader.BaseStream.Position = surf.FaceIndex * 2;
                        for (int i = 0; i < surf.FaceCount; i++)
                        {
                            // Read face indices (indices are relative to each surface, so we add vertexOffset to it)
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

                        // Update vertex offset
                        vertexOffset += surf.VertexCount;
                    }

                    else
                    {
                        if (!missingZones.Contains(dataInfo.ZoneIndex))
                        {
                            printCallback?.Invoke($"Zone {dataInfo.ZoneIndex} not loaded");
                            missingZones.Add(dataInfo.ZoneIndex);
                        }
                    }

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
                var mapEntities = ModernWarfare4.ReadStaticModels(reader,
                                                                  gfxMapAsset.ModelInstPtr,
                                                                  gfxMapAsset.ModelInstCount,
                                                                  gfxMapAsset.UniqueModelsPtr,
                                                                  gfxMapAsset.UniqueModelCount,
                                                                  gfxMapAsset.ModelInstDataPtr,
                                                                  gfxMapAsset.ModelInstDataCount);

                // Add them to IWMap
                mapFile.Entities.AddRange(mapEntities);
                mapFile.DumpToMap(outputName + ".map");

                printCallback?.Invoke(String.Format("Parsed models in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));

                printCallback?.Invoke("Done.");
            }
            else
            {
                printCallback?.Invoke("No map was loaded.");
            }
        }
        /// <summary>
        /// Reads a material for the given surface and its associated images
        /// </summary>
        public static WavefrontOBJ.Material ReadMaterial(ProcessReader reader, long address)
        {
            // Read Material
            var material = reader.ReadStruct<VGMaterial>(address);
            // Name
            string materialName = reader.ReadNullTerminatedString(material.NamePointer);
            if (materialName.Contains("248n_448n"))
                Console.WriteLine();
            // Create new OBJ Image
            var objMaterial = new WavefrontOBJ.Material(Path.GetFileNameWithoutExtension(materialName).Replace("*", ""));
            // Loop over images
            for (byte i = 0; i < material.ImageCount; i++)
            {
                // Material Image
                var materialImageTable = reader.ReadStruct<MaterialImage64A>(material.ImageTablePointer + i * Marshal.SizeOf<MaterialImage64A>());
                // Image Name
                var imageName = reader.ReadNullTerminatedString(reader.ReadInt64(materialImageTable.ImagePointer));
                // Check for color map for now
                if (materialImageTable.SemanticHash == 0)
                    objMaterial.DiffuseMap = "_images\\\\" + imageName + ".png";
            }
            // Done
            return objMaterial;
        }

    }
}
