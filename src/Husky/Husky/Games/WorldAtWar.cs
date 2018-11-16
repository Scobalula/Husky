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
using SELib;
using SELib.Utilities;
using System.Diagnostics;
// ApexModder: 16/11/18 - Added to use List<> type
using System.Collections.Generic;

namespace Husky
{
    /// <summary>
    /// WAW Logic
    /// </summary>
    class WorldatWar
    {
        /// <summary>
        /// Reads BSP Data
        /// </summary>
        public static void ExportBSPData(ProcessReader reader, long assetPoolsAddress, long assetSizesAddress)
        {
            // Found her
            Printer.WriteLine("INFO", "Found supported game: Call of Duty: World At War");

            // Get XModel Name 
            var firstXModelName = reader.ReadNullTerminatedString(reader.ReadInt32(reader.ReadInt32(assetPoolsAddress + 0x14) + 4));

            // Validate by XModel Name
            if (firstXModelName == "void" || firstXModelName == "defaultactor" || firstXModelName == "defaultweapon")
            {
                // Load BSP Pools (they only have a size of 1 so we have no free header)
                var gfxMapAsset = reader.ReadStruct<GfxMapWaW>(reader.ReadInt32(assetPoolsAddress + 0x44));

                // Name
                string gfxMapName = reader.ReadNullTerminatedString(gfxMapAsset.NamePointer);
                string mapName = reader.ReadNullTerminatedString(gfxMapAsset.MapNamePointer);

                // Verify a BSP is actually loaded (if in base menu, etc, no map is loaded)
                if (String.IsNullOrWhiteSpace(gfxMapName))
                {
                    Printer.WriteLine("ERROR", "No BSP loaded. Enter Main Menu or a Map to load in the required assets.", ConsoleColor.DarkRed);
                }
                else
                {
                    // Print Info
                    Printer.WriteLine("INFO", String.Format("Loaded Gfx Map     -   {0}", gfxMapName));
                    Printer.WriteLine("INFO", String.Format("Loaded Map         -   {0}", mapName));
                    Printer.WriteLine("INFO", String.Format("Vertex Count       -   {0}", gfxMapAsset.GfxVertexCount));
                    Printer.WriteLine("INFO", String.Format("Indices Count      -   {0}", gfxMapAsset.GfxIndicesCount));
                    Printer.WriteLine("INFO", String.Format("Surface Count      -   {0}", gfxMapAsset.SurfaceCount));

                    // Stop watch
                    var stopWatch = Stopwatch.StartNew();

                    // Read Vertices
                    Printer.WriteLine("INFO", "Parsing vertex data....");
                    var vertices = ReadGfxVertices(reader, gfxMapAsset.GfxVerticesPointer, (int)gfxMapAsset.GfxVertexCount);
                    Printer.WriteLine("INFO", String.Format("Parsed vertex data in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));

                    // Reset timer
                    stopWatch.Restart();

                    // Read Indices
                    Printer.WriteLine("INFO", "Parsing surface indices....");
                    var indices = ReadGfxIndices(reader, gfxMapAsset.GfxIndicesPointer, (int)gfxMapAsset.GfxIndicesCount);
                    Printer.WriteLine("INFO", String.Format("Parsed indices in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));

                    // Reset timer
                    stopWatch.Restart();

                    // Read Indices
                    Printer.WriteLine("INFO", "Parsing surfaces....");
                    var surfaces = ReadGfxSufaces(reader, gfxMapAsset.GfxSurfacesPointer, gfxMapAsset.SurfaceCount);
                    Printer.WriteLine("INFO", String.Format("Parsed surfaces in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));

                    // Reset timer
                    stopWatch.Restart();

                    // Write SEModel
                    Printer.WriteLine("INFO", "Converting to OBJ....");

                    // Create Dir
                    Directory.CreateDirectory(Path.GetDirectoryName(gfxMapName));

                    // ApexModder: 16/11/18 - Store materials
                    var materialNames = new List<String>();

                    // ApexModder: 16/11/18 - Store obj path as its used more than once
                    var mtlFilePath = Path.ChangeExtension(gfxMapName, ".mtl");

                    // Create OBJ output
                    using (StreamWriter writer = new StreamWriter(Path.ChangeExtension(gfxMapName, ".obj")))
                    {
                        // ApexModder: 16/11/18 - Reference the .MTL file we generate below
                        writer.WriteLine("mtllib {0}", Path.GetFileName(mtlFilePath)); // mtllib <mapname>.mtl

                        // Dump vertex data
                        foreach (var vertex in vertices)
                        {
                            writer.WriteLine("v {0} {1} {2}", vertex.Position.X, vertex.Position.Y, vertex.Position.Z);
                            writer.WriteLine("vn {0} {1} {2}", vertex.VertexNormal.X, vertex.VertexNormal.Y, vertex.VertexNormal.Z);
                            writer.WriteLine("vt {0} {1}", vertex.UVSets[0].X, vertex.UVSets[0].Y);
                        }

                        // Dump Surfaces
                        foreach (var surface in surfaces)
                        {
                            // Get Material Name, purge any prefixes and Auto-Gen star characters
                            var materialName = Path.GetFileNameWithoutExtension(reader.ReadNullTerminatedString(reader.ReadInt32(surface.MaterialPointer)).Replace("*", ""));

                            // ApexModder: 16/11/18 - Store if not already in array
                            if (!materialNames.Contains(materialName))
                                materialNames.Add(materialName);

                            // Write MTL and Group
                            writer.WriteLine("g {0}", materialName);
                            writer.WriteLine("usemtl {0}", materialName);
                            // Add points
                            for (ushort i = 0; i < surface.FaceCount; i++)
                            {
                                // Face Indices
                                var faceIndex1 = indices[i * 3 + surface.FaceIndex] + surface.VertexIndex + 1;
                                var faceIndex2 = indices[i * 3 + surface.FaceIndex + 1] + surface.VertexIndex + 1;
                                var faceIndex3 = indices[i * 3 + surface.FaceIndex + 2] + surface.VertexIndex + 1;

                                // Validate unique points, and write to OBJ
                                if (faceIndex1 != faceIndex2 && faceIndex1 != faceIndex3 && faceIndex2 != faceIndex3)
                                    writer.WriteLine("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}",
                                        faceIndex1,
                                        faceIndex3,
                                        faceIndex2);
                            }
                        }
                    }

                    // ApexModder: 16/11/18 - Write a .MTL file for the .OBJ file
                    using (StreamWriter mtlWriter = new StreamWriter(mtlFilePath))
                    {
                        foreach (var materialName in materialNames)
                        {
                            mtlWriter.WriteLine("newmtl {0}", materialName);
                            // ApexModder: 16/11/18 - flatwhite material
                            mtlWriter.WriteLine("Ka 0.5000 0.5000 0.5000");
                            mtlWriter.WriteLine("Kd 1.0000 1.0000 1.0000");
                            mtlWriter.WriteLine("illum 1");
                            // ApexModder: 16/11/18 - seperate each material def with blank line
                            mtlWriter.WriteLine();
                        }
                    }

                    // Done
                    Printer.WriteLine("INFO", String.Format("Converted to OBJ in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));
                }

            }
            else
            {
                Printer.WriteLine("ERROR", "Call of Duty: World At War is supported, but this EXE is not.", ConsoleColor.DarkRed);
            }
        }

        /// <summary>
        /// Reads Gfx Surfaces
        /// </summary>
        public static GfxSurfaceWaW[] ReadGfxSufaces(ProcessReader reader, long address, int count)
        {
            // Preallocate short array
            GfxSurfaceWaW[] surfaces = new GfxSurfaceWaW[count];

            // Loop number of indices we have
            for (int i = 0; i < count; i++)
            {
                // Add it
                surfaces[i] = reader.ReadStruct<GfxSurfaceWaW>(address + i * 52);

            }

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
        public static SEModelVertex[] ReadGfxVertices(ProcessReader reader, long address, int count)
        {
            // Preallocate vertex array
            SEModelVertex[] vertices = new SEModelVertex[count];
            // Read buffer
            var byteBuffer = reader.ReadBytes(address, count * 44);
            // Loop number of vertices we have
            for (int i = 0; i < count; i++)
            {
                // Grab Offset
                float x = BitConverter.ToSingle(byteBuffer, i * 44);
                float y = BitConverter.ToSingle(byteBuffer, i * 44 + 4);
                float z = BitConverter.ToSingle(byteBuffer, i * 44 + 8);

                // Grab UV
                float u = BitConverter.ToSingle(byteBuffer, i * 44 + 20);
                float v = BitConverter.ToSingle(byteBuffer, i * 44 + 24);

                // Grab Normal
                int vertexNormal = BitConverter.ToInt32(byteBuffer, i * 44 + 36);

                // Decode the scale of the vector
                float DecodeScale = (float)((float)((vertexNormal & 0xFF000000) >> 24) - -192.0) / 32385.0f;

                // Create new SEModel Vertex
                vertices[i] = new SEModelVertex()
                {
                    // Set offset
                    Position = new Vector3(
                        x,
                        y,
                        z),
                    // Decode and set normal (from DTZxPorter - Wraith, same as XModels)
                    VertexNormal = new Vector3(
                        (float)((float)(vertexNormal & 0xFF) - 127.0) * DecodeScale,
                        (float)((float)((vertexNormal & 0xFF00) >> 8) - 127.0) * DecodeScale,
                        (float)((float)((vertexNormal & 0xFF0000) >> 16) - 127.0) * DecodeScale)

                };

                // Set UV
                vertices[i].UVSets.Add(new Vector2(u, v));
            }

            // Done
            return vertices;
        }
    }
}