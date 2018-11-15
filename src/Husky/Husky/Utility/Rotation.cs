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
    class Rotation
    {
        /// <summary>
        /// Converts Eular value to degrees
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>Value in degrees</returns>
        public static float ToDegrees(float value)
        {
            return (float)(value / (2 * Math.PI)) * 360;
        }
    }
}
