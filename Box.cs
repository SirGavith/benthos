using Raylib_cs;

namespace raylibTest;

public class Box
{
    public Vertex Pos;
    public Vertex Size;
    public Color Color;

    public Box(Vertex pos, Vertex size, Color color)
    {
        Pos = pos;
        Size = size;
        Color = color;
    }

    public List<Triangle> GetTriangles()
    {
        var v = new Vertex[8] {
            Pos,
            Pos + new Vertex(Size.x, 0, 0),
            Pos + new Vertex(Size.x, 0, Size.z),
            Pos + new Vertex(0     , 0, Size.z),
            Pos + new Vertex(0     , Size.y, 0),
            Pos + new Vertex(Size.x, Size.y, 0),
            Pos + Size,
            Pos + new Vertex(0     , Size.y, Size.z),
        };



        return new List<Triangle> () {
            //bottom
            new (v[0], v[1], v[2], Color),
            new (v[0], v[3], v[2], Color),
            //left
            new (v[0], v[1], v[5], Color),
            new (v[0], v[4], v[5], Color),
            //front
            new (v[1], v[2], v[6], Color),
            new (v[1], v[5], v[6], Color),
            //right
            new (v[2], v[3], v[7], Color),
            new (v[2], v[6], v[7], Color),
            //back
            new (v[0], v[3], v[7], Color),
            new (v[0], v[4], v[7], Color),
            //top
            new (v[4], v[5], v[6], Color),
            new (v[4], v[7], v[6], Color),
        };

    }
}