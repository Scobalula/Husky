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

namespace Husky
{
    /// <summary>
    /// A class to hold a 2-D Vector
    /// </summary>
    public class Vector2
    {
        /// <summary>
        /// X Value
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y Value
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Initializes a 2-D Vector at 0
        /// </summary>
        public Vector2()
        {
            X = 0.0;
            Y = 0.0;
        }

        /// <summary>
        /// Initializes a 2-D Vector with the given values
        /// </summary>
        /// <param name="x">X Value</param>
        /// <param name="y">Y Value</param>
        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// A class to hold a 3-D Vector
    /// </summary>
    public class Vector3 : Vector2
    {
        /// <summary>
        /// Z Value
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Initializes a 3-D Vector at 0
        /// </summary>
        public Vector3()
        {
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
        }

        /// <summary>
        /// Initializes a 3-D Vector with the given values
        /// </summary>
        /// <param name="x">X Value</param>
        /// <param name="y">Y Value</param>
        /// <param name="Z">Z Value</param>
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    /// <summary>
    /// A class to hold a 4-D Vector
    /// </summary>
    public class Vector4 : Vector3
    {
        /// <summary>
        /// W Value
        /// </summary>
        public double W { get; set; }

        /// <summary>
        /// Initializes a 4-D Vector at 0
        /// </summary>
        public Vector4()
        {
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
            W = 0.0;
        }

        /// <summary>
        /// Initializes a 4-D Vector with the given values
        /// </summary>
        /// <param name="x">X Value</param>
        /// <param name="y">Y Value</param>
        /// <param name="z">Z Value</param>
        /// <param name="w">W Value</param>
        public Vector4(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }
}
