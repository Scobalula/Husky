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
// File: Cryptograhpy/Hash/SDBM.cs
// Author: Philip/Scobalula
// Description: Class to handle calculating SDBM Hash
using System.Text;

namespace PhilLibX.Cryptography.Hash
{
    /// <summary>
    /// Class to handle calculating SDBM Hash
    /// </summary>
    public class SDBM
    {
        /// <summary>
        /// Calculates SDBM Hash for the given string.
        /// </summary>
        /// <param name="value">String to generate a hash from</param>
        /// <param name="initial">Initial Hash Value (0)</param>
        /// <returns>Resulting Unsigned Hash Value</returns>
        public static uint Calculate(string value, uint initial = 0)
        {
            return Calculate(Encoding.ASCII.GetBytes(value), initial);
        }

        /// <summary>
        /// Calculates SDBM Hash for a sequence of bytes.
        /// </summary>
        /// <param name="value">Bytes to generate a hash from</param>
        /// <param name="initial">Initial Hash Value (Defaults to 0))</param>
        /// <returns>Resulting Unsigned Hash Value</returns>
        public static uint Calculate(byte[] value, uint initial = 0)
        {
            // Set INitial Value
            uint hash = initial;

            // Loop Bytes 
            for (int i = 0; i < value.Length; i++)
                hash = value[i] + (hash << 6) + (hash << 16) - hash;

            // Return
            return hash;
        }
    }
}
