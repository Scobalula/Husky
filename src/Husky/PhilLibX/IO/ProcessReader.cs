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
// File: IO/ProcessReader.cs
// Author: Philip/Scobalula
// Description: A class to help with reading the memory of other processes.
using System;
using System.Diagnostics;

namespace PhilLibX.IO
{
    /// <summary>
    /// A class to help with reading the memory of other processes.
    /// </summary>
    public class ProcessReader
    {
        /// <summary>
        /// Internal Process Property
        /// </summary>
        private Process _Process { get; set; }

        /// <summary>
        /// Internal Handle Property
        /// </summary>
        private IntPtr _Handle { get; set; }

        /// <summary>
        /// Active Process
        /// </summary>
        public Process ActiveProcess
        {
            get { return _Process; }
            set
            {
                _Process = value;
                _Handle = NativeMethods.OpenProcess(MemoryUtil.ProcessVMRead, false, _Process.Id);
            }
        }

        /// <summary>
        /// Active Process Handle
        /// </summary>
        public IntPtr Handle { get { return _Handle; } }

        /// <summary>
        /// Initalizes a Process Reader with a Process
        /// </summary>
        public ProcessReader(Process process)
        {
            ActiveProcess = process;
        }

        /// <summary>
        /// Reads bytes from the Processes Memory
        /// </summary>
        /// <param name="address">The address of the data to be read.</param>
        /// <param name="numBytes">The number of bytes to be read.</param>
        /// <returns>Bytes read</returns>
        public byte[] ReadBytes(long address, int numBytes)
        {
            return MemoryUtil.ReadBytes(Handle, address, numBytes);
        }

        /// <summary>
        /// Reads 64Bit Integer from the Processes Memory
        /// </summary>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public long ReadInt64(long address)
        {
            return MemoryUtil.ReadInt64(Handle, address);
        }

        /// <summary>
        /// Reads an unsigned 64Bit Integer from the Processes Memory
        /// </summary>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public ulong ReadUInt64(long address)
        {
            return MemoryUtil.ReadUInt64(Handle, address);
        }

        /// <summary>
        /// Reads 32Bit Integer from the Processes Memory
        /// </summary>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public int ReadInt32(long address)
        {
            return MemoryUtil.ReadInt32(Handle, address);
        }

        /// <summary>
        /// Reads 32Bit Integer from the Processes Memory
        /// </summary>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public uint ReadUInt32(long address)
        {
            return MemoryUtil.ReadUInt32(Handle, address);
        }

        /// <summary>
        /// Reads a 16Bit Integer from the Processes Memory
        /// </summary>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public short ReadInt16(long address)
        {
            return MemoryUtil.ReadInt16(Handle, address);
        }

        /// <summary>
        /// Reads an unsigned 16Bit Integer from the Processes Memory
        /// </summary>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public ushort ReadUInt16(long address)
        {
            return MemoryUtil.ReadUInt16(Handle, address);
        }

        /// <summary>
        /// Reads a 4 byte single precision floating point number from the Processes Memory
        /// </summary>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public float ReadSingle(long address)
        {
            return MemoryUtil.ReadSingle(Handle, address);
        }

        /// <summary>
        /// Reads an 8 byte double precision floating point number from the Processes Memory
        /// </summary>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public double ReadDouble(long address)
        {
            return MemoryUtil.ReadDouble(Handle, address);
        }

        /// <summary>
        /// Reads a string from the processes' memory terminated by a null byte.
        /// </summary>
        /// <param name="address">Memory Address</param>
        /// <param name="bufferSize">Buffer Read Size</param>
        /// <returns>Resulting String</returns>
        public string ReadNullTerminatedString(long address, int bufferSize = 0xFF)
        {
            return MemoryUtil.ReadNullTerminatedString(Handle, address, bufferSize);
        }

        /// <summary>
        /// Reads a struct from the Processes Memory
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="address">Memory Address</param>
        /// <returns>Resulting Struct</returns>
        public T ReadStruct<T>(long address)
        {
            return MemoryUtil.ReadStruct<T>(Handle, address);
        }

        /// <summary>
        /// Searches for bytes in the Processes Memory
        /// </summary>
        /// <param name="needle">Byte Sequence to scan for.</param>
        /// <param name="startAddress">Address to start the search at.</param>
        /// <param name="endAddress">Address to end the search at.</param>
        /// <param name="firstMatch">If we should stop the search at the first result.</param>
        /// <param name="bufferSize">Byte Buffer Size</param>
        /// <returns>Results<returns>
        public long[] FindBytes(byte?[] needle, long startAddress, long endAddress, bool firstMatch = false, int bufferSize = 0xFFFF)
        {
            return MemoryUtil.FindBytes(Handle, needle, startAddress, endAddress, firstMatch, bufferSize);
        }

        /// <summary>
        /// Gets the Active Processes' Base Address
        /// </summary>
        /// <returns>Base Address of the Active Process</returns>
        public long GetBaseAddress()
        {
            return (long)ActiveProcess?.MainModule.BaseAddress;
        }

        /// <summary>
        /// Gets the size of the Main Module Size
        /// </summary>
        /// <returns>Main Module Size</returns>
        public long GetModuleMemorySize()
        {
            return (long)ActiveProcess?.MainModule.ModuleMemorySize;
        }
    }
}
