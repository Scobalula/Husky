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
// TODO: Fix up a some code such as newly added material shite
using System;
using System.IO;
using System.Collections.Generic;

namespace Husky
{
    /// <summary>
    /// OBJ Exception Class
    /// </summary>
    public class OBJReadException : Exception
    {
        /// <summary>
        /// Initializes OBJ Read Exception
        /// </summary>
        public OBJReadException() { }

        /// <summary>
        /// Initializes OBJ Read Exception with a Message
        /// </summary>
        /// <param name="message">Exception Message</param>
        public OBJReadException(string message) : base(message) { }

        /// <summary>
        /// Initializes OBJ Read Exception with Message and Inner Exception
        /// </summary>
        /// <param name="message">Exception Message</param>
        /// <param name="inner">Inner Exception</param>
        public OBJReadException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Wavefront OBJ Model Class
    /// </summary>
    public class WavefrontOBJ
    {

        public string Name { get; set; }
        /// <summary>
        /// OBJ Material Class
        /// </summary>
        public class Material
        {
            /// <summary>
            /// Material Name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Diffuse/Color Map
            /// </summary>
            public string DiffuseMap { get; set; }

            /// <summary>
            /// Normal/Bump Map
            /// </summary>
            public string NormalMap { get; set; }

            /// <summary>
            /// Specular Map
            /// </summary>
            public string SpecularMap { get; set; }

            /// <summary>
            /// Initializes an OBJ Material with a Name
            /// </summary>
            /// <param name="name">Material Name</param>
            public Material(string name)
            {
                Name = name;
            }
        }

        /// <summary>
        /// OBJ Polygon Face Class
        /// </summary>
        public class Face
        {
            /// <summary>
            /// Face Vertex with Vert, Normal and UV index
            /// </summary>
            public class Vertex
            {
                /// <summary>
                /// Vertex Index
                /// </summary>
                public int VertexIndex { get; set; }

                /// <summary>
                /// Normal Index
                /// </summary>
                public int NormalIndex { get; set; }

                /// <summary>
                /// UV Index
                /// </summary>
                public int UVIndex { get; set; }

                /// <summary>
                /// Initializes Face Vertex with Indices
                /// </summary>
                /// <param name="vertexIndex">Vertex Index</param>
                /// <param name="normalIndex">Normal Index</param>
                /// <param name="uvIndex">UV Index</param>
                public Vertex(int vertexIndex, int normalIndex, int uvIndex)
                {
                    VertexIndex = vertexIndex;
                    NormalIndex = normalIndex;
                    UVIndex = uvIndex;
                }
            }

            /// <summary>
            /// Material Index
            /// </summary>
            public string MaterialName { get; set; }

            /// <summary>
            /// Face Vertex Points
            /// </summary>
            public Vertex[] Vertices = new Vertex[3];

            /// <summary>
            /// Initializes Face with Material Index
            /// </summary>
            /// <param name="materialIndex">Material Index</param>
            public Face(string materialName)
            {
                MaterialName = materialName;
            }
        }

        /// <summary>
        /// Random Float (For Material Colors)
        /// </summary>
        private readonly Random RandomInt = new Random();

        /// <summary>
        /// Vertex Points
        /// </summary>
        public List<Vector3> Vertices = new List<Vector3>();

        /// <summary>
        /// Vertex Normals
        /// </summary>
        public List<Vector3> Normals = new List<Vector3>();

        /// <summary>
        /// UV Texture Cooridinates
        /// </summary>
        public List<Vector2> UVs = new List<Vector2>();

        /// <summary>
        /// Faces
        /// </summary>
        public List<Face> Faces = new List<Face>();

        /// <summary>
        /// Materials
        /// </summary>
        public Dictionary<string, Material> Materials = new Dictionary<string, Material>();

        /// <summary>
        /// Comments
        /// </summary>
        public List<string> Comments = new List<string>();

        /// <summary>
        /// Active Material
        /// </summary>
        private string ActiveMaterial { get; set; }

        /// <summary>
        /// Active OBJ File Line Number
        /// </summary>
        private long ActiveLine { get; set; }
        
