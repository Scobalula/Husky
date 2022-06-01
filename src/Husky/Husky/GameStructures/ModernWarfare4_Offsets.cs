using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Husky
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MW4XAssetPoolData
    {
        // Pool pointer
        public long PoolPtr { get; set; }

        // A pointer to the closest free header
        public long PoolFreeHeadPtr { get; set; }

        // The maximum pool size
        public int PoolSize { get; set; }

        // The beginning of the pool
        public int AssetSize { get; set; }

    };
    /// <summary>
    /// MW4 GfxMap Asset
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MW4GfxMap
    {
        public long NamePointer;
        public long MapNamePointer;
        public fixed byte UnknownValues01[184];
        public int SurfaceCount;
        public int SurfaceDataCount;
        public fixed byte UnknownValues02[48];
        public long SurfacesPointer;
        public fixed byte UnknownValues03[64];
        public long SurfaceDataPointer;
        public fixed byte UnknownValues04[68];
        public int UniqueModelCount;
        public int ModelInstDataCount;
        public fixed byte UnknownValues06[4];
        public int ModelInstCount;
        public fixed byte UnknownValues07[44];
        public long UniqueModelsPtr;
        public long Padding;
        public long ModelInstDataPtr;
        public fixed byte UnknownValues08[248];
        public long ModelInstPtr;
        public fixed byte UnknownValues09[1348];
        public int TrZoneCount;
        public long TrZonePtr;
    }
    /// <summary>
    /// Contains data for each xmodel instances
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MW4ModelInstData
    {
        public int firstInstance;
        public int instanceCount;
        public ushort uniqueModelIndex;
        public fixed byte Padding[6];
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MW4GfxStaticModel
    {
        public long XModelPointer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] Flags;
    }

    /// <summary>
    /// Surface Data - points to surface data in TRZone
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MW4GfxSurfaceData
    {
        public int TransientZone; // Zone Index
        public int LayerCount;
        public int PositionsPosition;
        public int NormalQuatPosition;
        public int Unk01;
        public int ColorsPosition;
        public int UVsPosition;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public uint[] UnkOffsets; // Don't know
    };

    /// <summary>
    /// Modern Warfare 2019 World TRZone
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8, Size = 336)]
    public unsafe struct MW4GfxWorldTRZone
    {
        public long NamePointer;
        public int Index;
        public int Padding;
        public int PositionsBufferSize;
        public int DrawDataBufferSize;
        public long PositionsBufferPointer;
        public long DrawDataBufferPointer;
        public fixed byte UnknownData01[128];
        public int FaceIndicesBufferSize;
        public long FaceIndicesBufferPointer;
        public fixed byte UnknownData02[32];
    }

    /// <summary>
    /// Modern Warfare 2019 BSP Surface Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MW4GfxMapSurface
    {
        public int BufferOffset;
        public int UnknownFloat01;
        public ushort VertexCount;
        public ushort FaceCount;
        public int FaceIndex;
        public long MaterialPointer;
        public int DataIndex;
        public ushort TransientIndex;
        public byte LightmapIndex;
        public long UnknownValue01;
    }

    /// <summary>
    /// Modern Warfare 2019 Static Model Placement
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MW4GfxStaticModelPlacement
    {
        /// <summary>
        /// 12 bytes Packed Position
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] PackedPosition;
        /// <summary>
        /// Quaternion Rotation packed into 4 ushorts
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public ushort[] PackedQuat;
        /// <summary>
        /// Model Scale
        /// </summary>
        public float Scale;
    }

    /// <summary>
    /// Material Asset Info
    /// </summary>
    public unsafe struct MW4Material
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
        public fixed byte UnknownBytes1[34];

        /// <summary>
        /// A pointer to the Tech Set this Material uses
        /// </summary>
        public long TechniqueSetPointer { get; set; }

        /// <summary>
        /// A pointer to this Material's Image table
        /// </summary>
        public long ImageTablePointer { get; set; }

        /// <summary>
        /// Unknown Bytes (Flags, settings, etc.)
        /// </summary>
        public fixed byte UnknownBytes2[40];
    }
}