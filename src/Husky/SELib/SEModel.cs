using SELib.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
///   SEModel.cs
///   Author: DTZxPorter
///   Written for the SE Format Project
///   Follows SEModel specification v1.0
///   https://github.com/dtzxporter/SEModel-Docs/blob/master/spec.md
///

namespace SELib
{
    #region SEModel Enums

    /// <summary>
    /// Specifies what type of bone positioning to use in the model
    /// </summary>
    public enum ModelBoneSupportTypes : byte
    {
        /// <summary>
        /// The model will be saved with local bone matricies
        /// </summary>
        SupportsLocals,
        /// <summary>
        /// The model will be saved with global bone matricies
        /// </summary>
        SupportsGlobals,
        /// <summary>
        /// The model will be saved with both global and local matricies
        /// </summary>
        SupportsBoth
    }

    /// <summary>
    /// Specifies the data present for the model
    /// </summary>
    internal enum SEModel_DataPresenceFlags : byte
    {
        // Whether or not this model contains a bone block
        SEMODEL_PRESENCE_BONE = 1 << 0,
        // Whether or not this model contains submesh blocks
        SEMODEL_PRESENCE_MESH = 1 << 1,
        // Whether or not this model contains inline material blocks
        SEMODEL_PRESENCE_MATERIALS = 1 << 2,

        // The file contains a custom data block
        SEMODEL_PRESENCE_CUSTOM = 1 << 7,
    }

    /// <summary>
    /// Specifies the data present for each bone in the model
    /// </summary>
    internal enum SEModel_BoneDataPresenceFlags : byte
    {
        // Whether or not bones contain global-space matricies
        SEMODEL_PRESENCE_GLOBAL_MATRIX = 1 << 0,
        // Whether or not bones contain local-space matricies
        SEMODEL_PRESENCE_LOCAL_MATRIX = 1 << 1,

        // Whether or not bones contain scales
        SEMODEL_PRESENCE_SCALES = 1 << 2,
    }

    /// <summary>
    /// Specifies the data present for each vertex in the model
    /// </summary>
    internal enum SEModel_MeshDataPresenceFlags : byte
    {
        // Whether or not meshes contain at least 1 uv map
        SEMODEL_PRESENCE_UVSET = 1 << 0,

        // Whether or not meshes contain vertex normals
        SEMODEL_PRESENCE_NORMALS = 1 << 1,

        // Whether or not meshes contain vertex colors (RGBA)
        SEMODEL_PRESENCE_COLOR = 1 << 2,

        // Whether or not meshes contain at least 1 weighted skin
        SEMODEL_PRESENCE_WEIGHTS = 1 << 3,
    }

    #endregion

    #region SEModel Bone

    /// <summary>
    /// Contains information for a specific bone
    /// </summary>
    public class SEModelBone
    {
        /// <summary>
        /// Whether or not this bone is a root bone (No parent)
        /// </summary>
        public bool RootBone { get { return (BoneParent <= -1); } }

        /// <summary>
        /// Get or set the name of this bone
        /// </summary>
        public string BoneName { get; set; }
        /// <summary>
        /// Get or set the parent index of this bone, -1 for a root bone
        /// </summary>
        public int BoneParent { get; set; }

        /// <summary>
        /// Get or set the global position of this bone
        /// </summary>
        public Vector3 GlobalPosition { get; set; }
        /// <summary>
        /// Get or set the global rotation of this bone
        /// </summary>
        public Quaternion GlobalRotation { get; set; }

        /// <summary>
        /// Get or set the local position of this bone
        /// </summary>
        public Vector3 LocalPosition { get; set; }
        /// <summary>
        /// Get or set the local rotation of this bone
        /// </summary>
        public Quaternion LocalRotation { get; set; }

        /// <summary>
        /// Get or set the scale of this bone
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// Creates a new SEModelBone with the default settings
        /// </summary>
        public SEModelBone()
        {
            BoneName = string.Empty;
            BoneParent = -1;
            GlobalPosition = new Vector3() { X = 0, Y = 0, Z = 0 };
            LocalPosition = new Vector3() { X = 0, Y = 0, Z = 0 };
            GlobalRotation = new Quaternion() { X = 0, Y = 0, Z = 0, W = 1 };
            LocalRotation = new Quaternion() { X = 0, Y = 0, Z = 0, W = 1 };
            Scale = new Vector3() { X = 1, Y = 1, Z = 1 };
        }
    }

