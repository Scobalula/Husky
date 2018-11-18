namespace Husky
{
    /// <summary>
    /// Vertex Normal Unpacking Methods
    /// </summary>
    class VertexNormal
    {
        /// <summary>
        /// Unpacks a Vertex Normal from: WaW, MW2, MW3, Bo1
        /// </summary>
        /// <param name="packedNormal">Packed 4 byte Vertex Normal</param>
        /// <returns>Resulting Vertex Normal</returns>
        public static Vector3 UnpackA(PackedUnitVector packedNormal)
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
        public static Vector3 UnpackB(PackedUnitVector packedNormal)
        {
            // Return decoded vector
            return new Vector3(
                (float)(((packedNormal.Value & 0x3FF) / 1023.0) * 2.0 - 1.0),
                (float)((((packedNormal.Value >> 10) & 0x3FF) / 1023.0) * 2.0 - 1.0),
                (float)((((packedNormal.Value >> 20) & 0x3FF) / 1023.0) * 2.0 - 1.0));
        }
    }
}
