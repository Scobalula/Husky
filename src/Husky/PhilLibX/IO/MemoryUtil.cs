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
// File: IO/MemoryUtil.cs
// Author: Philip/Scobalula
// Description: Utilities for Reading Process Memory
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PhilLibX.IO
{
    /// <summary>
    /// Utilities for Reading Process Memory
    /// </summary>
    public class MemoryUtil
    {
        /// <summary>
        /// Required to read memory in a process using ReadProcessMemory.
        /// </summary>
        public const int ProcessVMRead = 0x0010;

        /// <summary>
        /// Required to write to memory in a process using WriteProcessMemory.
        /// </summary>
        public const int ProcessVMWrite = 0x0020;

        /// <summary>
        /// Required to perform an operation on the address space of a process
        /// </summary>
        public const int ProcessVMOperation = 0x0008;

        /// <summary>
        /// Reads bytes from a Processes Memory and returns a byte array of read data.
        /// </summary>
        /// <param name="processHandle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="address">The address of the data to be read.</param>
        /// <param name="numBytes">The number of bytes to be read from the specified process.</param>
        /// <returns>Bytes read</returns>
        public static byte[] ReadBytes(IntPtr processHandle, long address, int numBytes)
        {
            // Resulting buffer
            byte[] buffer = new byte[numBytes];
            // Request ReadProcessMemory
            NativeMethods.ReadProcessMemory((int)processHandle, address, buffer, buffer.Length, out int bytesRead);
            // Return result
            return buffer;
        }

        /// <summary>
        /// Reads a 64Bit Integer from a Processes Memory
        /// </summary>
        /// <param name="processHandle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public static long ReadInt64(IntPtr processHandle, long address)
        {
            return BitConverter.ToInt64(ReadBytes(processHandle, address, 8), 0);
        }

        /// <summary>
        /// Reads an unsigned 64Bit Integer from a Processes Memory
        /// </summary>
        /// <param name="processHandle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public static ulong ReadUInt64(IntPtr processHandle, long address)
        {
            return BitConverter.ToUInt64(ReadBytes(processHandle, address, 8), 0);
        }

        /// <summary>
        /// Reads a 32Bit Integer from a Processes Memory
        /// </summary>
        /// <param name="processHandle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public static int ReadInt32(IntPtr processHandle, long address)
        {
            return BitConverter.ToInt32(ReadBytes(processHandle, address, 4), 0);
        }

        /// <summary>
        /// Reads an unsigned 32Bit Integer from a Processes Memory
        /// </summary>
        /// <param name="processHandle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public static uint ReadUInt32(IntPtr processHandle, long address)
        {
            return BitConverter.ToUInt32(ReadBytes(processHandle, address, 4), 0);
        }

        /// <summary>
        /// Reads a 16Bit Integer from a Processes Memory
        /// </summary>
        /// <param name="processHandle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public static short ReadInt16(IntPtr processHandle, long address)
        {
            return BitConverter.ToInt16(ReadBytes(processHandle, address, 4), 0);
        }

        /// <summary>
        /// Reads an unsigned 16Bit Integer from a Processes Memory
        /// </summary>
        /// <param name="processHandle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public static ushort ReadUInt16(IntPtr processHandle, long address)
        {
            return BitConverter.ToUInt16(ReadBytes(processHandle, address, 2), 0);
        }

        /// <summary>
        /// Reads a 4 byte single precision floating point number from a Processes Memory
        /// </summary>
        /// <param name="processHandle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public static float ReadSingle(IntPtr processHandle, long address)
        {
            return BitConverter.ToSingle(ReadBytes(processHandle, address, 2), 0);
        }

        /// <summary>
        /// Reads an 8 byte double precision floating point number from a Processes Memory
        /// </summary>
        /// <param name="processHandle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="address">The address of the data to be read.</param>
        /// <returns>Resulting Data</returns>
        public static double ReadDouble(IntPtr processHandle, long address)
        {
            return BitConverter.ToDouble(ReadBytes(processHandle, address, 8), 0);
        }

        /// <summary>
        /// Reads a string from a processes' memory terminated by a null byte.
        /// </summary>
        /// <param name="processHandle">Process Handle Pointer</param>
        /// <param name="address">Memory Address</param>
        /// <param name="bufferSize">Buffer Read Size</param>
        /// <returns>Resulting String</returns>
        public static string ReadNullTerminatedString(IntPtr processHandle, long address, int bufferSize = 0xFF)
        {
            List<byte> result = new List<byte>();

            byte[] buffer = ReadBytes(processHandle, address, bufferSize);

            while (true)
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] == 0x0)
                        return Encoding.ASCII.GetString(result.ToArray());

                    result.Add(buffer[i]);
                }

                buffer = ReadBytes(processHandle, address += bufferSize, bufferSize);
            }
        }

        /// <summary>
        /// Reads a struct from a Processes Memory
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="processHandle">Process Handle Pointer</param>
        /// <param name="address">Memory Address</param>
        /// <returns>Resulting Struct</returns>
        public static T ReadStruct<T>(IntPtr processHandle, long address)
        {
            byte[] data = ReadBytes(processHandle, address, Marshal.SizeOf<T>());
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return theStructure;
        }

        /// <summary>
        /// Searches for bytes in a processes memory.
        /// </summary>
        /// <param name="processHandle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="needle">Byte Sequence to scan for.</param>
        /// <param name="startAddress">Address to start the search at.</param>
        /// <param name="endAddress">Address to end the search at.</param>
        /// <param name="bufferSize">Byte Buffer Size</param>
        /// <param name="firstMatch">If we should stop the search at the first result.</param>
        /// <returns></returns>
        public static long[] FindBytes(IntPtr processHandle, byte?[] needle, long startAddress, long endAddress, bool firstMatch = false, int bufferSize = 0xFFFF)
        {
            List<long> results = new List<long>();
            long searchAddress = startAddress;

            int needleIndex = 0;
            int bufferIndex = 0;

            while (true)
            {
                try
                {
                    byte[] buffer = ReadBytes(processHandle, searchAddress, bufferSize);

                    for (bufferIndex = 0; bufferIndex < buffer.Length; bufferIndex++)
                    {
                        if (needle[needleIndex] == null)
                        {
                            needleIndex++;
                            continue;
                        }

                        if (needle[needleIndex] == buffer[bufferIndex])
                        {
                            needleIndex++;

                            if (needleIndex == needle.Length)
                            {
                                results.Add(searchAddress + bufferIndex - needle.Length + 1);

                                if (firstMatch)
                                    return results.ToArray();

                                needleIndex = 0;
                            }
                        }
                        else
                        {
                            needleIndex = 0;
                        }
                    }
                }
                catch
                {
                    break;
                }

                searchAddress += bufferSize;

                if (searchAddress > endAddress)
                    break;
            }

            return results.ToArray();
        }
    }
}
