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

        /// <summary>
        /// Vertex Color
        /// </summary>
        public Color Color { get; set; }
    }
}
