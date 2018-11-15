using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Husky
{
    /// <summary>
    /// WaW GfxMap Asset (some pointers we skip over point to DirectX routines, etc. if that means anything to anyone)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxMapWaW
    {
        /// <summary>
        /// A pointer to the name of this GfxMap Asset
        /// </summary>
        public int NamePointer { get; set; }

        /// <summary>
        /// A pointer to the name of the map 
        /// </summary>
        public int MapNamePointer { get; set; }

        /// <summary>
        /// Unknown Bytes (Possibly counts for other data we don't care about)
        /// </summary>
        public fixed byte Padding[8];

        /// <summary>
        /// Number of Gfx Indices (for Faces)
        /// </summary>
        public int GfxIndicesCount { get; set; }

        /// <summary>
        /// Pointer to the Gfx Index Data
        /// </summary>
        public int GfxIndicesPointer { get; set; }

        /// <summary>
        /// Number of Surfaces
        /// </summary>
        public int SurfaceCount { get; set; }

        /// <summary>
        /// Unknown Bytes (Possibly counts, pointers, etc. for other data we don't care about)
        /// </summary>
        public fixed byte Padding1[0x18];

        /// <summary>
        /// Number of Gfx Vertices (XYZ, etc.)
        /// </summary>
        public int GfxVertexCount { get; set; }

        /// <summary>
        /// Pointer to the Gfx Vertex Data
        /// </summary>
        public int GfxVerticesPointer { get; set; }

        /// <summary>
        /// Unknown Bytes (more BSP data we probably don't care for)
        /// </summary>
        public fixed byte Padding2[0x26C];

        /// <summary>
        /// Pointer to the Gfx Index Data
        /// </summary>
        public int GfxSurfacesPointer { get; set; }
    }

    /// <summary>
    /// Gfx Map Surface
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxSurfaceWaW
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
        /// Base Face Index (this is what allows the GfxMap to have 65k+ faces with only 2 byte indices)
        /// </summary>
        public int FaceIndex { get; set; }

        /// <summary>
        /// Always 0xFFFFFFFF? 
        /// </summary>
        public int Padding1 { get; set; }

        /// <summary>
        /// Pointer to the Material Asset of this Surface
        /// </summary>
        public int MaterialPointer { get; set; }

        /// <summary>
        /// Unknown Bytes
        /// </summary>
        public fixed byte Padding[0x1C];
    }

    /// <summary>
    /// MW2 GfxMap Asset (some pointers we skip over point to DirectX routines, etc. if that means anything to anyone)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxMapMW2
    {
        /// <summary>
        /// A pointer to the name of this GfxMap Asset
        /// </summary>
        public int NamePointer { get; set; }

        /// <summary>
        /// A pointer to the name of the map 
        /// </summary>
        public int MapNamePointer { get; set; }

        /// <summary>
        /// Unknown Bytes (Possibly counts for other data we don't care about)
        /// </summary>
        public fixed byte Padding[8];

        /// <summary>
        /// Number of Surfaces
        /// </summary>
        public int SurfaceCount { get; set; }

        /// <summary>
        /// Unknown Bytes (Possibly counts, pointers, etc. for other data we don't care about)
        /// </summary>
        public fixed byte Padding1[0x64];

        /// <summary>
        /// Number of Gfx Vertices (XYZ, etc.)
        /// </summary>
        public int GfxVertexCount { get; set; }

        /// <summary>
        /// Pointer to the Gfx Vertex Data
        /// </summary>
        public int GfxVerticesPointer { get; set; }

        /// <summary>
        /// Unknown Bytes (more BSP data we probably don't care for)
        /// </summary>
        public fixed byte Padding2[0x10];

        /// <summary>
        /// Number of Gfx Indices (for Faces)
        /// </summary>
        public int GfxIndicesCount { get; set; }

        /// <summary>
        /// Pointer to the Gfx Index Data
        /// </summary>
        public int GfxIndicesPointer { get; set; }

        /// <summary>
        /// Unknown Bytes (more BSP data we probably don't care for)
        /// </summary>
        public fixed byte Padding3[0x184];

        /// <summary>
        /// Pointer to the Gfx Index Data
        /// </summary>
        public int GfxSurfacesPointer { get; set; }
    }

    /// <summary>
    /// MW3 GfxMap Asset (some pointers we skip over point to DirectX routines, etc. if that means anything to anyone)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxMapMW3
    {
        /// <summary>
        /// A pointer to the name of this GfxMap Asset
        /// </summary>
        public int NamePointer { get; set; }

        /// <summary>
        /// A pointer to the name of the map 
        /// </summary>
        public int MapNamePointer { get; set; }

        /// <summary>
        /// Unknown Bytes (Possibly counts for other data we don't care about)
        /// </summary>
        public fixed byte Padding[8];
        
        /// <summary>
        /// Number of Surfaces
        /// </summary>
        public int SurfaceCount { get; set; }

        /// <summary>
        /// Unknown Bytes (Possibly counts, pointers, etc. for other data we don't care about)
        /// </summary>
        public fixed byte Padding1[0x70];

        /// <summary>
        /// Number of Gfx Vertices (XYZ, etc.)
        /// </summary>
        public int GfxVertexCount { get; set; }

        /// <summary>
        /// Pointer to the Gfx Vertex Data
        /// </summary>
        public int GfxVerticesPointer { get; set; }

        /// <summary>
        /// Unknown Bytes (more BSP data we probably don't care for)
        /// </summary>
        public fixed byte Padding2[0x10];

        /// <summary>
        /// Number of Gfx Indices (for Faces)
        /// </summary>
        public int GfxIndicesCount { get; set; }

        /// <summary>
        /// Pointer to the Gfx Index Data
        /// </summary>
        public int GfxIndicesPointer { get; set; }

        /// <summary>
        /// Unknown Bytes (more BSP data we probably don't care for)
        /// </summary>
        public fixed byte Padding3[0x184];

        /// <summary>
        /// Pointer to the Gfx Index Data
        /// </summary>
        public int GfxSurfacesPointer { get; set; }
    }

    /// <summary>
    /// Gfx Map Surface
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxSurfaceMW3
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
        /// Base Face Index (this is what allows the GfxMap to have 65k+ faces with only 2 byte indices)
        /// </summary>
        public int FaceIndex { get; set; }

        /// <summary>
        /// Pointer to the Material Asset of this Surface
        /// </summary>
        public int MaterialPointer { get; set; }

        /// <summary>
        /// Unknown Bytes
        /// </summary>
        public fixed byte Padding[4];
    }

    /// <summary>
    /// MWR Map Ents Asset
    /// </summary>
    public unsafe struct MapEnts
    {
        /// <summary>
        /// A pointer to the name of this MapEnts Asset
        /// </summary>
        public long NamePointer { get; set; }

        /// <summary>
        /// A pointer to the entity map string
        /// </summary>
        public long EntityMapStringPointer { get; set; }

        /// <summary>
        /// Size of the entity map string
        /// </summary>
        public int EntityMapStringSize { get; set; }
    }

    /// <summary>
    /// Gfx Map Surface Ghosts
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxSurfaceGhosts
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
        /// Unknown Bytes (float?)
        /// </summary>
        public fixed byte Padding[4];

        /// <summary>
        /// Number of Vertices this surface has
        /// </summary>
        public ushort VertexCount { get; set; }

        /// <summary>
        /// Number of Faces this surface has
        /// </summary>
        public ushort FaceCount { get; set; }

        /// <summary>
        /// Base Face Index (this is what allows the GfxMap to have 65k+ faces with only 2 byte indices)
        /// </summary>
        public int FaceIndex { get; set; }

        /// <summary>
        /// Null Padding
        /// </summary>
        public int Padding1 { get; set; }

        /// <summary>
        /// Pointer to the Material Asset of this Surface
        /// </summary>
        public long MaterialPointer { get; set; }

        /// <summary>
        /// Unknown Bytes 
        /// </summary>
        public fixed byte Padding2[8];
    }

    /// <summary>
    /// Gfx Map Surface
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxSurfaceMWR
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
        /// Unknown Bytes (Possibly color? And vertex count, along with some float that might be size)
        /// </summary>
        public fixed byte Padding[0xA];

        /// <summary>
        /// Number of Faces this surface has
        /// </summary>
        public ushort FaceCount { get; set; }

        /// <summary>
        /// Base Face Index (this is what allows the GfxMap to have 65k+ faces with only 2 byte indices)
        /// </summary>
        public int FaceIndex { get; set; }

        /// <summary>
        /// Pointer to the Material Asset of this Surface
        /// </summary>
        public long MaterialPointer { get; set; }

        /// <summary>
        /// Unknown Bytes 
        /// </summary>
        public fixed byte Padding2[8];
    }

    /// <summary>
    /// Gfx Color
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxColor
    {
        /// <summary>
        /// Red Value
        /// </summary>
        public byte R { get; set; }

        /// <summary>
        /// Red Value
        /// </summary>
        public byte G { get; set; }

        /// <summary>
        /// Red Value
        /// </summary>
        public byte B { get; set; }

        /// <summary>
        /// Red Value
        /// </summary>
        public byte A { get; set; }
    }

    /// <summary>
    /// Gfx Vertex 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxVertex
    {
        /// <summary>
        /// X Position
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y Position
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Z Position
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// Bi Normal
        /// </summary>
        public float BiNormal { get; set; }

        /// <summary>
        /// RGBA Color
        /// </summary>
        public GfxColor Color { get; set; }

        /// <summary>
        /// U Texture Position
        /// </summary>
        public float U { get; set; }

        /// <summary>
        /// V Texture Position
        /// </summary>
        public float V { get; set; }

        /// <summary>
        /// Unknown Bytes (Possibly tangent, etc.)
        /// </summary>
        public long Padding { get; set; }

        /// <summary>
        /// Packed Vertex Normal (same as XModels)
        /// </summary>
        public int Normal { get; set; }

        /// <summary>
        /// Unknown Bytes (2 shorts?)
        /// </summary>
        public int Padding2 { get; set; }
    }

    /// <summary>
    /// Ghosts Gfx Map Asset (some pointers we skip over point to DirectX routines, etc. if that means anything to anyone)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxMapGhosts
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
        public fixed byte Padding1[0xD4];

        /// <summary>
        /// Number of Gfx Vertices (XYZ, etc.)
        /// </summary>
        public int GfxVertexCount { get; set; }

        /// <summary>
        /// Pointer to the Gfx Vertex Data
        /// </summary>
        public long GfxVerticesPointer { get; set; }

        /// <summary>
        /// Unknown Bytes (more BSP data we probably don't care for)
        /// </summary>
        public fixed byte Padding2[0x20];

        /// <summary>
        /// Number of Gfx Indices (for Faces)
        /// </summary>
        public long GfxIndicesCount { get; set; }

        /// <summary>
        /// Pointer to the Gfx Index Data
        /// </summary>
        public long GfxIndicesPointer { get; set; }

        /// <summary>
        /// Pointers, etc.
        /// </summary>
        public fixed byte Padding3[0x838];

        /// <summary>
        /// Pointer to the Gfx Index Data
        /// </summary>
        public long GfxSurfacesPointer { get; set; }
    }

    /// <summary>
    /// MWR GfxMap Asset (some pointers we skip over point to DirectX routines, etc. if that means anything to anyone)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxMapAW
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
        public fixed byte Padding1[0x110];

        /// <summary>
        /// Number of Gfx Vertices (XYZ, etc.)
        /// </summary>
        public long GfxVertexCount { get; set; }

        /// <summary>
        /// Pointer to the Gfx Vertex Data
        /// </summary>
        public long GfxVerticesPointer { get; set; }

        /// <summary>
        /// Unknown Bytes (more BSP data we probably don't care for)
        /// </summary>
        public fixed byte Padding2[0x20];

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
        public fixed byte Padding3[0x830];

        /// <summary>
        /// Pointer to the Gfx Index Data
        /// </summary>
        public long GfxSurfacesPointer { get; set; }
    }

    /// <summary>
    /// Gfx Map Surface
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxSurfaceAW
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
        /// Unknown Bytes
        /// </summary>
        public int Padding1 { get; set; }

        /// <summary>
        /// Number of Vertices this surface has
        /// </summary>
        public ushort VertexCount { get; set; }

        /// <summary>
        /// Number of Faces this surface has
        /// </summary>
        public ushort FaceCount { get; set; }

        /// <summary>
        /// Base Face Index (this is what allows the GfxMap to have 65k+ faces with only 2 byte indices)
        /// </summary>
        public int FaceIndex { get; set; }

        /// <summary>
        /// Unknown Bytes
        /// </summary>
        public int Padding2 { get; set; }

        /// <summary>
        /// Pointer to the Material Asset of this Surface
        /// </summary>
        public long MaterialPointer { get; set; }

        /// <summary>
        /// Unknown Bytes
        /// </summary>
        public fixed byte Padding3[8];
    }

    /// <summary>
    /// MWR GfxMap Asset (some pointers we skip over point to DirectX routines, etc. if that means anything to anyone)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxMapMWR
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
        public fixed byte Padding1[0x110];

        /// <summary>
        /// Number of Gfx Vertices (XYZ, etc.)
        /// </summary>
        public long GfxVertexCount { get; set; }

        /// <summary>
        /// Pointer to the Gfx Vertex Data
        /// </summary>
        public long GfxVerticesPointer { get; set; }

        /// <summary>
        /// Unknown Bytes (more BSP data we probably don't care for)
        /// </summary>
        public fixed byte Padding2[0x30];

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
        public fixed byte Padding3[0x5C8];

        /// <summary>
        /// Number of Static Models
        /// </summary>
        public long StaticModelsCount { get; set; }

        /// <summary>
        /// Unknown Bytes (more BSP data we probably don't care for)
        /// </summary>
        public fixed byte Padding4[0x290];

        /// <summary>
        /// Pointer to the Gfx Index Data
        /// </summary>
        public long GfxSurfacesPointer { get; set; }
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
        /// Rotation (TODO: Look into it)
        /// </summary>
        public fixed byte UnknownBytes1[36];

        /// <summary>
        /// Model Scale 
        /// </summary>
        public float ModelScale { get; set; }

        /// <summary>
        /// Null Padding
        /// </summary>
        public int Padding { get; set; }

        /// <summary>
        /// Pointer to the XModel Asset
        /// </summary>
        public long ModelPointer { get; set; }

        /// <summary>
        /// Unknown Bytes
        /// </summary>
        public fixed byte UnknownBytes2[0x10];
    }
}
