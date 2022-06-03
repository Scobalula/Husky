using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Husky
{
    /// <summary>
    /// Vanguard GfxWorldTRZone
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8, Size = 136)]
    public unsafe struct VGGfxWorldTRZone
    {
        public long NamePointer;
        public long UnknownPtr;
        public int Index;
        public int Unknown;
        public int VertexBufferSize; // 0x18
        public int IndexCount; // 0x1C
        public int UnknownCount2; // 0x20
        public int UnknownValue; // Don't know
        public long VertexBufferPointer; // 0x28
        public long IndicesBufferPointer; // 0x30 -- size is IndexCount * 2
        public long UnknownBuffer2; // 0x38 -- size is UnknownCount2 * 28
        public long Unknown2; // Don't know
        public long UnknownBuffer3; // 0x48
        public long UnknownSize4; // 0x50
        public long UnknownBuffer4; // 0x58 -- size is UnknownSize4 << 6
        public fixed byte Padding[32];
    }
    /// <summary>
    /// Vanguard GfxWorld
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct VGGfxMap
    {
        public long NamePointer; // 0x0
        public long MapNamePointer; // 0x8
        public fixed byte Unknown01[120];
        public int surfaceCount; // 0x88
        public int surfaceDataCount; // 0x8C
        public int materialCount; // 0x90
        public fixed byte Unknown02[76];
        public long surfacesPtr; // 0xE0
        public fixed byte Unknown03[72];
        public long surfaceBoundsPtr; // 0x130 // size is surfaceCount * 6
        public fixed byte Unknown04[64];
        public long materialsPtr; // 0x178 -- size is materialCount * 8
        public long unknownPtr1; // 0x180 -- size is materialCount * 4
        public long surfaceDataPtr; // 0x188 -- size is surfaceDataCount * 128 / surfaceDataCount << 7
        public long surfaceOffsetsPtr; // 0x190 -- size is surfaceDataCount * 16 (same as first 16 bytes of surfaceData)
        public long unknownPtr4; // 0x198 -- size is surfaceDataCount << 7
        public fixed byte Unknown05[420];
        public int UniqueModelCount; // 0x344
        public int ModelInstDataCount; // 0x348
        public int UnknownValue; // 0x34C
        public int ModelInstCount; // 0x350
        public fixed byte Unknown06[84];
        public long UniqueModelsPtr; // 0x3A8
        public long Padding;
        public long ModelInstDataPtr; // 0x3B8
        public fixed byte Unknown07[248];
        public long ModelInstPtr; // 0x4B8
        public fixed byte Unknown08[328];
        public long UnknownModelPtr; // 0x608 -- size is uniqueModelCount * 72
    }

    /// <summary>
    /// Vanguard Surface Data Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VGGfxSurfaceData
    {
        public float Scale;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] Offsets;
        public int ZoneIndex; // Transient Index probably here?
        public int Unknown01;
        public int Unknown02;
        public int LayerCount;
        /* VERTEX OFFSETS
         * 1 - Vertex Positions
         * 2 - Normals
         * 4 - Colors
         * 5 - UVs */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public int[] VertexOffsets;
        public float UnknownValue;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public int[] Unknown03;
    }

    /// <summary>
    /// Vanguard Surface Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VGGfxMapSurface
    {
        public int BufferOffset;
        public int FaceIndex;
        public int UnknownValue01;
        public ushort FaceCount;
        public ushort VertexCount;
        public int DataIndex;
        public int MaterialIndex;
        public int UnknownValue02;
        public int UnknownValue03;
    }

    /// <summary>
    /// Vanguard Material Asset
    /// </summary>
    public unsafe struct VGMaterial
    {
        /// <summary>
        /// A pointer to the name of this material
        /// </summary>
        public long NamePointer { get; set; }

        /// <summary>
        /// Unknown Bytes (Flags, settings, etc.)
        /// </summary>
        public fixed byte UnknownBytes[20];

        /// <summary>
        /// Number of Images this Material has
        /// </summary>
        public byte ImageCount { get; set; }

        public byte ConstantCount { get; set; }

        /// <summary>
        /// Unknown Bytes (Flags, settings, etc.)
        /// </summary>
        public fixed byte UnknownBytes1[18];

        /// <summary>
        /// A pointer to the Tech Set this Material uses
        /// </summary>
        public long TechniqueSetPointer { get; set; }

        /// <summary>
        /// A pointer to this Material's Image table
        /// </summary>
        public long ImageTablePointer { get; set; }

        public long Padding { get; set; } // Constant Table?

        /// <summary>
        /// Unknown Bytes (Flags, settings, etc.)
        /// </summary>
        public fixed byte UnknownBytes2[56];
    }
}