    #endregion

    #region SEModel Mesh

    public class SEModelMesh
    {
        /// <summary>
        /// Returns the count of verticies in the mesh
        /// </summary>
        public uint VertexCount { get { return (uint)Verticies.Count; } }
        /// <summary>
        /// Returns the count of faces in the mesh
        /// </summary>
        public uint FaceCount { get { return (uint)Faces.Count; } }

        /// <summary>
        /// A list of verticies in the mesh
        /// </summary>
        public List<SEModelVertex> Verticies { get; set; }
        /// <summary>
        /// A list of faces in the mesh, faces match D3DPT_TRIANGLELIST (DirectX) and VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST (Vulkan)
        /// </summary>
        public List<SEModelFace> Faces { get; set; }
        /// <summary>
        /// A list of material indicies per each UVLayer, -1 indicates no material assigned...
        /// </summary>
        public List<int> MaterialReferenceIndicies { get; set; }

        /// <summary>
        /// Creates a new mesh with default settings
        /// </summary>
        public SEModelMesh()
        {
            Verticies = new List<SEModelVertex>();
            Faces = new List<SEModelFace>();
            MaterialReferenceIndicies = new List<int>();
        }

        /// <summary>
        /// Adds the given vertex to the mesh
        /// </summary>
        /// <param name="Vertex">The vertex to add to the mesh</param>
        public void AddVertex(SEModelVertex Vertex)
        {
            // Add it
            Verticies.Add(Vertex);
        }

        /// <summary>
        /// Adds a new face to the mesh with the given indicies
        /// </summary>
        public void AddFace(uint Index1, uint Index2, uint Index3)
        {
            // Add new face
            Faces.Add(new SEModelFace(Index1, Index2, Index3));
        }

        /// <summary>
        /// Adds a new material index
        /// </summary>
        /// <param name="Index">The index of the material in the model, or -1 for null</param>
        public void AddMaterialIndex(int Index)
        {
            // Add new index
            MaterialReferenceIndicies.Add(Index);
        }
    }

    public class SEModelFace
    {
        /* Vertex indicies for this face */

        public uint FaceIndex1;
        public uint FaceIndex2;
        public uint FaceIndex3;

        /// <summary>
        /// Creates a new face with default settings
        /// </summary>
        public SEModelFace(uint Index1, uint Index2, uint Index3)
        {
            FaceIndex1 = Index1;
            FaceIndex2 = Index2;
            FaceIndex3 = Index3;
        }
    }

    public class SEModelVertex
    {
        /// <summary>
        /// Returns the amount of UVSets for this vertex
        /// </summary>
        public uint UVSetCount { get { return (uint)UVSets.Count; } }
        /// <summary>
        /// Returns the amount of skin influences for this vertex
        /// </summary>
        public uint WeightCount { get { return (uint)Weights.Count; } }

        /// <summary>
        /// The position of the vertex
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The uv sets for this vertex
        /// </summary>
        public List<Vector2> UVSets { get; set; }

        /// <summary>
        /// The vertex normal
        /// </summary>
        public Vector3 VertexNormal { get; set; }
        /// <summary>
        /// The vertex coloring
        /// </summary>
        public Color VertexColor { get; set; }

        /// <summary>
        /// A list of skin weights for this vertex
        /// </summary>
        public List<SEModelWeight> Weights { get; set; }

        /// <summary>
        /// Creates a new SEModelVertex using default settings
        /// </summary>
        public SEModelVertex()
        {
            Position = Vector3.Zero;
            UVSets = new List<Vector2>();
            VertexNormal = Vector3.Zero;
            VertexColor = Color.White;
            Weights = new List<SEModelWeight>();
        }
    }

    public class SEModelWeight
    {
        /// <summary>
        /// The bone index for this weight
        /// </summary>
        public uint BoneIndex { get; set; }
        /// <summary>
        /// The weight value, from 0.0 to 1.0 for this weight
        /// </summary>
        public float BoneWeight { get; set; }

        /// <summary>
        /// Creates a new SEModelWeight with default settings
        /// </summary>
        public SEModelWeight() { BoneIndex = 0; BoneWeight = 1.0f; }
    }

