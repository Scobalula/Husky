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
    /// Vertex Class (Position, Offset, etc.)
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// Vertex Position
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Vertex Normal
        /// </summary>
        public Vector3 Normal { get; set; }

        /// <summary>
        /// Vertex UV/Texture Coordinates
        /// </summary>
        public Vector2 UV { get; set; }

        public static Vector3 UnpackVGVertex(ulong PackedPosition, float scale, float[] midPoint)
        {
            return new Vector3(
                ((((PackedPosition >> 0) & 0x1FFFFF) * scale) + midPoint[0]) * 2.54,
                ((((PackedPosition >> 21) & 0x1FFFFF) * scale) + midPoint[1]) * 2.54,
                ((((PackedPosition >> 42) & 0x1FFFFF) * scale) + midPoint[2]) * 2.54);
        }
    }
}
