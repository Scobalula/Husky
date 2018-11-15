using PhilLibX;
using PhilLibX.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SELib;
using SELib.Utilities;
using System.Diagnostics;


namespace Husky
{
    class InfiniteWarfare
    {
        /// <summary>
        /// Entity Map Keys 
        /// </summary>
        public static Dictionary<string, string> EntityMapKeys = new Dictionary<string, string>()
        {
            { "170",    "classname" },
            { "738",    "origin" },
            { "65",     "angles" },
            { "1395",   "_color" },
            { "1188",   "target" },
            { "1190",   "targetname" },
            { "668",    "model" },
            { "22554",  "model_scale" },
        };

        /// <summary>
        /// Reads Modern Warfare RM BSP Data
        /// </summary>
        public static void ExportBSPData(ProcessReader reader, long assetPoolsAddress, long assetSizesAddress)
        {
            // Found her
            Printer.WriteLine("INFO", "Found supported game: Call of Duty: Ghosts");

            // Validate by XModel Name
            if (reader.ReadNullTerminatedString(reader.ReadInt64(reader.ReadInt64(assetPoolsAddress + 0x20) + 8)) == "void")
            {
                // Load BSP Pools (they only have a size of 1 so we don't care about reading more than 1)
                var gfxMapAsset = reader.ReadStruct<GfxMapGhosts>(reader.ReadInt64(assetPoolsAddress + 0xD0));

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

                    // Create OBJ output
                    using (StreamWriter writer = new StreamWriter(Path.ChangeExtension(gfxMapName, ".obj")))
                    {
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
                            var materialName = Path.GetFileNameWithoutExtension(reader.ReadNullTerminatedString(reader.ReadInt64(surface.MaterialPointer)).Replace("*", ""));

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

                    // Done
                    Printer.WriteLine("INFO", String.Format("Converted to OBJ in {0:0.00} seconds.", stopWatch.ElapsedMilliseconds / 1000.0));
                }

            }
            else
            {
                Printer.WriteLine("ERROR", "Call of Duty: Ghosts is supported, but this EXE is not.", ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Reads Gfx Surfaces
        /// </summary>
        public static GfxSurfaceGhosts[] ReadGfxSufaces(ProcessReader reader, long address, int count)
        {
            // Preallocate short array
            GfxSurfaceGhosts[] surfaces = new GfxSurfaceGhosts[count];

            // Loop number of indices we have
            for (int i = 0; i < count; i++)
                // Add it
                surfaces[i] = reader.ReadStruct<GfxSurfaceGhosts>(address + i * 40);

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

            // Loop number of indices we have
            for (int i = 0; i < count; i++)
                // Add 16bit index
                indices[i] = reader.ReadUInt16(address + i * 2);

            // Done
            return indices;
        }

        /// <summary>
        /// Reads Gfx Vertices from MWR
        /// </summary>
        public static SEModelVertex[] ReadGfxVertices(ProcessReader reader, long address, int count)
        {
            // Preallocate vertex array
            SEModelVertex[] vertices = new SEModelVertex[count];

            // Loop number of vertices we have
            for (int i = 0; i < count; i++)
            {
                // Read Gfx Vertex
                var gfxVertex = reader.ReadStruct<GfxVertex>(address + i * 44);

                // Create new SEModel Vertex
                vertices[i] = new SEModelVertex()
                {
                    // Set offset
                    Position = new Vector3(
                        gfxVertex.X,
                        gfxVertex.Y,
                        gfxVertex.Z),
                    // Decode and set normal (from DTZxPorter - Wraith, same as XModels)
                    VertexNormal = new Vector3(
                        (float)(((gfxVertex.Normal & 0x3FF) / 1023.0) * 2.0 - 1.0),
                        (float)((((gfxVertex.Normal >> 10) & 0x3FF) / 1023.0) * 2.0 - 1.0),
                        (float)((((gfxVertex.Normal >> 20) & 0x3FF) / 1023.0) * 2.0 - 1.0)),

                };

                // Set UV
                vertices[i].UVSets.Add(new Vector2(gfxVertex.U, gfxVertex.V));
            }

            // Done
            return vertices;
        }
    }
}