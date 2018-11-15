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
// File: Compression/Oodle.cs
// Author: Philip/Scobalula
// Description: Basic Oodle64 Compression Wrapper, currently supports decompression.
// Credits:
//  -   DTZxPorter - SirenLib (for OodleLZ_Decompress parameters)
// Other notes: 
//  -   Tested on Oodle V5 and V6

namespace PhilLibX.Compression
{
    /// <summary>
    /// Oodle Compression Utils
    /// </summary>
    public class Oodle
    {
        /// <summary>
        /// Decompresses a byte array of Oodle Compressed Data (Requires Oodle DLL)
        /// </summary>
        /// <param name="input">Input Compressed Data</param>
        /// <param name="decompressedLength">Decompressed Size</param>
        /// <returns>Resulting Array if success, otherwise null.</returns>
        public static byte[] Decompress(byte[] input, long decompressedLength)
        {
            // Resulting decompressed Data
            byte[] result = new byte[decompressedLength];
            // Decode the data (other parameters such as callbacks not required)
            long decodedSize = NativeMethods.OodleLZ_Decompress(input, input.Length, result, decompressedLength, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
            // Check did we fail
            if (decodedSize == 0) return null;
            // Return Result
            return result;
        }
    }
}
