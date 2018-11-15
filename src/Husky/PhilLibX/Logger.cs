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
// File: Logger.cs
// Author: Philip/Scobalula
// Description: Basic Logging Utilities
using System;
using System.IO;

namespace PhilLibX
{
    /// <summary>
    /// Basic Logging Class
    /// </summary>
    public class Logger : IDisposable
    {
        /// <summary>
        /// Message Types for logging
        /// </summary>
        public enum MessageType
        {
            INFO,
            WARNING,
            ERROR,
        }

        /// <summary>
        /// Log File Name
        /// </summary>
        public string LogFile { get; set; }

        /// <summary>
        /// Current Log Name
        /// </summary>
        private string LogName { get; set; }

        /// <summary>
        /// Active Stream
        /// </summary>
        private StreamWriter ActiveStream { get; set; }

        /// <summary>
        /// Initiate Logger
        /// </summary>
        /// <param name="logName">Log Name</param>
        /// <param name="fileName">Log File Name</param>
        public Logger(string logName, string fileName)
        {
            // Set properties
            LogFile = fileName;
            LogName = logName;

            // Write Initial 
            Write(LogName, MessageType.INFO);

            // Close
            Close();
        }

        /// <summary>
        /// Writes a message to the log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        public void Write(string message, MessageType messageType)
        {
            /*
            // Re-open stream if closed/null
            if ((ActiveStream == null) || (ActiveStream.BaseStream == null))
                ActiveStream = new StreamWriter(LogFile, true);

            // Write to file
            ActiveStream.WriteLine("{0} [ {1} ] {2}", DateTime.Now.ToString("dd-MM-yyyy - HH:mm:ss"), messageType, message);
            */
        }

        /// <summary>
        /// Closes current Streamwriter
        /// </summary>
        public void Close()
        {
            // Close it and clear it
            ActiveStream?.Close();
            ActiveStream = null;
        }

        /// <summary>
        /// Disposes of the Logger and its Streamwriter
        /// </summary>
        public void Dispose()
        {
            // Close stream
            Close();
        }
    }
}
