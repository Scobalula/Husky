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
// File: Imaging/Texturing.cs
// Author: Philip/Scobalula
// Description: Utilities for dealing with textures such as normal maps, merged specular/gloss maps, etc.
using System;
using System.Drawing;

namespace PhilLibX.Imaging
{
    /// <summary>
    /// Utilities for Dealing with Textures
    /// </summary>
    public class Texturing
    {
        /// <summary>
        /// Expands an XY normal map to XYZ
        /// </summary>
        /// <param name="bitmapSource">Bitmap to patch</param>
        public static void ExpandNormalMap(BitmapX bitmapSource)
        {
            // Loop X/Width
            for (int x = 0; x < bitmapSource.Width; x++)
            {
                // Loop Y/Height
                for (int y = 0; y < bitmapSource.Height; y++)
                {
                    // Get Original Color
                    var color = bitmapSource.GetPixel(x, y);
                    
                    // Patch it
                    bitmapSource.SetPixel(x, y, Color.FromArgb(255, color.R, color.G, CalculateNormalMapZValue(color.R, color.G)));
                }
            }
        }

        /// <summary>
        /// Splits a Normal Gloss and Occlusion Map packed into 1 Image
        /// </summary>
        /// <param name="bitmapSource">Bitmap to split</param>
        /// <param name="normalPath">Normal Output Path</param>
        /// <param name="glossPath">Gloss Path</param>
        /// <param name="occlusionPath">Occlusion Path</param>
        public static void SplitNormalGlossOcclusion(BitmapX bitmapSource, string normalPath, string glossPath, string occlusionPath)
        {
            // Input and Output
            using(bitmapSource)
            using (BitmapX normalMap = new BitmapX(bitmapSource.Width, bitmapSource.Height))
            using (BitmapX glossMap = new BitmapX(bitmapSource.Width, bitmapSource.Height))
            using (BitmapX occlusionMap = new BitmapX(bitmapSource.Width, bitmapSource.Height))
            {
                // Loop X/Width
                for (int x = 0; x < bitmapSource.Width; x++)
                {
                    // Loop Y/Height
                    for (int y = 0; y < bitmapSource.Height; y++)
                    {
                        // Get Original Color
                        var color = bitmapSource.GetPixel(x, y);

                        // Set Values
                        var normalColor    = Color.FromArgb(color.G, color.A, CalculateNormalMapZValue(color.G, color.A));
                        var glossColor     = Color.FromArgb(color.R, color.R, color.R);
                        var occlusionColor = Color.FromArgb(color.B, color.B, color.B);

                        // Patch Pixels
                        normalMap.SetPixel( x, y, normalColor);
                        glossMap.SetPixel(x, y, glossColor);
                        occlusionMap.SetPixel(x, y, occlusionColor);
                    }
                }

                // Save Images
                normalMap.Save(normalPath, false);
                glossMap.Save(glossPath, false);
                occlusionMap.Save(occlusionPath, false);
            }
        }

        /// <summary>
        /// Splits a Bitmap's RGB and Alpha Channel
        /// </summary>
        /// <param name="bitmapSource">Bitmap to split</param>
        /// <param name="rgbOutput">RGB Output Path</param>
        /// <param name="alphaOutput">Alpha Output</param>
        public static void SplitColorAndAlphaChannel(BitmapX bitmapSource, string rgbOutput, string alphaOutput)
        {
            // Input and Output
            using (bitmapSource)
            using (BitmapX rgbMap = new BitmapX(bitmapSource.Width, bitmapSource.Height))
            using (BitmapX alphaMap = new BitmapX(bitmapSource.Width, bitmapSource.Height))
            {

                // Loop X/Width
                for (int x = 0; x < bitmapSource.Width; x++)
                {
                    // Loop Y/Height
                    for (int y = 0; y < bitmapSource.Height; y++)
                    {
                        // Get Original Color
                        var color = bitmapSource.GetPixel(x, y);

                        // Set Values
                        var rgbColor = Color.FromArgb(color.R, color.G, color.B);
                        var alphaColor = Color.FromArgb(color.A, color.A, color.A);

                        // Patch Pixels
                        rgbMap.SetPixel(x, y, rgbColor);
                        alphaMap.SetPixel(x, y, alphaColor);
                    }
                }

                // Save Images
                rgbMap.Save(rgbOutput, false);
                alphaMap.Save(alphaOutput, false);
            }
        }

