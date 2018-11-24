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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Husky
{
    /// <summary>
    /// WW2 Logic
    /// </summary>
    public class WorldWarII
    {
        /// <summary>
        /// WW2 GfxMap TRZone Asset (some pointers we skip over point to DirectX routines, etc. if that means anything to anyone)
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct GfxMapTRZone
        {
            /// <summary>
            /// Unknown Bytes
            /// </summary>
            public fixed byte Padding[0x98];

            /// <summary>
            /// A pointer to the name of the map 
            /// </summary>
            public long MapNamePointer { get; set; }

            /// <summary>
            /// Unknown Bytes
            /// </summary>
            public int Padding1 { get; set; }

            /// <summary>
            /// Number of Gfx Vertices (XYZ, etc.)
            /// </summary>
            public int GfxVertexCount { get; set; }

            /// <summary>
            /// Pointer to the Gfx Vertex Positions Data
            /// </summary>
            public long GfxVertexPositionsPointer { get; set; }

            /// <summary>
            /// Pointer to the Gfx Vertex Positions Data
            /// </summary>
            public long GfxVertexUnknownPointer { get; set; }

            /// <summary>
            /// Pointer to the Gfx Vertex Colors Data
            /// </summary>
            public long GfxVertexColorsPointer { get; set; }

            /// <summary>
            /// Pointer to the Gfx Vertex UVs Data
            /// </summary>
            public long GfxVertexUVsPointer { get; set; }

            /// <summary>
            /// Unknown Pointer
            /// </summary>
            public long GfxVertexUnknown2Pointer { get; set; }

            /// <summary>
            /// Pointer to the Gfx Vertex Normals Data
            /// </summary>
            public long GfxVertexNormalsPointer { get; set; }

            /// <summary>
            /// Unknown Pointer (Probably tangents)
            /// </summary>
            public long GfxVertexUnknown3Pointer { get; set; }

            /// <summary>
            /// Unknown Bytes
            /// </summary>
            public fixed byte Padding2[0x50];
        }

        /// <summary>
        /// IW GfxMap Asset (some pointers we skip over point to DirectX routines, etc. if that means anything to anyone)
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct GfxMap
        {
            /// <summary>
            /// A pointer to the name of this GfxMap Asset
            /// </summary>
            public long NamePointer { get; set; }

            /// <summary>
            /// A pointer to the name of the map 
            /// </summary>
            public long MapNamePointer { get; set; }

            /// <summary>
            /// Unknown Bytes (Possibly counts for other data we don't care about)
            /// </summary>
            public fixed byte Padding[0xC];

            /// <summary>
            /// Number of Surfaces
            /// </summary>
            public int SurfaceCount { get; set; }

            /// <summary>
            /// Unknown Bytes (Possibly counts, pointers, etc. for other data we don't care about)
            /// </summary>
            public fixed byte Padding1[0x3A8];

            /// <summary>
            /// Number of Gfx Indices (for Faces)
            /// </summary>
            public long GfxIndicesCount { get; set; }

            /// <summary>
            /// Pointer to the Gfx Index Data
            /// </summary>
            public long GfxIndicesPointer { get; set; }

            /// <summary>
            /// Points, etc.
            /// </summary>
            public fixed byte Padding2[0x750];

            /// <summary>
            /// Number of Static Models
            /// </summary>
            public long GfxStaticModelsCount { get; set; }

            /// <summary>
            /// Unknown Bytes (more BSP data we probably don't care for)
            /// </summary>
            public fixed byte Padding3[0x468];

            /// <summary>
            /// Pointer to the Gfx Surfaces
            /// </summary>
            public long GfxSurfacesPointer { get; set; }

            /// <summary>
            /// Unknown Bytes (more BSP data we probably don't care for)
            /// </summary>
            public fixed byte Padding4[0x18];

            /// <summary>
            /// Pointer to the Gfx Static Models
            /// </summary>
            public long GfxStaticModelsPointer { get; set; }
        }

        /// <summary>
        /// Gfx Map Surface
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct GfxSurface
        {
            /// <summary>
            /// Unknown Int (I know which pointer in the GfxMap it correlates it, but doesn't seem to interest us)
            /// </summary>
            public int UnknownBaseIndex { get; set; }

            /// <summary>
            /// Base Vertex Index (this is what allows the GfxMap to have 65k+ verts with only 2 byte indices)
            /// </summary>
            public int VertexIndex { get; set; }

            /// <summary>
            /// Number of Vertices this surface has
            /// </summary>
            public ushort VertexCount { get; set; }

            /// <summary>
            /// Number of Faces this surface has
            /// </summary>
            public ushort FaceCount { get; set; }

            /// <summary>
            /// Unknown Bytes
            /// </summary>
            public fixed byte Padding[0xC];

            /// <summary>
            /// Base Face Index (this is what allows the GfxMap to have 65k+ faces with only 2 byte indices)
            /// </summary>
            public int FaceIndex { get; set; }

            /// <summary>
            /// Unknown Bytes
            /// </summary>
            public fixed byte Padding2[4];

            /// <summary>
            /// Pointer to the Material Asset of this Surface
            /// </summary>
            public long MaterialPointer { get; set; }

            /// <summary>
            /// Unknown Bytes 
            /// </summary>
            public fixed byte Padding3[0x10];
        }

        /// <summary>
        /// Material Asset Info
        /// </summary>
        public unsafe struct Material
        {
            /// <summary>
            /// A pointer to the name of this material
            /// </summary>
            public long NamePointer { get; set; }

            /// <summary>
            /// Unknown Bytes (Flags, settings, etc.)
            /// </summary>
            public fixed byte UnknownBytes[0x9A];

            /// <summary>
            /// Number of Images this Material has
            /// </summary>
            public byte ImageCount { get; set; }

            /// <summary>
            /// Unknown Bytes (Flags, settings, etc.)
            /// </summary>
            public fixed byte UnknownBytes1[0x15];

            /// <summary>
            /// A pointer to the Tech Set this Material uses
            /// </summary>
            public long TechniqueSetPointer { get; set; }

            /// <summary>
            /// A pointer to this Material's Image table
            /// </summary>
            public long ImageTablePointer { get; set; }

            /// <summary>
            /// UnknownPointer (Probably settings that changed based off TechSet)
            /// </summary>
            public long UnknownPointer { get; set; }

            /// <summary>
            /// Null Bytes
            /// </summary>
            public long Padding { get; set; }

            /// <summary>
            /// Unknown Bytes (Flags, settings, etc.)
            /// </summary>
            public fixed byte UnknownBytes2[0x90];
        }

        /// <summary>
        /// Vertex Position
        /// </summary>
        public struct GfxVertexPosition
        {
            /// <summary>
            /// X Origin
            /// </summary>
            public float X { get; set; }

            /// <summary>
            /// Y Origin
            /// </summary>
            public float Y { get; set; }

            /// <summary>
            /// Z Origin
            /// </summary>
            public float Z { get; set; }
        }

        /// <summary>
        /// Vertex Position
        /// </summary>
        public struct GfxVertexUV
        {
            /// <summary>
            /// X Origin
            /// </summary>
            public float U { get; set; }

            /// <summary>
            /// Y Origin
            /// </summary>
            public float V { get; set; }
        }

        /// <summary>
        /// Gfx Static Model
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct GfxStaticModel
        {
            /// <summary>
            /// X Origin
            /// </summary>
            public float X { get; set; }

            /// <summary>
            /// Y Origin
            /// </summary>
            public float Y { get; set; }

            /// <summary>
            /// Z Origin
            /// </summary>
            public float Z { get; set; }

            /// <summary>
            /// 3x3 Rotation Matrix
            /// </summary>
            public fixed float Matrix[9];

            /// <summary>
            /// Model Scale 
            /// </summary>
            public float ModelScale { get; set; }

            /// <summary>
            /// Unknown Bytes
            /// </summary>
            public fixed byte UnknownBytes2[0x14];

            /// <summary>
            /// Pointer to the XModel Asset
            /// </summary>
            public long ModelPointer { get; set; }

            /// <summary>
            /// Unknown Bytes
            /// </summary>
            public fixed byte UnknownBytes3[0x30];
        }

        /// <summary>
        /// Reads BSP Data
        /// </summary>
        public static void ExportBSPData(ProcessReader reader, long assetPoolsAddress, long assetSizesAddress, string gameType, Action<object> printCallback = null)
        {
            // Found her
            printCallback?.Invoke("Found supported game: Call of Duty: World War II");

            // Validate by XModel Name
            if (reader.ReadNullTerminatedString(reader.ReadInt64(reader.ReadInt64(reader.GetBaseAddress() + assetPoolsAddress + 0x50) + 8)) == "empty_model")
            {
                // Load BSP Pools (they only have a size of 1 so we don't care about reading more than 1)
                var gfxMapAsset = reader.ReadStruct<GfxMap>(reader.ReadInt64(reader.GetBaseAddress() + assetPoolsAddress + 0x138));
                var gfxMapTrZoneAsset = reader.ReadStruct<GfxMapTRZone>(reader.ReadInt64(reader.GetBaseAddress() + assetPoolsAddress + 0x140) + 8);

                // Name
                string gfxMapName = reader.ReadNullTerminatedString(gfxMapAsset.NamePointer);
                string mapName = reader.ReadNullTerminatedString(gfxMapAsset.MapNamePointer);

                // Verify a BSP is actually loaded (if in base menu, etc, no map is loaded)
                if (String.IsNullOrWhiteSpace(gfxMapName))
                {
                    printCallback?.Invoke("No BSP loaded. Enter Main Menu or a Map to load in the required assets.");
                }
                else
                {
                    // New IW Map
                    var mapFile = new IWMap();
                    // Print Info
                    printCallback?.Invoke(String.Format("Loaded Gfx Map     -   {0}", gfxMapName));
                    printCallback?.Invoke(String.Format("Loaded Map         -   {0}", mapName));
                    printCallback?.Invoke(String.Format("Vertex Count       -   {0}", gfxMapTrZoneAsset.GfxVertexCount));
                    printCallback?.Invoke(String.Format("Indices Count      -   {0}", gfxMapAsset.GfxIndicesCount));
                    printCallback?.Invoke(String.Format("Surface Count      -   {0}", gfxMapAsset.SurfaceCount));
                    printCallback?.Invoke(String.Format("Model Count        -   {0}", gfxMapAsset.GfxStaticModelsCount));

                    // Build output Folder
                    string outputName = Path.Combine("exported_maps", "world_war_2", gameType, mapName, mapName);
                    Directory.CreateDirectory(Path.GetDirectoryName(outputName));

                    // Stop watch
                    var stopWatch = Stopwatch.StartNew();

                    // Read Vertices
                    printCallback?.Invoke("Parsing vertex data....");
                    var vertices = ReadGfxVertices(
                        reader,
                        gfxMapTrZoneAsset.GfxVertexPositionsPointer,
                        gfxMapTrZoneAsset.GfxVertexUVsPointer,
                        gfxMapTrZoneAsset.GfxVertexNormalsPointer,
                        gfxMapTrZoneAsset.GfxVertexCount);
                    printCallback?.Invoke(String.Format("Parsed vertex data in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));

                    // Reset timer
                    stopWatch.Restart();

                    // Read Indices
                    printCallback?.Invoke("Parsing surface indices....");
                    var indices = ReadGfxIndices(reader, gfxMapAsset.GfxIndicesPointer, (int)gfxMapAsset.GfxIndicesCount);
                    printCallback?.Invoke(String.Format("Parsed indices in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));

                    // Reset timer
                    stopWatch.Restart();
                    // Read Indices
                    printCallback?.Invoke("Parsing surfaces....");
                    var surfaces = ReadGfxSufaces(reader, gfxMapAsset.GfxSurfacesPointer, gfxMapAsset.SurfaceCount);
                    printCallback?.Invoke(String.Format("Parsed surfaces in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));

                    // Reset timer
                    stopWatch.Restart();

                    // Write OBJ
                    printCallback?.Invoke("Converting to OBJ....");

                    // Create new OBJ
                    var obj = new WavefrontOBJ();

                    // Append Vertex Data
                    foreach (var vertex in vertices)
                    {
                        obj.Vertices.Add(vertex.Position);
                        obj.Normals.Add(vertex.Normal);
                        obj.UVs.Add(vertex.UV);
                    }

                    // Image Names (for Search String)
                    HashSet<string> imageNames = new HashSet<string>();
                    int x = 0;
                    // Append Faces
                    foreach (var surface in surfaces)
                    {
                        // Create new Material
                        var material = ReadMaterial(reader, surface.MaterialPointer);
                        // Add to images
                        imageNames.Add(material.DiffuseMap);
                        // Add it
                        obj.AddMaterial(material);
                        // Add points
                        for (ushort i = 0; i < surface.FaceCount; i++)
                        {
                            // Face Indices
                            var faceIndex1 = indices[i * 3 + surface.FaceIndex] + surface.VertexIndex;
                            var faceIndex2 = indices[i * 3 + surface.FaceIndex + 1] + surface.VertexIndex;
                            var faceIndex3 = indices[i * 3 + surface.FaceIndex + 2] + surface.VertexIndex;

                            // Validate unique points, and write to OBJ
                            if (faceIndex1 != faceIndex2 && faceIndex1 != faceIndex3 && faceIndex2 != faceIndex3)
                            {
                                // new Obj Face
                                var objFace = new WavefrontOBJ.Face(material.Name);

                                // Add points
                                objFace.Vertices[0] = new WavefrontOBJ.Face.Vertex(faceIndex1, faceIndex1, faceIndex1);
                                objFace.Vertices[2] = new WavefrontOBJ.Face.Vertex(faceIndex2, faceIndex2, faceIndex2);
                                objFace.Vertices[1] = new WavefrontOBJ.Face.Vertex(faceIndex3, faceIndex3, faceIndex3);

                                // Add to OBJ
                                obj.Faces.Add(objFace);
                            }
                        }

                        x++;
                    }

                    // Save it
                    obj.Save(outputName + ".obj");

                    // Build search strinmg
                    string searchString = "";

                    // Loop through images, and append each to the search string (for Wraith/Greyhound)
                    foreach (string imageName in imageNames)
                        searchString += String.Format("{0},", Path.GetFileNameWithoutExtension(imageName));

                    // Dump it
                    File.WriteAllText(outputName + "_search_string.txt", searchString);

                    // Read entities and dump to map
                    mapFile.Entities.AddRange(ReadStaticModels(reader, gfxMapAsset.GfxStaticModelsPointer, (int)gfxMapAsset.GfxStaticModelsCount));
                    mapFile.DumpToMap(outputName + ".map");

                    // Done
                    printCallback?.Invoke(String.Format("Converted to OBJ in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));
                }

            }
            else
            {
                printCallback?.Invoke("Call of Duty: World War II is supported, but this EXE is not.");
            }
        }

        /// <summary>
        /// Reads Gfx Surfaces
        /// </summary>
        public static GfxSurface[] ReadGfxSufaces(ProcessReader reader, long address, int count)
        {
            // Preallocate array
            GfxSurface[] surfaces = new GfxSurface[count];

            // Loop number of indices we have
            for (int i = 0; i < count; i++)
                // Add it
                surfaces[i] = reader.ReadStruct<GfxSurface>(address + i * 56);

            // Done
            return surfaces;
        }


        /// <summary>
        /// Reads Gfx Vertex Indices
        /// </summary>
        public static ushort[] ReadGfxIndices(ProcessReader reader, long address, int count)
        {
            // Preallocate short array
            ushort[] indices = new ushort[count];
            // Read buffer
            var byteBuffer = reader.ReadBytes(address, count * 2);
            // Copy buffer 
            Buffer.BlockCopy(byteBuffer, 0, indices, 0, byteBuffer.Length);
            // Done
            return indices;
        }

        /// <summary>
        /// Reads Gfx Vertices
        /// </summary>
        public static Vertex[] ReadGfxVertices(ProcessReader reader, long positionsAddress, long uvsAddress, long normalsAddress, int count)
        {
            // Preallocate vertex array
            Vertex[] vertices = new Vertex[count];
            // Read buffer
            var positionsBuffer = reader.ReadBytes(positionsAddress, count * 12);
            var uvsBuffer = reader.ReadBytes(uvsAddress, count * 8);
            var normalsBuffer = reader.ReadBytes(normalsAddress, count * 4);
            // Loop number of vertices we have
            for (int i = 0; i < count; i++)
            {
                // Read Struct
                var gfxVertexPosition = ByteUtil.BytesToStruct<GfxVertexPosition>(positionsBuffer, i * 12);
                var gfxVertexUV = ByteUtil.BytesToStruct<GfxVertexUV>(uvsBuffer, i * 8);
                var gfxVertexNormal = ByteUtil.BytesToStruct<PackedUnitVector>(normalsBuffer, i * 4);

                // Create new SEModel Vertex
                vertices[i] = new Vertex()
                {
                    // Set offset
                    Position = new Vector3(
                        gfxVertexPosition.X * 2.54,
                        gfxVertexPosition.Y * 2.54,
                        gfxVertexPosition.Z * 2.54),
                    // Decode and set normal (from DTZxPorter - Wraith, same as XModels)
                    Normal = VertexNormalUnpacking.MethodB(gfxVertexNormal),
                    // Set UV
                    UV = new Vector2(gfxVertexUV.U, 1 - gfxVertexUV.V)
                };
            }

            // Done
            return vertices;
        }

        /// <summary>
        /// Reads a material for the given surface and its associated images
        /// </summary>
        public static WavefrontOBJ.Material ReadMaterial(ProcessReader reader, long address)
        {
            // Read Material
            var material = reader.ReadStruct<Material>(address);
            // Create new OBJ Image
            var objMaterial = new WavefrontOBJ.Material(Path.GetFileNameWithoutExtension(reader.ReadNullTerminatedString(reader.ReadInt64(address)).Replace("*", "")));
            // Loop over images
            for (byte i = 0; i < material.ImageCount; i++)
            {
                // Read Material Image
                var materialImage = reader.ReadStruct<MaterialImage64A>(material.ImageTablePointer + i * Marshal.SizeOf<MaterialImage64A>());
                // Check for color map for now
                if (materialImage.SemanticHash == 0xA0AB1041)
                    objMaterial.DiffuseMap = "_images\\\\" + reader.ReadNullTerminatedString(reader.ReadInt64(materialImage.ImagePointer)) + ".png";
            }
            // Done
            return objMaterial;
        }

        /// <summary>
        /// Reads Static Models
        /// </summary>
        public unsafe static List<IWMap.Entity> ReadStaticModels(ProcessReader reader, long address, int count)
        {
            // Resulting Entities
            List<IWMap.Entity> entities = new List<IWMap.Entity>(count);
            // Read buffer
            var byteBuffer = reader.ReadBytes(address, count * Marshal.SizeOf<GfxStaticModel>());
            // Loop number of models we have
            for (int i = 0; i < count; i++)
            {
                // Read Struct
                var staticModel = ByteUtil.BytesToStruct<GfxStaticModel>(byteBuffer, i * Marshal.SizeOf<GfxStaticModel>());
                // Model Name
                var modelName = reader.ReadNullTerminatedString(reader.ReadInt64(staticModel.ModelPointer));
                // New Matrix
                var matrix = new Rotation.Matrix();
                // Copy X Values
                matrix.Values[0] = staticModel.Matrix[0];
                matrix.Values[1] = staticModel.Matrix[1];
                matrix.Values[2] = staticModel.Matrix[2];
                // Copy Y Values
                matrix.Values[4] = staticModel.Matrix[3];
                matrix.Values[5] = staticModel.Matrix[4];
                matrix.Values[6] = staticModel.Matrix[5];
                // Copy Z Values
                matrix.Values[8] = staticModel.Matrix[6];
                matrix.Values[9] = staticModel.Matrix[7];
                matrix.Values[10] = staticModel.Matrix[8];
                // Convert to Euler
                var euler = matrix.ToEuler();
                // Add it
                entities.Add(IWMap.Entity.CreateMiscModel(modelName, new Vector3(staticModel.X, staticModel.Y, staticModel.Z), Rotation.ToDegrees(euler), staticModel.ModelScale));
            }
            // Done
            return entities;
        }
    }
}