    public class SEModelUVSet
    {
        /// <summary>
        /// The UV coords for this UVSet
        /// </summary>
        public Vector2 UVCoord { get; set; }
        /// <summary>
        /// The material index of the UV, the index is within the models materials
        /// </summary>
        public uint MaterialIndex { get; set; }

        /// <summary>
        /// Creates a new SEModelUVSet with default settings
        /// </summary>
        public SEModelUVSet() { UVCoord = new Vector2(); MaterialIndex = 0; }
    }

    #endregion

    #region SEModel Material

    public class SEModelMaterial
    {
        /// <summary>
        /// The name of the material
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The material data, determined by type
        /// </summary>
        public object MaterialData { get; set; }
    }

    public class SEModelSimpleMaterial
    {
        public string DiffuseMap { get; set; }
        public string NormalMap { get; set; }
        public string SpecularMap { get; set; }
    }

    #endregion

    /// <summary>
    /// Represents a SEModel file, allows for reading and writing model data
    /// </summary>
    public class SEModel
    {
        /* Model data */

        /// <summary>
        /// A list of bones, in order by index
        /// </summary>
        public List<SEModelBone> Bones { get; private set; }
        /// <summary>
        /// A list of meshes, in order
        /// </summary>
        public List<SEModelMesh> Meshes { get; private set; }
        /// <summary>
        /// A list of materials, in order
        /// </summary>
        public List<SEModelMaterial> Materials { get; private set; }

        /// <summary>
        /// Gets the SEModel specification version this library supports
        /// </summary>
        public string APIVersion { get { return "v1.0.0"; } }

        /* Model properties */

        /// <summary>
        /// Returns the number of bones in this model, this is automatically updated
        /// </summary>
        public uint BoneCount { get { return (uint)Bones.Count; } }
        /// <summary>
        /// Returns the number of meshes in this model, this is automatically updated
        /// </summary>
        public uint MeshCount { get { return (uint)Meshes.Count; } }
        /// <summary>
        /// Returns the number of materials in this model, this is automatically updated
        /// </summary>
        public uint MaterialCount { get { return (uint)Materials.Count; } }

        /// <summary>
        /// Creates a new SEModel using default settings
        /// </summary>
        public SEModel()
        {
            Bones = new List<SEModelBone>();
            Meshes = new List<SEModelMesh>();
            Materials = new List<SEModelMaterial>();
        }

        /* Functions and utilities */

        #region Writing

