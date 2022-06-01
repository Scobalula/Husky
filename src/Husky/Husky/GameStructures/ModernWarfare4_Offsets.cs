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
    /// MW4 GfxMap TRZone Asset
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MW4GfxMap
    {
        public long NamePointer; // 0x0
        public long MapNamePointer; // 0x8
        public fixed byte UnknownValues01[184];
        public int SurfaceCount; // 0xC8
        public int SurfaceDataCount; // 0xCC
        public fixed byte UnknownValues02[48];
        public long SurfacesPointer; // 0x100
        public fixed byte UnknownValues03[64];
        public long SurfaceDataPointer;
        public fixed byte UnknownValues04[68];
        public int UniqueModelCount; // 0x194
        public int CollectionCount; // 0x198
        public fixed byte UnknownValues06[4];
        public int StaticModelCount; // 0x1BC
        public fixed byte UnknownValues07[44];
        public long UniqueModelsPtr; // 0x1D0
        public long Padding;
        public long ModelCollectionsPtr;
        public fixed byte UnknownValues08[248];
        public long StaticModelPtr; // 0x2E0
        public fixed byte UnknownValues09[1348];
        public int TrZoneCount;
        public long TrZonePtr;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MW4GfxStaticModelIndices
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
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MW4GfxPixelShader
    {
        public long nameHash;
        public long namePtr;
        public long Flags;
        public long dataPtr;
        public int dataSize;
        public int padding;
    }

    /// <summary>
    ///
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MW4GfxTrZoneBufferInfo
    {
        public int TransientZone; // Zone Index
        public int LayerCount;
        public int PositionsPosition;
        public int NormalQuatPosition;
        public int Unk01; // Not a clue
        public int ColorsPosition; // Maybe Colors?
        public int UVsPosition;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public uint[] UnkOffsets; // Don't know
    };

    /// <summary>
    /// Modern Warfare 2019 BSP Surface Structure
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
    public class MW4GfxStaticModelPlacement
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] PackedPosition;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public ushort[] PackedQuat;
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

        public long ConstantTablePointer { get; set; }

        /// <summary>
        /// Unknown Bytes (Flags, settings, etc.)
        /// </summary>
        public fixed byte UnknownBytes2[32];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MW4GfxImage
    {
        public long NamePtr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] padding;
        public byte ImageFormat;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] padding2;
        public ushort LoadedMipWidth;
        public ushort LoadedMipHeight;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] padding3;
        public byte LoadedMipLevels;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] padding4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public MW4GfxMip[] MipLevels;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] padding5;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MW4GfxMip
    {

        public ulong HashID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public byte[] padding;
        /// <summary>
        /// MipWidth
        /// </summary>
        public uint Size { get; set; }
        /// <summary>
        /// MipWidth
        /// </summary>
        public ushort Width { get; set; }

        /// <summary>
        /// MipHeight
        /// </summary>
        public ushort Height { get; set; }

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MW4XModelLod
    {
        public long MeshPtr;
        public long SurfsPtr;

        public float LodDistance;

        public ushort NumSurfs;
        public ushort SurfacesIndex;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 37)]
        public byte[] Padding;
    };
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MW4XModel
    {
        public long NamePtr;
        public ushort NumSurfaces;
        public byte NumLods;
        public byte MaxLods;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Padding;

        public byte NumBones;
        public byte NumRootBones;
        public ushort UnkBoneCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x80)]
        public byte[] Padding3;

        public long BoneIDsPtr;
        public long ParentListPtr;
        public long RotationsPtr;
        public long TranslationsPtr;
        public long PartClassificationPtr;
        public long BaseMatriciesPtr;
        public long UnknownPtr;
        public long UnknownPtr2;
        public long MaterialHandlesPtr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public MW4XModelLod[] ModelLods;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public byte[] Padding4;
    };
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MW4XModelMesh
    {
        public long NamePtr;
        public long SurfsPtr;
        public ulong LODStreamKey;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x18)]
        public byte[] Padding;

        public long MeshBufferPointer;
        public ushort NumSurfs;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 38)]
        public byte[] Padding2;

    };
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MW4XModelMeshBufferInfo
    {
        public long BufferPtr; // 0 if the model is not loaded, otherwise a pointer, even if streamed
        public int BufferSize;
        public int Streamed;
    };
    [StructLayout(LayoutKind.Sequential)]
    public struct XRigidVertList
    {
        public ushort BoneIndex;
        public ushort VertexCount;
        public ushort FaceCount;
        public ushort FaceIndex;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct UnkSubDivRelatedStruct01
    {
        public ulong UnkSubDivPtr00;
        public unsafe fixed byte Padding[116];
    };
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MW4XModelSurface
    {
        public ushort Flags;
        public ushort VertexCount;
        public ushort FaceCount;
        public ushort UnkCount;
        public byte VertListCount;
        public unsafe fixed byte Padding00[31];
        public int VerticesOffset;
        public int UnkDataOffset;
        public int UVsOffset;
        public int NormalsOffset;
        public int TriOffset;
        public int Unk01Offset;
        public int ColorOffset;
        public int Unk02Offset;
        public int UnkO3Offset;
        public int UnkO4Offset;
        public int Unk05Offset;
        public int Unk06Offset;
        public ulong MeshBuffer;
        public unsafe fixed byte Padding01[64];
        public float XOffset;
        public float YOffset;
        public float ZOffset;
        public float Scale;
        public float Min;
        public float Max;
        public unsafe fixed byte Padding02[24];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MW4GfxStreamVertex
    {
        public ulong PackedPosition; // Packed 21bits, scale + offset in Mesh Info
        public int BiNormal;
        public ushort UVU;
        public ushort UVV;
        public uint NormalQuaternion;
    };

    [StructLayout(LayoutKind.Sequential, Size = 0xA0)]
    public unsafe struct MW4ComPrimaryLight
    {
        public byte spawnFlags;
        public byte type;
        public byte canUseShadowMap;
        public byte needsDynamicShadows;
        public byte isVolumetric;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        public byte[] padding;
        public float intensity;
        public float uvIntensity;
        public float irIntensity;
        public float heatIntensity;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] color;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] dir;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] up;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] origin;
        public float radius;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] fade;
        public float bulbRadius;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] bulbLength;
        public float cosHalfFovOuter;
        public float cosHalfFovInner;
        public float shadowNearPlaneBias;
        public float shadowSoftness;
        public float shadowBias;
        public float shadowArea;
        public float distanceFalloff;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] padding2;
        public long defNamePtr;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MW4CollisionModelList
    {
        public int numModels;
        public int padding;
        public long collisionModelsPtr;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MW4CollisionModel
    {
        public fixed byte Padding[16];
        public long modelCollisionPtr;
        public long numModelInstances;
        public long instanceTransformsPtr;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MW4ColModelPlacement
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] Position; // Quat rotation
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] Rotation;
        public float Scale;
    }
}
