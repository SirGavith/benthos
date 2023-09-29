// x indicates the movement in the left and right directions
// y indicates the up-and-down movement on the screen
// z indicates depth (so the z axis is perpendicular to your screen). Positive z means "towards the observer".
using System.Numerics;

public class Vertex
{
    public float x;
    public float y;
    public float z;
    public Vertex(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public float Magnitude()
    {
        return (float)Math.Sqrt(x * x + y * y + z * z);
    }

    public static Vertex operator +(Vertex a, Vertex b) =>
        new (a.x + b.x, a.y + b.y, a.z + b.z);
    
    public static Vertex operator -(Vertex a, Vertex b) =>
        new (a.x - b.x, a.y - b.y, a.z - b.z);

    public static Vertex operator *(Vertex a, float b) =>
        new(a.x * b, a.y * b, a.z * b);

    public static Vertex operator /(Vertex a, float b) =>
        new(a.x / b, a.y / b, a.z / b);

    public Vertex4 ToVertex4()
    {
        return new Vertex4(x, y, z, 1);
    }

    public static float SameSide(Vertex A, Vertex B, float Px, float Py)
    {
        //var V1V2 = new Vertex(B.x - A.x, B.y - A.y, B.z - A.z);
        //var V1P = new Vertex(this.x - A.x, this.y - A.y, this.z - A.z);
        //only care about z component of cross prod
        return (B.x - A.x) * (Py - A.y) - (B.y - A.y) * (Px - A.x);
    }

    public float Dot(Vertex B)
    {
        return x * B.x + y * B.y + z * B.z;
    }

    public Vertex Cross(Vertex B)
    {
        return new Vertex(
            y * B.z - z * B.y,
            z * B.x - x * B.z,
            x * B.y - y * B.x
        );
    }
}

public class Vertex4
{
    public float x;
    public float y;
    public float z;
    public float w;
    public Vertex4(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public float Magnitude()
    {
        return (float)Math.Sqrt(x * x + y * y + z * z + w * w);
    }

    public static Vertex4 operator +(Vertex4 a, Vertex4 b) =>
        new(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);

    public static Vertex4 operator -(Vertex4 a, Vertex4 b) =>
        new(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);

    public static Vertex4 operator *(Vertex4 v, Matrix4x4 m) =>
        new(
            m[0, 0] * v.x + m[0, 1] * v.y + m[0, 2] * v.z + m[0, 3] * v.w,
            m[1, 0] * v.x + m[1, 1] * v.y + m[1, 2] * v.z + m[1, 3] * v.w,
            m[2, 0] * v.x + m[2, 1] * v.y + m[2, 2] * v.z + m[2, 3] * v.w,
            m[3, 0] * v.x + m[3, 1] * v.y + m[3, 2] * v.z + m[3, 3] * v.w);

    public static Vertex4 operator /(Vertex4 a, float b) =>
        new(a.x / b, a.y / b, a.z / b, a.w / b);



    public static Vertex4 Norm(Vertex4 A, Vertex4 B)
    {
        return new Vertex4(
            A.y * B.z - A.z * B.y,
            A.z * B.x - A.x * B.z,
            A.x * B.y - A.y * B.x,
            1
        );
    }

    public static float SameSide(Vertex4 A, Vertex4 B, float Px, float Py)
    {
        //var V1V2 = new Vertex(B.x - A.x, B.y - A.y, B.z - A.z);
        //var V1P = new Vertex(this.x - A.x, this.y - A.y, this.z - A.z);
        //only care about z component of cross prod
        return (B.x - A.x) * (Py - A.y) - (B.y - A.y) * (Px - A.x);
    }
}