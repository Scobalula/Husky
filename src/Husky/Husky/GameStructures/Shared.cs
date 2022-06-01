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
using System.Runtime.InteropServices;

namespace Husky
{
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
        public PackedUnitVector Normal { get; set; }

        /// <summary>
        /// Unknown Bytes (2 shorts?)
        /// </summary>
        public int Padding2 { get; set; }
    }

    /// <summary>
    /// Packed Unit Vector as 4 bytes
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct PackedUnitVector
    {
        /// <summary>
        /// Packed Value
        /// </summary>
        [FieldOffset(0)]
        public int Value;

        /// <summary>
        /// First Byte
        /// </summary>
        [FieldOffset(0)]
        public byte Byte1;

        /// <summary>
        /// Second Byte
        /// </summary>
        [FieldOffset(1)]
        public byte Byte2;

        /// <summary>
        /// Third Byte
        /// </summary>
        [FieldOffset(2)]
        public byte Byte3;

        /// <summary>
        /// Fourth Byte
        /// </summary>
        [FieldOffset(3)]
        public byte Byte4;
    }

    /// <summary>
    /// Material Image for: WaW
    /// </summary>
    public unsafe struct MaterialImage32A
    {
        /// <summary>
        /// Semantic Hash/Usage
        /// </summary>
        public uint SemanticHash { get; set; }

        /// <summary>
        /// Unknown Int
        /// </summary>
        public uint UnknownInt { get; set; }

        /// <summary>
        /// Null Padding
        /// </summary>
        public int Padding { get; set; }

        /// <summary>
        /// Pointer to the Image Asset
        /// </summary>
        public int ImagePointer { get; set; }
    }

    /// <summary>
    /// Material Image for: MW2, MW3
    /// </summary>
    public unsafe struct MaterialImage32B
    {
        /// <summary>
        /// Semantic Hash/Usage
        /// </summary>
        public uint SemanticHash { get; set; }

        /// <summary>
        /// Unknown Int
        /// </summary>
        public uint UnknownInt { get; set; }

        /// <summary>
        /// Pointer to the Image Asset
        /// </summary>
        public int ImagePointer { get; set; }
    }

    /// <summary>
    /// Material Image for: Ghosts, AW, MWR
    /// </summary>
    public unsafe struct MaterialImage64A
    {
        /// <summary>
        /// Semantic Hash/Usage
        /// </summary>
        public uint SemanticHash { get; set; }

        /// <summary>
        /// Unknown Int (It's possible the semantic hash is actually 64bit, and this is apart of the actual hash)
        /// </summary>
        public uint UnknownInt { get; set; }

        /// <summary>
        /// Pointer to the Image Asset
        /// </summary>
        public long ImagePointer { get; set; }
    }

    /// <summary>
    /// Bo3 Material Image Struct
    /// </summary>
    public unsafe struct MaterialImage64B
    {
        /// <summary>
        /// A pointer to the image asset
        /// </summary>
        public long ImagePointer { get; set; }

        /// <summary>
        /// Semantic Hash (i.e. colorMap, colorMap00, etc.) Varies from MTL type, base ones like colorMap are always the same
        /// </summary>
        public uint SemanticHash { get; set; }

        /// <summary>
        /// End Bytes (Usage, etc.)
        /// </summary>
        public fixed byte Padding[0x14];
    }

    /// <summary>
    /// 3Float Vertex Position
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxVertexPosition
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3 ToCentimeter()
        {
            return new Vector3(X * 2.54, Y * 2.54, Z * 2.54);
        }
    }

    /// <summary>
    /// 2Float Vertex UVs
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GfxVertexUV
    {
        /// <summary>
        /// U Texture Position
        /// </summary>
        public float U { get; set; }

        /// <summary>
        /// V Texture Position
        /// </summary>
        public float V { get; set; }
    }

    /// <summary>
    /// Parasytes XAsset Structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ParasyteXAsset64
    {
        /// <summary>
        /// The xasset type depending on the game it is from.
        /// </summary>
        public int Type;

        /// <summary>
        /// The xasset size.
        /// </summary>
        public int HeaderSize;

        /// <summary>
        /// The xasset ID.
        /// </summary>
        public long ID;

        /// <summary>
        /// The xasset Name.
        /// </summary>
        public long Name;

        /// <summary>
        /// Whether or not this asset is a tempt slot.
        /// </summary>
        public int Temp;

        /// <summary>
        /// Header Memory.
        /// </summary>
        public long HeaderMemory;

        /// <summary>
        /// The fast file that owns this asset.
        /// </summary>
        public long Owner;

        /// <summary>
        /// The previous xasset in the list.
        /// </summary>
        public long Previous;

        /// <summary>
        /// The next xasset in the list.
        /// </summary>
        public long Next;

        /// <summary>
        /// First child asset we have overriden.
        /// </summary>
        public long FirstChild;

        /// <summary>
        /// Last child asset we have overriden.
        /// </summary>
        public long LastChild;

        /// <summary>
        /// The asset header.
        /// </summary>
        public long Header;
        /// <summary>
        /// The size of the extended data.
        /// </summary>
        public long ExtendedDataSize;

        /// <summary>
        /// The extended data, if any.
        /// </summary>
        public long ExtendedData;

        /// <summary>
        /// The pointer that points to the extended data.
        /// </summary>
        public long ExtendedDataPtrOffset;
    };

    /// <summary>
    /// Parasytes XAsset Pool Structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ParasyteXAssetPool64
    {
        /// <summary>
        /// The start of the asset chain.
        /// </summary>
        public long FirstXAsset;

        /// <summary>
        /// The end of the asset chain.
        /// </summary>
        public long LastXAsset;

        /// <summary>
        /// The asset hash table for this pool.
        /// </summary>
        public long LookupTable;

        /// <summary>
        /// Storage for asset headers for this pool.
        /// </summary>
        public long HeaderMemory;
    };
}
