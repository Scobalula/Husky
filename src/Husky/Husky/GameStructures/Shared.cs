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
}
