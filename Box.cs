using Raylib_cs;

namespace raylibTest;

public class Box : IDrawable
{
    public Vertex Pos { get; set; }
    public Vertex Size;
    public Color Color { get; set; }

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
            new (v[0], v[1], v[2], new Color(200, 200, 200, 255)),
            new (v[0], v[3], v[2], new Color(130, 130, 130, 255)),
            //left
            new (v[0], v[1], v[5], new Color(80, 80, 80, 255)),
            new (v[0], v[4], v[5], new Color(253, 249, 0, 255)),
            //front
            new (v[1], v[2], v[6], new Color(255, 203, 0, 255)),
            new (v[1], v[5], v[6], new Color(255, 161, 0, 255)),
            //right
            new (v[2], v[3], v[7], new Color(200, 200, 200, 255)),
            new (v[2], v[6], v[7], new Color(130, 130, 130, 255)),
            //back
            new (v[0], v[3], v[7], new Color(80, 80, 80, 255)),
            new (v[0], v[4], v[7], new Color(253, 249, 0, 255)),
            //top
            new (v[4], v[5], v[6], new Color(255, 203, 0, 255)),
            new (v[4], v[7], v[6], new Color(255, 161, 0, 255)),
        };

    }
}