        /// <summary>
        /// Initializes OBJ 
        /// </summary>
        public WavefrontOBJ()
        {
            // Default
        }

        
        public WavefrontOBJ(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Loads an OBJ File
        /// </summary>
        /// <param name="fileName">File path</param>
        public void Load(string fileName)
        {
            ActiveLine = 0;

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                string[] lineSplit;

                while ((line = reader.ReadLine()?.Trim()) != null)
                {
                    ActiveLine++;

                    if (String.IsNullOrWhiteSpace(line))
                        continue;

                    if (line[0] == '#')
                        continue;

                    lineSplit = line.Split();

                    switch (lineSplit[0])
                    {
                        case "v":
                            LoadVertexPoint(lineSplit);
                            break;
                        case "vt":
                            LoadUVPoint(lineSplit);
                            break;
                        case "vn":
                            LoadNormal(lineSplit);
                            break;
                        case "f":
                            LoadFace(lineSplit);
                            break;
                        case "usemtl":
                            LoadMaterial(lineSplit);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Loads material name from the OBJ File
        /// </summary>
        /// <param name="lineSplit">Line split with material name</param>
        private void LoadMaterial(string[] lineSplit)
        {
            if (lineSplit.Length < 2)
                throw new OBJReadException("Hit material line but no material name present.");

            string materialName = lineSplit[1];

            AddMaterial(new Material(materialName));

            ActiveMaterial = materialName;
        }

        /// <summary>
        /// Loads a vertex point from the OBJ File
        /// </summary>
        /// <param name="lineSplit">Line split containing x,y,z as indexes 1,2,3</param>
        private void LoadVertexPoint(string[] lineSplit)
        {
            if (!Single.TryParse(lineSplit[1], out float x))
                throw new OBJReadException(String.Format("Failed to parse an X Value from Vertex::{0} Line::{1}", Vertices.Count, ActiveLine));

            if (!Single.TryParse(lineSplit[2], out float y))
                throw new OBJReadException(String.Format("Failed to parse a Y Value from Vertex::{0} Line::{1}", Vertices.Count, ActiveLine));

            if (!Single.TryParse(lineSplit[3], out float z))
                throw new OBJReadException(String.Format("Failed to parse a Z Value from Vertex::{0} Line::{1}", Vertices.Count, ActiveLine));

            Vertices.Add(new Vector3(x, y, z));
        }

        /// <summary>
        /// Loads a UV Texture Point from the OBJ File
        /// </summary>
        /// <param name="lineSplit">Line split containing u,v as indexes 1,2</param>
        private void LoadUVPoint(string[] lineSplit)
        {
            if (!Single.TryParse(lineSplit[1], out float u))
                throw new OBJReadException(String.Format("Failed to parse a U Value from UV::{0} Line::{1}", UVs.Count, ActiveLine));

            if (!Single.TryParse(lineSplit[2], out float v))
                throw new OBJReadException(String.Format("Failed to parse a V Value from UV::{0} Line::{1}", UVs.Count, ActiveLine));

            UVs.Add(new Vector2(u, v));
        }

        /// <summary>
        /// Loads a vertex point from the OBJ File
        /// </summary>
        /// <param name="lineSplit">Line split containing x,y,z as indexes 1,2,3</param>
        private void LoadNormal(string[] lineSplit)
        {
            if (!Single.TryParse(lineSplit[1], out float x))
                throw new OBJReadException(String.Format("Failed to parse an X Value from Normal::{0} Line::{1}", Normals.Count, ActiveLine));

            if (!Single.TryParse(lineSplit[2], out float y))
                throw new OBJReadException(String.Format("Failed to parse a Y Value from Normal::{0} Line::{1}", Normals.Count, ActiveLine));

            if (!Single.TryParse(lineSplit[3], out float z))
                throw new OBJReadException(String.Format("Failed to parse a Z Value from Normal::{0} Line::{1}", Normals.Count, ActiveLine));

            Normals.Add(new Vector3(x, y, z));
        }

        /// <summary>
        /// Loads a Polygon Face from the OBJ File.
        /// </summary>
        /// <param name="lineSplit">Line split containing face data.</param>
        private void LoadFace(string[] lineSplit)
        {
            if (lineSplit.Length != 4)
                throw new OBJReadException(String.Format("Only polygons are supported, {1} indices on Face::{0} Line::{2}", Faces.Count, lineSplit.Length - 1, ActiveLine));

            Face face = new Face(ActiveMaterial);

            for (int i = 1; i < 4; i++)
            {
                string[] faceSplit = lineSplit[i].Split('/');

                if (faceSplit.Length != 3)
                    throw new OBJReadException(String.Format("No UV and/or Normal index on Face::{0} Vertex::{1} Line::{2}", Faces.Count, i, ActiveLine));

                if (!Int32.TryParse(faceSplit[0], out int vertexIndex))
                    throw new OBJReadException(String.Format("Failed to parse Vertex Index on Face::{0} Vertex::{1} Line::{2}", Faces.Count, i, ActiveLine));

                if (!Int32.TryParse(faceSplit[1], out int uvIndex))
                    throw new OBJReadException(String.Format("Failed to parse UV Index on Face::{0} Vertex::{1} Line::{2}", Faces.Count, i, ActiveLine));

                if (!Int32.TryParse(faceSplit[2], out int normalIndex))
                    throw new OBJReadException(String.Format("Failed to parse Normal Index on Face::{0} Vertex::{1} Line::{2}", Faces.Count, i, ActiveLine));

                face.Vertices[i - 1] = new Face.Vertex
                (
                    vertexIndex - 1,
                    uvIndex - 1,
                    normalIndex - 1
                );
            }

            Faces.Add(face);
        }

        /// <summary>
        /// Adds a material to the OBJ file, if a material with the same name exists already, the previous material is overwritten
        /// </summary>
        /// <param name="material"></param>
        public void AddMaterial(Material material)
        {
            Materials[material.Name] = material;
        }

        /// <summary>
        /// Saves OBJ Object to a file
        /// </summary>
        /// <param name="outputPath">Output file path</param>
        public void Save(string outputPath, bool randomizeColors = false)
        {
            // Get folder
            string folder = Path.GetDirectoryName(outputPath);
            // Create it
            Directory.CreateDirectory(folder);
            // Material Tracker
            string materialNameTracker = null;

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                foreach (string comment in Comments)
                    writer.WriteLine("# {0}", comment);

                writer.WriteLine("# Export Path : {0}", outputPath);
                writer.WriteLine("# Export Time : {0}", DateTime.Now);
                writer.WriteLine("# Vertex Count: {0}", Vertices.Count);
                writer.WriteLine("# Normal Count: {0}", Normals.Count);
                writer.WriteLine("# UV Count    : {0}", UVs.Count);
                writer.WriteLine("# Face Count  : {0}", Faces.Count);
                writer.WriteLine();

                foreach (var vertex in Vertices)
                    writer.WriteLine("v {0:0.00000} {1:0.00000} {2:0.00000}",
                        vertex.X,
                        vertex.Y,
                        vertex.Z);
                writer.WriteLine();
                foreach (var normal in Normals)
                    writer.WriteLine("vn {0:0.00000} {1:0.00000} {2:0.00000}",
                        normal.X,
                        normal.Y,
                        normal.Z);
                writer.WriteLine();
                foreach (var uv in UVs)
                    writer.WriteLine("vt {0:0.00000} {1:0.00000}",
                        uv.X,
                        uv.Y);
                writer.WriteLine();
                foreach (var face in Faces)
                {
                    if (face.MaterialName != materialNameTracker)
                    {
                        writer.WriteLine();
                        writer.WriteLine("g {0}", face.MaterialName);
                        writer.WriteLine("usemtl {0}", face.MaterialName);
                        writer.WriteLine("mtllib {0}", Path.GetFileName(Path.ChangeExtension(outputPath, ".mtl")));
                        materialNameTracker = face.MaterialName;
                    }
                    writer.Write("f");
                    foreach (var faceVertex in face.Vertices)
                        writer.Write(" {0}/{1}/{2}",
                            faceVertex.VertexIndex + 1,
                            faceVertex.UVIndex + 1,
                            faceVertex.NormalIndex + 1
                            );
                    writer.WriteLine();
                }
                writer.WriteLine();
            }

            // Write Material Library
            using (StreamWriter writer = new StreamWriter(Path.ChangeExtension(outputPath, ".mtl")))
            {
                // Loop over materials
                foreach (var material in Materials)
                {
                    // Write Name
                    writer.WriteLine("newmtl {0}", material.Key);
                    // Write Illumination model
                    writer.WriteLine("illum 4");
                    // Write colors, randomize diffuse if requested
                    writer.WriteLine("Kd {0:0.00} {1:0.00} {2:0.00}",
                        randomizeColors ? RandomInt.Next(128, 255) / 255.0 : 1.0,
                        randomizeColors ? RandomInt.Next(128, 255) / 255.0 : 1.0,
                        randomizeColors ? RandomInt.Next(128, 255) / 255.0 : 1.0);
                    writer.WriteLine("Ka 0.00 0.00 0.00");
                    writer.WriteLine("Ks 0.00 0.00 0.00");
                    // Write Maps, if we have them
                    if (!String.IsNullOrWhiteSpace(material.Value.DiffuseMap))
                        writer.WriteLine("map_Kd {0}", material.Value.DiffuseMap);
                    if (!String.IsNullOrWhiteSpace(material.Value.NormalMap))
                        writer.WriteLine("map_bump {0}", material.Value.NormalMap);
                    if (!String.IsNullOrWhiteSpace(material.Value.SpecularMap))
                        writer.WriteLine("map_Ks {0}", material.Value.SpecularMap);
                    // Space
                    writer.WriteLine();
                }
            }
        }
    }
}
