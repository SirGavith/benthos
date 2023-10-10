// x indicates the movement in the left and right directions
// y indicates the up-and-down movement on the screen
// z indicates depth (so the z axis is perpendicular to your screen). Positive z means "towards the observer".
using System.Numerics;

public class Vector
{
    public float X;
    public float Y;
    public float Z;
    public Vector(float x = 0, float y = 0, float z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public float Magnitude()
    {
        return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public Vector Norm()
    {
        return this / Magnitude();
    }


    public static Vector operator +(Vector a, Vector b) =>
        new (a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    
    public static Vector operator -(Vector a, Vector b) =>
        new (a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Vector operator *(Vector a, float b) =>
        new(a.X * b, a.Y * b, a.Z * b);

    public static Vector operator /(Vector a, float b) =>
        new(a.X / b, a.Y / b, a.Z / b);

    public Vector4 ToVector4()
    {
        return new Vector4(X, Y, Z, 1);
    }
    public Vector2 ToVector2()
    {
        return new Vector2(X, Y);
    }

    public static float SameSide(Vector A, Vector B, float Px, float Py)
    {
        //var V1V2 = new Vertex(B.x - A.x, B.y - A.y, B.z - A.z);
        //var V1P = new Vertex(this.x - A.x, this.y - A.y, this.z - A.z);
        //only care about z component of cross prod
        return (B.X - A.X) * (Py - A.Y) - (B.Y - A.Y) * (Px - A.X);
    }

    public float Dot(Vector B)
    {
        return X * B.X + Y * B.Y + Z * B.Z;
    }

    public Vector Cross(Vector B)
    {
        return new Vector(
            Y * B.Z - Z * B.Y,
            Z * B.X - X * B.Z,
            X * B.Y - Y * B.X
        );
    }

    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}>";
    }

}

public class Vector4
{
    public float X;
    public float Y;
    public float Z;
    public float W;
    public Vector4(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public float Magnitude()
    {
        return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
    }

    public static Vector4 operator +(Vector4 a, Vector4 b) =>
        new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);

    public static Vector4 operator -(Vector4 a, Vector4 b) =>
        new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);

    public static Vector4 operator *(Vector4 v, Matrix4x4 m) =>
        new(
            m[0, 0] * v.X + m[0, 1] * v.Y + m[0, 2] * v.Z + m[0, 3] * v.W,
            m[1, 0] * v.X + m[1, 1] * v.Y + m[1, 2] * v.Z + m[1, 3] * v.W,
            m[2, 0] * v.X + m[2, 1] * v.Y + m[2, 2] * v.Z + m[2, 3] * v.W,
            m[3, 0] * v.X + m[3, 1] * v.Y + m[3, 2] * v.Z + m[3, 3] * v.W);

    public static Vector4 operator /(Vector4 a, float b) =>
        new(a.X / b, a.Y / b, a.Z / b, a.W / b);

    public Vector2 ToVector2()
    {
        return new Vector2(X, Y);
    }

    public static Vector4 Norm(Vector4 A, Vector4 B)
    {
        return new Vector4(
            A.Y * B.Z - A.Z * B.Y,
            A.Z * B.X - A.X * B.Z,
            A.X * B.Y - A.Y * B.X,
            1
        );
    }

    public static float SameSide(Vector4 A, Vector4 B, float Px, float Py)
    {
        //var V1V2 = new Vertex(B.x - A.x, B.y - A.y, B.z - A.z);
        //var V1P = new Vertex(this.x - A.x, this.y - A.y, this.z - A.z);
        //only care about z component of cross prod
        return (B.X - A.X) * (Py - A.Y) - (B.Y - A.Y) * (Px - A.X);
    }
}