        /// <summary>
        /// Splits Specular and Albedo Maps by using a Metallic Mask
        /// </summary>
        /// <param name="albedoSource">Bitmap to split</param>
        /// <param name="specularMaskSource">Bitmap to use as Specular Mask</param>
        /// <param name="albedoOutput">Albedo Output Path</param>
        /// <param name="specularOutput">Specular Output Path</param>
        /// <param name="channel">Specular Mask Channel to use as a mask</param>
        /// <param name="clampValue">Clamp Specular Values to this Value</param>
        /// <param name="removeAlpha">Wehther to remove the Alpha Channel from the Color map</param>
        public static void SplitSpecularAlbedo(BitmapX albedoSource, BitmapX specularMaskSource, string albedoOutput, string specularOutput, int channel = 0, int clampValue = 32, bool removeAlpha = false)
        {
            // Check Pixel Counts
            if (albedoSource.PixelCount != specularMaskSource.PixelCount)
                throw new ArgumentException("Albedo and Specular Mask have different pixel counts.");

            // Input and Output
            using (albedoSource)
            using (specularMaskSource)
            using (BitmapX albedoMap = new BitmapX(albedoSource.Width, albedoSource.Height))
            using (BitmapX specularMap = new BitmapX(albedoSource.Width, albedoSource.Height))
            {

                // Loop X/Width
                for (int x = 0; x < albedoSource.Width; x++)
                {
                    // Loop Y/Height
                    for (int y = 0; y < albedoSource.Height; y++)
                    {
                        // Get Values 
                        var albedoSourceColor = albedoSource.GetPixel(x, y);
                        var specularMaskSourceColor = specularMaskSource.GetPixel(x, y);

                        // Specular/Metallic value
                        double specularAmount = MathUtilities.Clamp(GetChannelByIndex(specularMaskSourceColor, channel) / 255.0, 1.0, 0.0);

                        // Color Map Value (if metalic will be black)
                        double colorAmount = 1 - specularAmount;

                        // Set Values
                        var specularColor = Color.FromArgb(
                            specularAmount > 0.5 ? (int)MathUtilities.Clamp((albedoSourceColor.R * specularAmount), 255, clampValue) : 56,
                            specularAmount > 0.5 ? (int)MathUtilities.Clamp((albedoSourceColor.G * specularAmount), 255, clampValue) : 56,
                            specularAmount > 0.5 ? (int)MathUtilities.Clamp((albedoSourceColor.B * specularAmount), 255, clampValue) : 56);
                        var albedoColor = Color.FromArgb(
                            albedoSourceColor.A,
                            colorAmount < 0.5 ? (int)(albedoSourceColor.R * colorAmount) : albedoSourceColor.R,
                            colorAmount < 0.5 ? (int)(albedoSourceColor.G * colorAmount) : albedoSourceColor.G,
                            colorAmount < 0.5 ? (int)(albedoSourceColor.B * colorAmount) : albedoSourceColor.B);

                        // Patch Pixels
                        albedoMap.SetPixel(x, y, albedoColor);
                        specularMap.SetPixel(x, y, specularColor);
                    }
                }

                // Close them
                albedoSource.Dispose();
                specularMaskSource.Dispose();

                // Save Images
                albedoMap.Save(albedoOutput, false);
                specularMap.Save(specularOutput, false);
            }
        }

        /// <summary>
        /// Splits a Bitmap's RGB and Alpha Channel
        /// </summary>
        /// <param name="bitmapSource">Bitmap to split</param>
        /// <param name="rgbOutput">RGB Output Path</param>
        /// <param name="alphaOutput">Alpha Output</param>
        public static void SplitAllChannels(BitmapX bitmapSource, string rOutput, string gOutput, string bOutput, string aOutput)
        {
            // Input and Output
            using (bitmapSource)
            using (BitmapX rMap = new BitmapX(bitmapSource.Width, bitmapSource.Height))
            using (BitmapX gMap = new BitmapX(bitmapSource.Width, bitmapSource.Height))
            using (BitmapX bMap = new BitmapX(bitmapSource.Width, bitmapSource.Height))
            using (BitmapX aMap = new BitmapX(bitmapSource.Width, bitmapSource.Height))
            {

                // Loop X/Width
                for (int x = 0; x < bitmapSource.Width; x++)
                {
                    // Loop Y/Height
                    for (int y = 0; y < bitmapSource.Height; y++)
                    {
                        // Get Original Color
                        var color = bitmapSource.GetPixel(x, y);

                        // Set Values
                        var rCol = Color.FromArgb(color.R, color.R, color.R);
                        var gCol = Color.FromArgb(color.G, color.G, color.G);
                        var bCol = Color.FromArgb(color.B, color.B, color.B);
                        var aCol = Color.FromArgb(color.A, color.A, color.A);

                        // Patch Pixels
                        rMap.SetPixel(x, y, rCol);
                        gMap.SetPixel(x, y, gCol);
                        bMap.SetPixel(x, y, bCol);
                        aMap.SetPixel(x, y, aCol);
                    }
                }

                // Save Images
                rMap.Save(rOutput, false);
                gMap.Save(gOutput, false);
                bMap.Save(bOutput, false);
                aMap.Save(aOutput, false);
            }
        }

        /// <summary>
        /// Gets Channel Value by Index
        /// </summary>
        /// <param name="color">Color Value</param>
        /// <param name="channel">Channel Index</param>
        /// <returns>Result</returns>
        private static int GetChannelByIndex(Color color, int channel)
        {
            // Switch Channel
            switch(channel)
            {
                case 0:
                    return color.R;
                case 1:
                    return color.G;
                case 2:
                    return color.B;
                case 3:
                    return color.A;
                default:
                    return 255;
            }
        }

        /// <summary>
        /// Calculates Blue Channel Value for a normal map from X and Y Value, from DTZxPorter
        /// </summary>
        /// <param name="x">Red Value as an Int from 0 to 255</param>
        /// <param name="y">Green Value as an Int from 0 to 255</param>
        /// <returns>Blue Channel Value as an Int from 0 to 255</returns>
        public static int CalculateNormalMapZValue(int x, int y)
        {
            // Get Values
            double X = 2.0 * (x / 255.0000) - 1;
            double Y = 2.0 * (y / 255.0000) - 1;
            double Z = 0.0000000;

            // Check if we can average the value
            if ((1 - (X * X) - (Y * Y)) > 0)
                Z = Math.Sqrt(1 - (X * X) - (Y * Y));

            // Return Final Value
            return (int)(MathUtilities.Clamp((Z + 1.0) / 2.0, 1.0, 0.0) * 255);
        }
    }
}
