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
    /// Vertex Normal Unpacking Methods
    /// </summary>
    class VertexNormalUnpacking
    {
        /// <summary>
        /// Unpacks a Vertex Normal from: WaW, MW2, MW3, Bo1
        /// </summary>
        /// <param name="packedNormal">Packed 4 byte Vertex Normal</param>
        /// <returns>Resulting Vertex Normal</returns>
        public static Vector3 MethodA(PackedUnitVector packedNormal)
        {
            // Decode the scale of the vector
            float decodeScale = ((float)(packedNormal.Byte4 - -192.0) / 32385.0f);

            // Return decoded vector
            return new Vector3(
                (float)(packedNormal.Byte1 - 127.0) * decodeScale,
                (float)(packedNormal.Byte2 - 127.0) * decodeScale,
                (float)(packedNormal.Byte3 - 127.0) * decodeScale);
        }

        /// <summary>
        /// Unpacks a Vertex Normal from: Ghosts, AW, MWR
        /// </summary>
        /// <param name="packedNormal">Packed 4 byte Vertex Normal</param>
        /// <returns>Resulting Vertex Normal</returns>
        public static Vector3 MethodB(PackedUnitVector packedNormal)
        {
            // Return decoded vector
            return new Vector3(
                (float)(((packedNormal.Value & 0x3FF) / 1023.0) * 2.0 - 1.0),
                (float)((((packedNormal.Value >> 10) & 0x3FF) / 1023.0) * 2.0 - 1.0),
                (float)((((packedNormal.Value >> 20) & 0x3FF) / 1023.0) * 2.0 - 1.0));
        }

        /// <summary>
        /// Unpacks a Vertex Normal from: Bo2
        /// </summary>
        /// <param name="packedNormal">Packed 4 byte Vertex Normal</param>
        /// <returns>Resulting Vertex Normal</returns>
        public static Vector3 MethodC(PackedUnitVector packedNormal)
        {
            // Resulting values
            var builtX = new FloatToInt { Integer = (uint)((packedNormal.Value & 0x3FF) - 2 * (packedNormal.Value & 0x200) + 0x40400000) };
            var builtY = new FloatToInt { Integer = (uint)((((packedNormal.Value >> 10) & 0x3FF) - 2 * ((packedNormal.Value >> 10) & 0x200) + 0x40400000)) };
            var builtZ = new FloatToInt { Integer = (uint)((((packedNormal.Value >> 20) & 0x3FF) - 2 * ((packedNormal.Value >> 20) & 0x200) + 0x40400000)) };

            // Return decoded vector
            return new Vector3(
                (builtX.Float - 3.0) * 8208.0312f,
                (builtY.Float - 3.0) * 8208.0312f,
                (builtZ.Float - 3.0) * 8208.0312f);
        }

        /// <summary>
        /// Unpacks a Vertex Normal from: Bo3
        /// </summary>
        /// <param name="packedNormal">Packed 4 byte Vertex Normal</param>
        /// <returns>Resulting Vertex Normal</returns>
        public static Vector3 MethodD(PackedUnitVector packedNormal)
        {
            // Unpack normal
            int packedX = (((packedNormal.Value >> 0) & ((1 << 10) - 1)) - 512);
            int packedY = (((packedNormal.Value >> 10) & ((1 << 10) - 1)) - 512);
            int packedZ = (((packedNormal.Value >> 20) & ((1 << 10) - 1)) - 512);

            // Return decoded vector
            return new Vector3(
                packedX / 511.0,
                packedY / 511.0,
                packedZ / 511.0);

        }

        public static (Vector3, Vector3, Vector3) UnpackTangentFrame(uint packedQuat)
        {
            // https://dev.theomader.com/qtangents/
            var idx = packedQuat >> 30;

            var tx = ((((packedQuat >> 00) & 0x3FF) / 511.5f) - 1.0f) / 1.4142135f;
            var ty = ((((packedQuat >> 10) & 0x3FF) / 511.5f) - 1.0f) / 1.4142135f;
            var tz = ((((packedQuat >> 20) & 0x1FF) / 255.5f) - 1.0f) / 1.4142135f;
            var tw = 0.0f;

            var sum = (tx * tx) + (ty * ty) + (tz * tz);

            if (sum <= 1.0f)
                tw = (float)Math.Sqrt(1.0f - sum);

            var q = new Vector4();

            switch (idx)
            {
                case 0:
                    q.X = tw;
                    q.Y = tx;
                    q.Z = ty;
                    q.W = tz;
                    break;
                case 1:
                    q.X = tx;
                    q.Y = tw;
                    q.Z = ty;
                    q.W = tz;
                    break;
                case 2:
                    q.X = tx;
                    q.Y = ty;
                    q.Z = tw;
                    q.W = tz;
                    break;
                case 3:
                    q.X = tx;
                    q.Y = ty;
                    q.Z = tz;
                    q.W = tw;
                    break;
            }

            var tangent = new Vector3(
                1 - 2 * (q.Y * q.Y + q.Z * q.Z),
                2 * (q.X * q.Y + q.W * q.Z),
                2 * (q.X * q.Z - q.W * q.Y));
            var bitangent = new Vector3(
                2 * (q.X * q.Y - q.W * q.Z),
                1 - 2 * (q.X * q.X + q.Z * q.Z),
                2 * (q.Y * q.Z + q.W * q.X));
            var normal = Vector3.Cross(tangent, bitangent);

            return (tangent, bitangent, normal);
        }
    }
}
