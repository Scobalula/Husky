using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SELib.Utilities
{
    /// <summary>
    /// Generic interface for all key data
    /// </summary>
    public interface KeyData
    {
        // A generic interface for a container
    }

    /// <summary>
    /// A container for a vector (XY) (Normalized)
    /// </summary>
    public class Vector2 : KeyData
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2() { X = 0; Y = 0; }
        public Vector2(float XCoord, float YCoord) { X = XCoord; Y = YCoord; }

        public override bool Equals(object obj)
        {
            Vector2 vec = (Vector2)obj;
            return (vec.X == X && vec.Y == Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static readonly Vector2 Zero = new Vector2() { X = 0, Y = 0 };
        public static readonly Vector2 One = new Vector2() { X = 1, Y = 1 };
    }

    /// <summary>
    /// A container for a vector (XYZ) (Normalized)
    /// </summary>
    public class Vector3 : Vector2
    {
        public double Z { get; set; }

        public Vector3() { X = 0; Y = 0; Z = 0; }
        public Vector3(float XCoord, float YCoord, float ZCoord) { X = XCoord; Y = YCoord; Z = ZCoord; }

        public override bool Equals(object obj)
        {
            Vector3 vec = (Vector3)obj;
            return (vec.X == X && vec.Y == Y && vec.Z == Z);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Z.GetHashCode();
        }

        new public static readonly Vector3 Zero = new Vector3() { X = 0, Y = 0, Z = 0 };
        new public static readonly Vector3 One = new Vector3() { X = 1, Y = 1, Z = 1 };
    }

    /// <summary>
    /// A container for a color (RGBA)
    /// </summary>
    public class Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public Color() { R = 255; G = 255; B = 255; A = 255; }
        public Color(byte Red, byte Green, byte Blue, byte Alpha) { R = Red; G = Green; B = Blue; A = Alpha; }

        public override bool Equals(object obj)
        {
            Color vec = (Color)obj;
            return (vec.R == R && vec.G == G && vec.B == B && vec.A == A);
        }

        public override int GetHashCode()
        {
            return R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode() ^ A.GetHashCode();
        }

        public static readonly Color White = new Color(255, 255, 255, 255);
    }

    /// <summary>
    /// A container for a quaternion rotation (XYZW) (Normalized)
    /// </summary>
    public class Quaternion : Vector3
    {
        public double W { get; set; }

        public Quaternion() { X = 0; Y = 0; Z = 0; W = 1; }
        public Quaternion(float XCoord, float YCoord, float ZCoord, float WCoord) { X = XCoord; Y = YCoord; Z = ZCoord; W = WCoord; }

        public override bool Equals(object obj)
        {
            Quaternion vec = (Quaternion)obj;
            return (vec.X == X && vec.Y == Y && vec.Z == Z && vec.W == W);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ W.GetHashCode();
        }

        public static readonly Quaternion Identity = new Quaternion() { X = 0, Y = 0, Z = 0, W = 1 };
    }
}
