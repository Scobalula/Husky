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
    class HalfFloats
    {
        /// <summary>
        /// Shift value
        /// </summary>
        const int Shift = 13;

        /// <summary>
        /// Sign used to shift
        /// </summary>
        const int ShiftSign = 16;

        /// <summary>
        /// Infinity of a float
        /// </summary>
        const int FloatInfinity = 0x7F800000;

        /// <summary>
        /// Maximum value of a half float
        /// </summary>
        const int MaxValue = 0x477FE000;

        /// <summary>
        /// Minimum value of a half float
        /// </summary>
        const int MinValue = 0x38800000;

        /// <summary>
        /// Sign bit of a float
        /// </summary>
        const uint SignBit = 0x80000000;

        /// <summary>
        /// Precalculated properties of a half float
        /// </summary>
        const int InfC = FloatInfinity >> Shift;
        const int NaNN = (InfC + 1) << Shift;
        const int MaxC = MaxValue >> Shift;
        const int MinC = MinValue >> Shift;
        const int SignC = (int)(SignBit >> ShiftSign);

        /// <summary>
        /// Precalculated (1 << 23) / minN
        /// </summary>
        const int MulN = 0x52000000;

        /// <summary>
        /// Precalculated minN / (1 << (23 - shift))
        /// </summary>
        const int MulC = 0x33800000;

        /// <summary>
        /// Max float subnormal down shifted
        /// </summary>
        const int SubC = 0x003FF;

        /// <summary>
        /// Min float normal down shifted
        /// </summary>
        const int NorC = 0x00400;

        /// <summary>
        /// Precalculated min and max decimals
        /// </summary>
        const int MaxD = InfC - MaxC - 1;
        const int MinD = MinC - SubC - 1;

        /// <summary>
        /// A struct to hold different data types in 1 set of bytes
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct FloatBits
        {
            /// <summary>
            /// Floating Point Value
            /// </summary>
            [FieldOffset(0)]
            public float Float;

            /// <summary>
            /// Unsigned Integer Value
            /// </summary>
            [FieldOffset(0)]
            public uint UnsignedInteger;

            /// <summary>
            /// Signed Integer Value
            /// </summary>
            [FieldOffset(0)]
            public int SignedInteger;
        }

        /// <summary>
        /// Converts a half precision floating point number to a single precision floating point number
        /// </summary>
        /// <param name="value">16bit Int representation of the float</param>
        /// <returns>Resulting Float</returns>
        public static float ToFloat(ushort value)
        {
            // Define bit values
            FloatBits v, s = new FloatBits();
            // Set the initial values (if we don't do this, .NET considers them unassigned)
            v.SignedInteger = 0;
            v.UnsignedInteger = 0;
            v.Float = 0;
            s.SignedInteger = 0;
            s.UnsignedInteger = 0;
            s.Float = 0;
            // Assign the initial value given
            v.UnsignedInteger = value;
            // Calculate sign
            int sign = v.SignedInteger & SignC;
            v.SignedInteger ^= sign;
            sign <<= ShiftSign;
            v.SignedInteger ^= ((v.SignedInteger + MinD) ^ v.SignedInteger) & -(v.SignedInteger > SubC ? 1 : 0);
            v.SignedInteger ^= ((v.SignedInteger + MaxD) ^ v.SignedInteger) & -(v.SignedInteger > MaxC ? 1 : 0);
            // Inverse Subnormals
            s.SignedInteger = MulC;
            s.Float *= v.SignedInteger;
            int mask = -(NorC > v.SignedInteger ? 1 : 0);
            v.SignedInteger <<= Shift;
            v.SignedInteger ^= (s.SignedInteger ^ v.SignedInteger) & mask;
            v.SignedInteger |= sign;
            // Return the expanded result
            return v.Float;

        }
    }
}