        /// <summary>
        /// Saves the SEModel to a stream, following the current specification version, using the provided data
        /// </summary>
        /// <param name="Stream">The file stream to write to</param>
        public void Write(Stream Stream)
        {
            // Open up a binary writer
            using (ExtendedBinaryWriter writeFile = new ExtendedBinaryWriter(Stream))
            {
                // Write magic
                writeFile.Write(new char[] { 'S', 'E', 'M', 'o', 'd', 'e', 'l' });
                // Write version
                writeFile.Write((short)0x1);
                // Write header size
                writeFile.Write((short)0x14);

                // Build data present flags
                {
                    // Buffer
                    byte DataPresentFlags = 0x0;
                    // Check for bones
                    if (Bones.Count > 0)
                    {
                        DataPresentFlags |= (byte)SEModel_DataPresenceFlags.SEMODEL_PRESENCE_BONE;
                    }
                    // Check for meshes
                    if (Meshes.Count > 0)
                    {
                        DataPresentFlags |= (byte)SEModel_DataPresenceFlags.SEMODEL_PRESENCE_MESH;
                    }
                    // Check for materials
                    if (Materials.Count > 0)
                    {
                        DataPresentFlags |= (byte)SEModel_DataPresenceFlags.SEMODEL_PRESENCE_MATERIALS;
                    }
                    // Write it
                    writeFile.Write((byte)DataPresentFlags);
                }

                // Dynamic properties
                bool HasScales = false, HasLocals = true, HasGlobals = false;

                // Build bone data present flags
                {
                    // Buffer
                    byte DataPresentFlags = 0x0;

                    // Check for non-default bone data
                    foreach (var Bone in Bones)
                    {
                        if (Bone.Scale != Vector3.One)
                            HasScales = true;
                        if (Bone.LocalPosition != Vector3.Zero || Bone.LocalRotation != Quaternion.Identity)
                            HasLocals = true;
                        if (Bone.GlobalPosition != Vector3.Zero || Bone.GlobalRotation != Quaternion.Identity)
                            HasGlobals = true;

                        // Check to end
                        if (HasScales && HasLocals && HasGlobals)
                            break;
                    }

                    // Check for bone types
                    if (HasScales)
                        DataPresentFlags |= (byte)SEModel_BoneDataPresenceFlags.SEMODEL_PRESENCE_SCALES;
                    if (HasLocals)
                        DataPresentFlags |= (byte)SEModel_BoneDataPresenceFlags.SEMODEL_PRESENCE_LOCAL_MATRIX;
                    if (HasGlobals)
                        DataPresentFlags |= (byte)SEModel_BoneDataPresenceFlags.SEMODEL_PRESENCE_GLOBAL_MATRIX;

                    // Write it
                    writeFile.Write((byte)DataPresentFlags);
                }

                // Dynamic properties
                bool HasNormals = false, HasColors = false;

                // Build mesh data present flags
                {
                    // Buffer
                    byte DataPresentFlags = 0x0;

                    // Implied properties
                    bool HasUVSet = false, HasWeights = false;

                    // Check for non-default properties
                    foreach (var Mesh in Meshes)
                    {
                        foreach (var Vertex in Mesh.Verticies)
                        {
                            if (Vertex.UVSets.Count > 0)
                                HasUVSet = true;
                            if (Vertex.Weights.Count > 0)
                                HasWeights = true;
                            if (Vertex.VertexColor != Color.White)
                                HasColors = true;
                            if (Vertex.VertexNormal != Vector3.Zero)
                                HasNormals = true;

                            // Check to end
                            if (HasUVSet && HasWeights && HasNormals && HasColors)
                                break;
                        }

                        // Check to end
                        if (HasUVSet && HasWeights && HasNormals && HasColors)
                            break;
                    }

                    // Check for UVSets
                    if (HasUVSet)
                    {
                        DataPresentFlags |= (byte)SEModel_MeshDataPresenceFlags.SEMODEL_PRESENCE_UVSET;
                    }
                    // Check for normals
                    if (HasNormals)
                    {
                        DataPresentFlags |= (byte)SEModel_MeshDataPresenceFlags.SEMODEL_PRESENCE_NORMALS;
                    }
                    // Check for colors
                    if (HasColors)
                    {
                        DataPresentFlags |= (byte)SEModel_MeshDataPresenceFlags.SEMODEL_PRESENCE_COLOR;
                    }
                    // Check for weights
                    if (HasWeights)
                    {
                        DataPresentFlags |= (byte)SEModel_MeshDataPresenceFlags.SEMODEL_PRESENCE_WEIGHTS;
                    }

                    // Write it
                    writeFile.Write((byte)DataPresentFlags);
                }

                // The bonecount buffer
                uint BoneCountBuffer = BoneCount;
                // The meshcount buffer
                uint MeshCountBuffer = MeshCount;
                // The matcount buffer
                uint MatCountBuffer = MaterialCount;

                // Write count of bones
                writeFile.Write((uint)BoneCountBuffer);
                // Write count of meshes
                writeFile.Write((uint)MeshCountBuffer);
                // Write count of mats
                writeFile.Write((uint)MatCountBuffer);

                // Write 3 reserved bytes
                writeFile.Write(new byte[3] { 0x0, 0x0, 0x0 });

                // Write bone tagnames
                foreach (var Bone in Bones)
                    writeFile.WriteNullTermString(Bone.BoneName);

                // Write bone data
                foreach (var Bone in Bones)
                {
                    // Write bone flags
                    writeFile.Write((byte)0x0);

                    // Write parent index
                    writeFile.Write((int)Bone.BoneParent);

                    // Write global matrix
                    if (HasGlobals)
                    {
                        writeFile.Write((float)Bone.GlobalPosition.X);
                        writeFile.Write((float)Bone.GlobalPosition.Y);
                        writeFile.Write((float)Bone.GlobalPosition.Z);
                        writeFile.Write((float)Bone.GlobalRotation.X);
                        writeFile.Write((float)Bone.GlobalRotation.Y);
                        writeFile.Write((float)Bone.GlobalRotation.Z);
                        writeFile.Write((float)Bone.GlobalRotation.W);
                    }

                    // Write local matrix
                    if (HasLocals)
                    {
                        writeFile.Write((float)Bone.LocalPosition.X);
                        writeFile.Write((float)Bone.LocalPosition.Y);
                        writeFile.Write((float)Bone.LocalPosition.Z);
                        writeFile.Write((float)Bone.LocalRotation.X);
                        writeFile.Write((float)Bone.LocalRotation.Y);
                        writeFile.Write((float)Bone.LocalRotation.Z);
                        writeFile.Write((float)Bone.LocalRotation.W);
                    }

                    // Write scales
                    if (HasScales)
                    {
                        writeFile.Write((float)Bone.Scale.X);
                        writeFile.Write((float)Bone.Scale.Y);
                        writeFile.Write((float)Bone.Scale.Z);
                    }
                }

                // Write mesh data
                foreach (var Mesh in Meshes)
                {
                    // Write mesh flags
                    writeFile.Write((byte)0x0);

                    // Buffers for counts
                    byte MatIndiciesCount = 0, MaxSkinInfluence = 0;
                    int VertexCount = Mesh.Verticies.Count, FaceCount = Mesh.Faces.Count;

                    // Iterate and calculate indicies
                    foreach (var Vertex in Mesh.Verticies)
                    {
                        if (Vertex.UVSets.Count > MatIndiciesCount)
                            MatIndiciesCount = (byte)Vertex.UVSets.Count;
                        if (Vertex.Weights.Count > MaxSkinInfluence)
                            MaxSkinInfluence = (byte)Vertex.Weights.Count;
                    }

                    // Write material indicies
                    writeFile.Write((byte)MatIndiciesCount);
                    // Write max skin influence
                    writeFile.Write((byte)MaxSkinInfluence);
                    // Write vertex count
                    writeFile.Write((int)VertexCount);
                    // Write face count
                    writeFile.Write((int)FaceCount);

                    // Write positions
                    foreach (var Vertex in Mesh.Verticies)
                    {
                        writeFile.Write((float)Vertex.Position.X);
                        writeFile.Write((float)Vertex.Position.Y);
                        writeFile.Write((float)Vertex.Position.Z);
                    }

                    // Write uvlayers
                    foreach (var Vertex in Mesh.Verticies)
                    {
                        for (int i = 0; i < MatIndiciesCount; i++)
                        {
                            var Layer = (i < Vertex.UVSets.Count) ? Vertex.UVSets[i] : Vector2.Zero;

                            // Write it
                            writeFile.Write((float)Layer.X);
                            writeFile.Write((float)Layer.Y);
                        }
                    }

                    // Write normals
                    if (HasNormals)
                    {
                        foreach (var Vertex in Mesh.Verticies)
                        {
                            writeFile.Write((float)Vertex.VertexNormal.X);
                            writeFile.Write((float)Vertex.VertexNormal.Y);
                            writeFile.Write((float)Vertex.VertexNormal.Z);
                        }
                    }

                    // Write colors
                    if (HasColors)
                    {
                        foreach (var Vertex in Mesh.Verticies)
                        {
                            writeFile.Write((byte)Vertex.VertexColor.R);
                            writeFile.Write((byte)Vertex.VertexColor.G);
                            writeFile.Write((byte)Vertex.VertexColor.B);
                            writeFile.Write((byte)Vertex.VertexColor.A);
                        }
                    }

                    // Write weights
                    foreach (var Vertex in Mesh.Verticies)
                    {
                        for (int i = 0; i < MaxSkinInfluence; i++)
                        {
                            var WeightID = (i < Vertex.Weights.Count) ? Vertex.Weights[i].BoneIndex : 0;
                            var WeightValue = (i < Vertex.Weights.Count) ? Vertex.Weights[i].BoneWeight : 0.0f;

                            // Write ID based on count
                            if (BoneCountBuffer <= 0xFF)
                                writeFile.Write((byte)WeightID);
                            else if (BoneCountBuffer <= 0xFFFF)
                                writeFile.Write((ushort)WeightID);
                            else
                                writeFile.Write((uint)WeightID);

                            // Write value
                            writeFile.Write((float)WeightValue);
                        }
                    }

                    // Write faces
                    foreach (var Face in Mesh.Faces)
                    {
                        // Write face indicies based on total vertex count
                        if (VertexCount <= 0xFF)
                        {
                            writeFile.Write((byte)Face.FaceIndex1);
                            writeFile.Write((byte)Face.FaceIndex2);
                            writeFile.Write((byte)Face.FaceIndex3);
                        }
                        else if (VertexCount <= 0xFFFF)
                        {
                            writeFile.Write((ushort)Face.FaceIndex1);
                            writeFile.Write((ushort)Face.FaceIndex2);
                            writeFile.Write((ushort)Face.FaceIndex3);
                        }
                        else
                        {
                            writeFile.Write((uint)Face.FaceIndex1);
                            writeFile.Write((uint)Face.FaceIndex2);
                            writeFile.Write((uint)Face.FaceIndex3);
                        }
                    }

                    // Write material indicies
                    for (int i = 0; i < MatIndiciesCount; i++)
                    {
                        var Index = (i < Mesh.MaterialReferenceIndicies.Count) ? Mesh.MaterialReferenceIndicies[i] : -1;

                        // Write the index, or null for no reference
                        writeFile.Write((int)Index);
                    }
                }

                // Write material data
                foreach (var Mat in Materials)
                {
                    // Write name
                    writeFile.WriteNullTermString(Mat.Name);

                    // Check type
                    if (Mat.MaterialData is SEModelSimpleMaterial)
                    {
                        // Simple material
                        writeFile.Write((bool)true);

                        // Write the image references
                        writeFile.WriteNullTermString(((SEModelSimpleMaterial)Mat.MaterialData).DiffuseMap);
                        writeFile.WriteNullTermString(((SEModelSimpleMaterial)Mat.MaterialData).NormalMap);
                        writeFile.WriteNullTermString(((SEModelSimpleMaterial)Mat.MaterialData).SpecularMap);
                    }
                }
            }
        }

