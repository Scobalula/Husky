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
// File: Printer.cs
// Author: Philip/Scobalula
// Description: Class to Print stuff to the Console with more swag 🖨️
using System;
using System.Diagnostics;
using System.IO;

namespace PhilLibX
{
    /// <summary>
    /// Class to Print stuff to the Console with more swag 🖨️
    /// </summary>
    public class Printer
    {
        /// <summary>
        /// Prefix Padding
        /// </summary>
        private static int PrefixPadding = 12;

        /// <summary>
        /// Prefix Color
        /// </summary>
        private static ConsoleColor PrefixColor = ConsoleColor.DarkBlue;

        /// <summary>
        /// Sets Prefix Padding
        /// </summary>
        /// <param name="padding">Padding Size</param>
        public static void SetPrefixPadding(int padding)
        {
            PrefixPadding = padding;
        }

        /// <summary>
        /// Sets the Console Prefix Color
        /// </summary>
        /// <param name="color">Background Color for Prefix</param>
        public static void SetPrefixBackgroundColor(ConsoleColor color)
        {
            PrefixColor = color;
        }

        /// <summary>
        /// Writes a line to the console with optional BackGround
        /// </summary>
        /// <param name="value">Value to print</param>
        /// <param name="prefix">Value to prefix</param>
        /// <param name="padding">Prefix Padding</param>
        /// <param name="backgroundColor">Background Color</param>
        /// <param name="prefixColor">Prefix Background Color</param>
        public static void WriteLine(object prefix = null, object value = null, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            // Check if we even have a prefix, if not just do a normal print
            if(prefix != null)
            {
                Console.BackgroundColor = PrefixColor;
                Console.Write(" {0}", prefix.ToString().PadRight(PrefixPadding));
                Console.ResetColor();
                Console.Write("│ ");
            }
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine("{0}", value);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a line to the console with optional BackGround
        /// </summary>
        /// <param name="value">Value to print</param>
        /// <param name="prefix">Value to prefix</param>
        /// <param name="padding">Prefix Padding</param>
        /// <param name="backgroundColor">Background Color</param>
        /// <param name="prefixColor">Prefix Background Color</param>
        public static void WriteException(Exception exception, object prefix = null, object value = null, ConsoleColor backgroundColor = ConsoleColor.DarkRed)
        {
            // Write Initial Value
            WriteLine(prefix, value, backgroundColor);

            // Grab stack trace and recent frame
            var stackTrace = new StackTrace(exception, true);
            var stackFrame = stackTrace.GetFrame(0);

            // Write Formatted Exception
            WriteLine(prefix, exception.Message, backgroundColor);
            WriteLine(prefix, String.Format("{0}::{1}.{2}(...)::Line::{3}:{4}", 
                Path.GetFileName(stackFrame.GetFileName()),
                stackFrame.GetMethod().ReflectedType.Name,
                stackFrame.GetMethod().Name,
                stackFrame.GetFileLineNumber(),
                stackFrame.GetFileColumnNumber()), backgroundColor);

            Console.ResetColor();
        }

        /// <summary>
        /// Writes to the console with optional BackGround
        /// </summary>
        /// <param name="value">Value to print</param>
        /// <param name="prefix">Value to prefix</param>
        /// <param name="padding">Prefix Padding</param>
        /// <param name="backgroundColor">Background Color</param>
        /// <param name="prefixColor">Prefix Background Color</param>
        public static void Write(object prefix = null, object value = null, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            // Check if we even have a prefix, if not just do a normal print
            if (prefix != null)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write(" {0}", prefix.ToString().PadRight(PrefixPadding));
                Console.ResetColor();
                Console.Write("│ ");
            }
            Console.BackgroundColor = backgroundColor;
            Console.Write("{0}", value);
            Console.ResetColor();
        }
    }
}
