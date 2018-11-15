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
// File: Imaging/BitmapX.cs
// Author: Philip/Scobalula
// Description: Faster Bitmap Processing
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace PhilLibX.Imaging
{
    /// <summary>
    /// Faster Bitmap Processing
    /// </summary>
    public class BitmapX : IDisposable
    {
        /// <summary>
        /// Accepted Bits Per Pixel (for now we only support 24 and 32bpp
        /// </summary>
        public static readonly int[] AcceptedBitsPerPixel =
        {
            24,
            32,
        };

        /// <summary>
        /// Pixel Data Pointer
        /// </summary>
        private IntPtr PixelDataPointer { get; set; }

        /// <summary>
        /// Raw Pixel Data
        /// </summary>
        private byte[] Pixels { get; set; }

        /// <summary>
        /// Source Bitmap Object
        /// </summary>
        private Bitmap BitmapSource { get; set; }

        /// <summary>
        /// Source Bitmap Data Object
        /// </summary>
        private BitmapData BitmapDataSource { get; set; }

        /// <summary>
        /// Internal Bits Per Pizel Value
        /// </summary>
        private int _BitsPerPixel { get; set; }

        /// <summary>
        /// Internal Bitmap Width Value
        /// </summary>
        private int _Width { get; set; }

        /// <summary>
        /// Internal  Bitmap Height Value
        /// </summary>
        private int _Height { get; set; }

        /// <summary>
        /// Bitmap Width
        /// </summary>
        public int Width { get { return _Width; } }

        /// <summary>
        /// Bitmap Height
        /// </summary>
        public int Height { get { return _Height; } }

        /// <summary>
        /// Number of bits per pixel. (24, 
        /// </summary>
        public int BitsPerPixel { get { return _BitsPerPixel; } }

        /// <summary>
        /// Number of bits per pixel. (24, 
        /// </summary>
        public int BytesPerPixel { get { return BitsPerPixel / 8; } }

        /// <summary>
        /// Pixel Count
        /// </summary>
        public int PixelCount { get { return Width * Height; } }

        /// <summary>
        /// Initializes BitmapX with a new Bitmap
        /// </summary>
        /// <param name="width">Pixel Width</param>
        /// <param name="height">Pixel Height</param>
        /// <param name="pixelFormat">Pixel Format (32bpp ARGB by Default)</param>
        public BitmapX(int width, int height, PixelFormat pixelFormat = PixelFormat.Format32bppArgb)
        {
            LoadBitmap(new Bitmap(width, height, pixelFormat));
        }

        /// <summary>
        /// Initializes BitmapX with a file
        /// </summary>
        /// <param name="fileName">Path to image file</param>
        public BitmapX(string fileName)
        {
            LoadBitmap(new Bitmap(fileName));
        }

        /// <summary>
        /// Initializes BitmapX with an Image
        /// </summary>
        /// <param name="image">Existing Image Object</param>
        public BitmapX(Image image)
        {
            LoadBitmap(new Bitmap(image));
        }

        /// <summary>
        /// Initializes BitmapX with a Stream
        /// </summary>
        /// <param name="stream">Stream with Image data</param>
        public BitmapX(Stream stream)
        {
            LoadBitmap(new Bitmap(stream));
        }

        /// <summary>
        /// Initializes BitmapX with a Bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap Source</param>
        public BitmapX(Bitmap bitmap)
        {
            LoadBitmap(bitmap);
        }

        /// <summary>
        /// Loads a Bitmap into the BitmapX
        /// </summary>
        /// <param name="bitmap">Bitmap Source</param>
        public void LoadBitmap(Bitmap bitmap)
        {
            // Dispose the Original if exists
            BitmapSource?.Dispose();

            // Get Bpp
            _BitsPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat);

            // Check for supported Bpp
            if (!AcceptedBitsPerPixel.Contains(BitsPerPixel))
                throw new ArgumentException("Unsupported Bitmap Pixel Size: " + BitsPerPixel.ToString());

            // Set Bitmap
            BitmapSource = bitmap;

            // Set Width + Height
            _Width = BitmapSource.Width;
            _Height = BitmapSource.Height;

            LockBits();
        }

        /// <summary>
        /// Locks the Bits of the Source Bitmap
        /// </summary>
        public void LockBits()
        {
            // Lock the Bits
            BitmapDataSource = BitmapSource.LockBits(
                new Rectangle(0, 0, BitmapSource.Width, BitmapSource.Height),
                ImageLockMode.ReadWrite,
                BitmapSource.PixelFormat);

            // Set Pixel Array
            Pixels = new byte[PixelCount * BytesPerPixel];

            // Set Pixel Pointer
            PixelDataPointer = BitmapDataSource.Scan0;

            // Copy the Data, maintain safe code using Marshal instead of accessing the raw pointer
            Marshal.Copy(PixelDataPointer, Pixels, 0, Pixels.Length);
        }

        /// <summary>
        /// Unlocks the Bits of the Source Image
        /// </summary>
        public void UnlockBits()
        {
            // Copy the Data, maintain safe code using Marshal instead of accessing the raw pointer
            Marshal.Copy(Pixels, 0, PixelDataPointer, Pixels.Length);

            // Unlock bitmap data
            BitmapSource.UnlockBits(BitmapDataSource);
        }

        /// <summary>
        /// Gets Color at the given Pixel
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>Resulting Color</returns>
        public Color GetPixel(int x, int y)
        {
            // Get Position of the Pixe, based off Bpp
            int pixelIndex = ((y * Width) + x) * BytesPerPixel;

            // Convert to Color, only take Alpha if we're 32Bpp
            Color result = Color.FromArgb(
                // Alpha
                BitsPerPixel == 32 ? Pixels[pixelIndex + 3] : 255,
                // Red
                Pixels[pixelIndex + 2],
                // Green
                Pixels[pixelIndex + 1],
                // Blue
                Pixels[pixelIndex]);

            // Ship back the result
            return result;
        }

        /// <summary>
        /// Sets Color at the given Pixel 
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <param name="color">Color to set</param>
        public void SetPixel(int x, int y, Color color)
        {
            // Get Position of the Pixe, based off Bpp
            int pixelIndex = ((y * Width) + x) * BytesPerPixel;

            // Set Pixel Data
            // Blue
            Pixels[pixelIndex] = color.B;
            // Green
            Pixels[pixelIndex + 1] = color.G;
            // Red
            Pixels[pixelIndex + 2] = color.R;

            // Set Alpha only if we're 32Bpp
            if (BitsPerPixel == 32)
                Pixels[pixelIndex + 3] = color.A;
        }

        public void Save(string filePath, bool relockBits = true)
        {
            // Unlock the Bits
            UnlockBits();


            // Save the Bitmap
            BitmapSource.Save(filePath);


            // Check if we must relock the bits
            if (relockBits) LockBits();
        }

        /// <summary>
        /// Checks if this image is 1 color
        /// </summary>
        /// <returns>False if has pixel differences, otherwise True</returns>
        public bool IsSingleColor()
        {
            // Color trackers
            int previousColor = 0;
            int pixel;

            // Get initial pixel
            for (int i = 0; i < BytesPerPixel; i++)
                previousColor += Pixels[i];

            // Loop pixels
            for (int i = 0; i < PixelCount; i += BytesPerPixel)
            {
                // Initial Value
                pixel = 0;

                // Sum values
                for (int j = 0; j < BytesPerPixel; j++)
                    pixel += Pixels[i + j];

                // Compare
                if (previousColor != pixel)
                    return false;

                // Set for next comparison
                previousColor = pixel;
            }

            // All 1 color if we got here
            return true;
        }

        /// <summary>
        /// Checks if this image has colors greater than the given values
        /// </summary>
        /// <param name="rValue"></param>
        /// <param name="bValue"></param>
        /// <param name="gValue"></param>
        /// <returns></returns>
        public bool HasColorsGreaterThan(int rValue, int bValue, int gValue)
        {
            // Color Values to compare
            int[] colorCheck =
            {
                rValue,
                bValue,
                gValue
            };

            // Loop pixels
            for (int i = 0; i < PixelCount; i += BytesPerPixel)
                for (int j = 0; j < 3; j++)
                    if (Pixels[i + j] >= colorCheck[j])
                        return false;

            // All 1 color if we got here
            return true;
        }

        /// <summary>
        /// Checks if this image is monochrome
        /// </summary>
        /// <returns>False if has pixel differences, otherwise True</returns>
        public bool IsMonochrome()
        {
            // Loop pixels
            for (int i = 0; i < PixelCount; i += BytesPerPixel)
            {
                // We only care about actual color values
                if (
                    Pixels[i] != Pixels[i + 1] || 
                    Pixels[i] != Pixels[i + 2] || 
                    Pixels[i + 1] != Pixels[i + 2])
                    return false;
            }

            // All 1 color if we got here
            return true;
        }

        /// <summary>
        /// Disposes of the BitmapX Object
        /// </summary>
        public void Dispose()
        {
            // Dispose the Bitmap
            BitmapSource?.Dispose();
        }
    }
}
