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
// File: ByteUtil.cs
// Author: Philip/Scobalula
// Description: Utilities for working with Bytes and Bits
using System.Collections.Generic;
using System.Text;

namespace PhilLibX
{
    /// <summary>
    /// Utilities for working with Bytes and Bits
    /// </summary>
    public class ByteUtil
    {
        /// <summary>
        /// Reads a null terminated string from a byte array
        /// </summary>
        /// <param name="input">Byte Array input</param>
        /// <param name="startIndex">Start Index</param>
        /// <returns>Resulting string</returns>
        public static string ReadNullTerminatedString(byte[] input, int startIndex)
        {
            List<byte> result = new List<byte>();

            for (int i = startIndex; i < input.Length && input[i] != 0; i++)
                result.Add(input[i]);

            return Encoding.ASCII.GetString(result.ToArray());
        }

        /// <summary>
        /// Returns the value of the bit in the given integer
        /// </summary>
        /// <param name="input">Integer Input</param>
        /// <param name="bit">Position</param>
        /// <returns>Result</returns>
        public static byte GetBit(long input, int bit)
        {
            return (byte)((input >> bit) & 1);
        }
    }
}
