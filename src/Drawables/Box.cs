using Raylib_cs;

namespace Benthos;

public class Box : IDrawable
{
    public Vector Pos { get; set; }
    public Vector Size;
    public Color Color { get; set; }

    public Box(Vector pos, Vector size, Color color)
    {
        Pos = pos;
        Size = size;
        Color = color;
    }

    public List<Triangle> GetTriangles()
    {
        var v = new Vector[8] {
            Pos,
            Pos + new Vector(Size.X, 0, 0),
            Pos + new Vector(Size.X, 0, Size.Z),
            Pos + new Vector(0     , 0, Size.Z),
            Pos + new Vector(0     , Size.Y, 0),
            Pos + new Vector(Size.X, Size.Y, 0),
            Pos + Size,
            Pos + new Vector(0     , Size.Y, Size.Z),
        };

        return new List<Triangle>() {
            //bottom
            new (v[0], v[1], v[2], Color),
            new (v[0], v[2], v[3], Color),
            //left
            new (v[0], v[5], v[1], Color),
            new (v[0], v[4], v[5], Color),
            //front
            new (v[1], v[6], v[2], Color),
            new (v[1], v[5], v[6], Color),
            //right
            new (v[2], v[7], v[3], Color),
            new (v[2], v[6], v[7], Color),
            //back
            new (v[0], v[3], v[7], Color),
            new (v[0], v[7], v[4], Color),
            //top
            new (v[4], v[6], v[5], Color),
            new (v[4], v[7], v[6], Color),
        };

        // return new List<Triangle>() {
        //     //bottom
        //     new (v[0], v[1], v[2], Color.BLUE),
        //     new (v[0], v[2], v[3], Color.DARKBLUE),
        //     //left
        //     new (v[0], v[5], v[1], Color.BROWN),
        //     new (v[0], v[4], v[5], Color.DARKBROWN),
        //     //front
        //     new (v[1], v[6], v[2], Color.GREEN),
        //     new (v[1], v[5], v[6], Color.DARKGREEN),
        //     //right
        //     new (v[2], v[7], v[3], Color.PURPLE),
        //     new (v[2], v[6], v[7], Color.DARKPURPLE),
        //     //back
        //     new (v[0], v[3], v[7], Color.RED),
        //     new (v[0], v[7], v[4], Color.ORANGE),
        //     //top
        //     new (v[4], v[6], v[5], Color.YELLOW),
        //     new (v[4], v[7], v[6], Color.GOLD),
        // };
    }
}