        /// <summary>
        /// Saves the SEModel to a file (Overwriting if exists), following the current specification version, using the provided data
        /// </summary>
        /// <param name="FileName">The file name to save the model to</param>
        public void Write(string FileName)
        {
            // Proxy off
            Write(File.Create(FileName));
        }

        #endregion

        #region Reading

        /// <summary>
        /// Reads a SEAnim from a stream
        /// </summary>
        /// <param name="Stream">The stream to read from</param>
        /// <returns>A SEAnim if successful, otherwise throws an error and returns null</returns>
        public static SEModel Read(Stream Stream)
        {
            // Create a new model
            var model = new SEModel();
            // Setup a new reader
            using (ExtendedBinaryReader readFile = new ExtendedBinaryReader(Stream))
            {
                // Magic
                var Magic = readFile.ReadChars(7);
                // Version
                var Version = readFile.ReadInt16();
                // Header size
                var HeaderSize = readFile.ReadInt16();
                // Check magic
                if (!Magic.SequenceEqual(new char[] { 'S', 'E', 'M', 'o', 'd', 'e', 'l' }))
                {
                    // Bad file
                    throw new Exception("Bad SEModel file, magic was invalid");
                }
                // Data present flags
                var DataPresentFlags = readFile.ReadByte();
                // Bone data present flags
                var BoneDataPresentFlags = readFile.ReadByte();
                // Mesh data present flags
                var MeshDataPresentFlags = readFile.ReadByte();

                // Read counts
                var BoneCount = readFile.ReadInt32();
                var MeshCount = readFile.ReadInt32();
                var MatCount = readFile.ReadInt32();

                // Skip 3 reserved bytes
                readFile.BaseStream.Position += 3;

                // Read bone tag names
                List<string> BoneNames = new List<string>();
                // Loop
                for (int i = 0; i < BoneCount; i++)
                {
                    BoneNames.Add(readFile.ReadNullTermString());
                }

                // Loop and read bones
                for (int i = 0; i < BoneCount; i++)
                {
                    // Read bone flags (unused)
                    var BoneFlags = readFile.ReadByte();

                    // Read bone index
                    var ParentIndex = readFile.ReadInt32();

                    // Check for global matricies
                    Vector3 GlobalPosition = Vector3.Zero;
                    Quaternion GlobalRotation = Quaternion.Identity;
                    // Check
                    if (Convert.ToBoolean(BoneDataPresentFlags & (byte)SEModel_BoneDataPresenceFlags.SEMODEL_PRESENCE_GLOBAL_MATRIX))
                    {
                        GlobalPosition = new Vector3(readFile.ReadSingle(), readFile.ReadSingle(), readFile.ReadSingle());
                        GlobalRotation = new Quaternion(readFile.ReadSingle(), readFile.ReadSingle(), readFile.ReadSingle(), readFile.ReadSingle());
                    }

                    // Check for local matricies
                    Vector3 LocalPosition = Vector3.Zero;
                    Quaternion LocalRotation = Quaternion.Identity;
                    // Check
                    if (Convert.ToBoolean(BoneDataPresentFlags & (byte)SEModel_BoneDataPresenceFlags.SEMODEL_PRESENCE_LOCAL_MATRIX))
                    {
                        LocalPosition = new Vector3(readFile.ReadSingle(), readFile.ReadSingle(), readFile.ReadSingle());
                        LocalRotation = new Quaternion(readFile.ReadSingle(), readFile.ReadSingle(), readFile.ReadSingle(), readFile.ReadSingle());
                    }

                    // Check for scales
                    Vector3 Scale = Vector3.One;
                    // Check
                    if (Convert.ToBoolean(BoneDataPresentFlags & (byte)SEModel_BoneDataPresenceFlags.SEMODEL_PRESENCE_SCALES))
                    {
                        Scale = new Vector3(readFile.ReadSingle(), readFile.ReadSingle(), readFile.ReadSingle());
                    }

                    // Add the bone
                    model.AddBone(BoneNames[i], ParentIndex, GlobalPosition, GlobalRotation, LocalPosition, LocalRotation, Scale);
                }

                // Loop and read meshes
                for (int i = 0; i < MeshCount; i++)
                {
                    // Make a new submesh
                    var mesh = new SEModelMesh();

                    // Read mesh flags (unused)
                    var MeshFlags = readFile.ReadByte();

                    // Read counts
                    var MatIndiciesCount = readFile.ReadByte();
                    var MaxSkinInfluenceCount = readFile.ReadByte();
                    var VertexCount = readFile.ReadInt32();
                    var FaceCount = readFile.ReadInt32();

                    // Loop and read positions
                    for (int v = 0; v < VertexCount; v++)
                        mesh.AddVertex(new SEModelVertex() { Position = new Vector3(readFile.ReadSingle(), readFile.ReadSingle(), readFile.ReadSingle()) });

                    // Read uvlayers
                    if (Convert.ToBoolean(MeshDataPresentFlags & (byte)SEModel_MeshDataPresenceFlags.SEMODEL_PRESENCE_UVSET))
                    {
                        for (int v = 0; v < VertexCount; v++)
                        {
                            for (int l = 0; l < MatIndiciesCount; l++)
                                mesh.Verticies[v].UVSets.Add(new Vector2(readFile.ReadSingle(), readFile.ReadSingle()));
                        }
                    }

                    // Read normals
                    if (Convert.ToBoolean(MeshDataPresentFlags & (byte)SEModel_MeshDataPresenceFlags.SEMODEL_PRESENCE_NORMALS))
                    {
                        // Loop and read vertex normals
                        for (int v = 0; v < VertexCount; v++)
                            mesh.Verticies[v].VertexNormal = new Vector3(readFile.ReadSingle(), readFile.ReadSingle(), readFile.ReadSingle());
                    }

                    // Read colors
                    if (Convert.ToBoolean(MeshDataPresentFlags & (byte)SEModel_MeshDataPresenceFlags.SEMODEL_PRESENCE_COLOR))
                    {
                        // Loop and read colors
                        for (int v = 0; v < VertexCount; v++)
                            mesh.Verticies[v].VertexColor = new Color(readFile.ReadByte(), readFile.ReadByte(), readFile.ReadByte(), readFile.ReadByte());
                    }

                    // Read weights
                    if (Convert.ToBoolean(MeshDataPresentFlags & (byte)SEModel_MeshDataPresenceFlags.SEMODEL_PRESENCE_WEIGHTS))
                    {
                        for (int v = 0; v < VertexCount; v++)
                        {
                            // Read IDs and Values
                            for (int l = 0; l < MaxSkinInfluenceCount; l++)
                            {
                                if (BoneCount <= 0xFF)
                                    mesh.Verticies[v].Weights.Add(new SEModelWeight() { BoneIndex = readFile.ReadByte(), BoneWeight = readFile.ReadSingle() });
                                else if (BoneCount <= 0xFFFF)
                                    mesh.Verticies[v].Weights.Add(new SEModelWeight() { BoneIndex = readFile.ReadUInt16(), BoneWeight = readFile.ReadSingle() });
                                else
                                    mesh.Verticies[v].Weights.Add(new SEModelWeight() { BoneIndex = readFile.ReadUInt32(), BoneWeight = readFile.ReadSingle() });
                            }
                        }
                    }

                    // Loop and read faces
                    for (int f = 0; f < FaceCount; f++)
                    {
                        if (VertexCount <= 0xFF)
                            mesh.AddFace(readFile.ReadByte(), readFile.ReadByte(), readFile.ReadByte());
                        else if (VertexCount <= 0xFFFF)
                            mesh.AddFace(readFile.ReadUInt16(), readFile.ReadUInt16(), readFile.ReadUInt16());
                        else
                            mesh.AddFace(readFile.ReadUInt32(), readFile.ReadUInt32(), readFile.ReadUInt32());
                    }

                    // Read material reference indicies
                    for (int f = 0; f < MatIndiciesCount; f++)
                        mesh.AddMaterialIndex(readFile.ReadInt32());

                    // Add the mesh
                    model.AddMesh(mesh);
                }

                // Loop and read materials
                for (int m = 0; m < MatCount; m++)
                {
                    var mat = new SEModelMaterial();

                    // Read the name
                    mat.Name = readFile.ReadNullTermString();
                    // Read IsSimpleMaterial
                    var IsSimpleMaterial = readFile.ReadBoolean();

                    // Read the material
                    if (IsSimpleMaterial)
                    {
                        mat.MaterialData = new SEModelSimpleMaterial()
                        {
                            DiffuseMap = readFile.ReadNullTermString(),
                            NormalMap = readFile.ReadNullTermString(),
                            SpecularMap = readFile.ReadNullTermString()
                        };
                    }

                    // Add the material
                    model.AddMaterial(mat);
                }
            }
            // Return result
            return model;
        }

