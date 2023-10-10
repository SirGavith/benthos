using Raylib_cs;
namespace Benthos;

public class Arrow : IDrawable
{
    public Vector Pos { get; set; }
    public Vector Axis;
    public Color Color { get; set; }

    public Arrow(Vector pos, Vector axis, Color color)
    {
        Pos = pos;
        Axis = axis;
        Color = color;
    }

    public List<Triangle> GetTriangles()
    {
        var tris = new List<Triangle>();

        //TODO: Only works with axis-aligned vectors
        var box = new Box(Pos, Axis, Color);
        var ball = new Ball(Pos + Axis, 10, Color);

        tris.AddRange(box.GetTriangles());
        tris.AddRange(ball.GetTriangles());

        return tris;
    }
}