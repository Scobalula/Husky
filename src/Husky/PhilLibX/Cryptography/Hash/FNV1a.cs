// ------------------------------------------------------------------------
// PhilLibX - My Utility Library
// Copyright(c) 2018 Philip/Scobalula
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ------------------------------------------------------------------------
// File: Cryptograhpy/Hash/FNV1a.cs
// Author: Philip/Scobalula
// Description: Class to handle calculating FNV1a
using System.Text;

namespace PhilLibX.Cryptography.Hash
{
    public class FNV1a
    {
        /// <summary>
        /// Offset Basis for calculating 32bit FNV1a Hashes
        /// </summary>
        public const uint OffsetBasis32 = 0x811C9DC5;

        /// <summary>
        /// Offset Basis for calculating 64bit FNV1a Hashes
        /// </summary>
        public const ulong OffsetBasis64 = 0xCBF29CE484222325;


        /// <summary>
        /// Calculates 32Bit FNV1a Hash for a given string
        /// </summary>
        /// <param name="value">String to generate a hash from</param>
        /// <param name="initial">Initial Hash Value (Defaults to OffsetBasis32))</param>
        /// <returns>Resulting Unsigned Hash Value</returns>
        public static uint Calculate32(string value, uint initial = OffsetBasis32)
        {
            return Calculate32(Encoding.ASCII.GetBytes(value), initial);
        }

        /// <summary>
        /// Calculates 32Bit FNV1a Hash for a given string
        /// </summary>
        /// <param name="value">String to generate a hash from</param>
        /// <param name="initial">Initial Hash Value (Defaults to OffsetBasis32))</param>
        /// <returns>Resulting Unsigned Hash Value</returns>
        public static ulong Calculate64(string value, ulong initial = OffsetBasis64)
        {
            return Calculate64(Encoding.ASCII.GetBytes(value), initial);
        }

        /// <summary>
        /// Calculates 32Bit FNV1a Hash for a sequence of bytes.
        /// </summary>
        /// <param name="value">Bytes to generate a hash from</param>
        /// <param name="initial">Initial Hash Value (Defaults to OffsetBasis32))</param>
        /// <returns>Resulting Unsigned Hash Value</returns>
        public static uint Calculate32(byte[] value, uint initial = 0)
        {
            // Set Initial Value
            uint hash = initial;

            // Loop Bytes
            for (int i = 0; i < value.Length; i++)
            {
                hash ^= value[i];
                hash *= 0x1000193;
            }

            // Return
            return hash;
        }

        /// <summary>
        /// Calculates 64Bit FNV1a Hash for a sequence of bytes.
        /// </summary>
        /// <param name="value">Bytes to generate a hash from</param>
        /// <param name="initial">Initial Hash Value (Defaults to OffsetBasis64))</param>
        /// <returns>Resulting Unsigned Hash Value</returns>
        public static ulong Calculate64(byte[] value, ulong initial = OffsetBasis64)
        {
            // Set Initial Value
            ulong hash = initial;

            // Loop Bytes
            for (int i = 0; i < value.Length; i++)
            {
                hash ^= value[i];
                hash *= 0x100000001B3;
            }

            // Return
            return hash;
        }
    }
}
