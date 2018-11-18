// ------------------------------------------------------------------------
// Husky - Call of Duty BSP Extractor
// Copyright (C) 2018 Philip/Scobalula
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------
using System;

namespace Husky
{
    /// <summary>
    /// Rotation Utilities
    /// </summary>
    public class Rotation
    {
        /// <summary>
        /// Class to hold a 4x4 Matrix
        /// </summary>
        public class Matrix
        {
            /// <summary>
            /// Values
            /// </summary>
            public double[] Values = new double[16];

            /// <summary>
            /// Converts a Matrix to a Quaternion
            /// </summary>
            /// <returns>Resulting Quaternion</returns>
            public Vector4 ToQuaternion()
            {
                Vector4 result = new Vector4(0, 0, 0, 1.0);

                double divisor;

                double transRemain = Values[0] + Values[5] + Values[10];

                if (transRemain > 0)
                {
                    divisor = Math.Sqrt(transRemain + 1.0) * 2.0;
                    result.W = 0.25 * divisor;
                    result.X = (Values[6] - Values[9]) / divisor;
                    result.Y = (Values[8] - Values[2]) / divisor;
                    result.Z = (Values[1] - Values[4]) / divisor;
                }
                else if ((Values[0] > Values[5]) && (Values[0] > Values[10]))
                {
                    divisor = Math.Sqrt(
                        1.0 + Values[0] - Values[5] - Values[10]) * 2.0;
                    result.W = (Values[6] - Values[9]) / divisor;
                    result.X = 0.25 * divisor;
                    result.Y = (Values[4] + Values[1]) / divisor;
                    result.Z = (Values[8] + Values[2]) / divisor;
                }
                else if (Values[5] > Values[10])
                {
                    divisor = Math.Sqrt(
                        1.0 + Values[5] - Values[0] - Values[10]) * 2.0;
                    result.W = (Values[8] - Values[2]) / divisor;
                    result.X = (Values[4] + Values[1]) / divisor;
                    result.Y = 0.25 * divisor;
                    result.Z = (Values[9] + Values[6]) / divisor;
                }
                else
                {
                    divisor = Math.Sqrt(
                        1.0 + Values[10] - Values[0] - Values[5]) * 2.0;
                    result.W = (Values[1] - Values[4]) / divisor;
                    result.X = (Values[8] + Values[2]) / divisor;
                    result.Y = (Values[9] + Values[6]) / divisor;
                    result.Z = 0.25 * divisor;
                }

                // Return resulting vector
                return result;
            }

            /// <summary>
            /// Converts a Matrix to Euler Angles
            /// </summary>
            /// <returns>Resulting Euler Vector</returns>
            public Vector3 ToEuler()
            {
                // I need to switch this back from Mat -> Quat -> Euler to straight Mat -> Euler
                // I thought none the calculations were working, NO, the bastards
                // in the .MAP files store them as YXZ or some crap like that
                var quaternion = ToQuaternion();

                Vector3 result = new Vector3();

                double t0 = 2.0 * (quaternion.W * quaternion.X + quaternion.Y * quaternion.Z);
                double t1 = 1.0 - 2.0 * (quaternion.X * quaternion.X + quaternion.Y * quaternion.Y);

                result.X = Math.Atan2(t0, t1);


                double t2 = 2.0 * (quaternion.W * quaternion.Y - quaternion.Z * quaternion.X);

                t2 = t2 > 1.0 ? 1.0 : t2;
                t2 = t2 < -1.0 ? -1.0 : t2;
                result.Y = Math.Asin(t2);


                double t3 = +2.0 * (quaternion.W * quaternion.Z + quaternion.X * quaternion.Y);
                double t4 = +1.0 - 2.0 * (quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z);

                result.Z = Math.Atan2(t3, t4);

                return result;
            }
        }

        /// <summary>
        /// Converts Euler value to degrees
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>Value in degrees</returns>
        public static double ToDegrees(double value)
        {
            return (value / (2 * Math.PI)) * 360;
        }

        /// <summary>
        /// Converts Euler value to degrees
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>Value in degrees</returns>
        public static Vector3 ToDegrees(Vector3 value)
        {
            // Return new vector
            return new Vector3(
                ToDegrees(value.X),
                ToDegrees(value.Y),
                ToDegrees(value.Z)
                );
        }
    }
}