        /// <summary>
        /// Reads a SEModel file, following the current specification
        /// </summary>
        /// <param name="FileName">The file name to open</param>
        /// <returns>A SEModel if successful, otherwise throws an error and returns null</returns>
        public static SEModel Read(string FileName)
        {
            // Proxy off
            return Read(File.OpenRead(FileName));
        }

        #endregion

        #region Adding Data

        /// <summary>
        /// Adds a new bone to the model with the given information
        /// </summary>
        /// <param name="Name">The tag name of this bone</param>
        /// <param name="ParentIndex">The parent index of this bone, -1 for a root bone</param>
        /// <param name="GlobalPosition">The global space position of this bone</param>
        /// <param name="GlobalRotation">The global space rotation of this bone</param>
        /// <param name="LocalPosition">The local parent space position of this bone</param>
        /// <param name="LocalRotation">The local parent space rotation of this bone</param>
        /// <param name="Scale">The scale of this bone, 1.0 is default</param>
        public void AddBone(string Name, int ParentIndex, Vector3 GlobalPosition, Quaternion GlobalRotation, Vector3 LocalPosition, Quaternion LocalRotation, Vector3 Scale)
        {
            var Bone = new SEModelBone() { BoneName = Name, BoneParent = ParentIndex };

            // Set matricies
            Bone.GlobalPosition = GlobalPosition;
            Bone.GlobalRotation = GlobalRotation;
            Bone.LocalPosition = LocalPosition;
            Bone.LocalRotation = LocalRotation;

            // Set scale
            Bone.Scale = Scale;

            // Add
            Bones.Add(Bone);
        }

        /// <summary>
        /// Adds the given mesh to the model
        /// </summary>
        /// <param name="Mesh">The mesh to add</param>
        public void AddMesh(SEModelMesh Mesh)
        {
            // Add it
            Meshes.Add(Mesh);
        }

        /// <summary>
        /// Adds the given material to the model
        /// </summary>
        /// <param name="Material">The material to add</param>
        public void AddMaterial(SEModelMaterial Material)
        {
            // Add it
            Materials.Add(Material);
        }

        #endregion
    }
}
