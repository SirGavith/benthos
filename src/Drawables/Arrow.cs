using Raylib_cs;
namespace raylibTest;

public class Arrow : IDrawable
{
    public Vertex Pos { get; set; }
    public Vertex Axis;
    public Color Color { get; set; }

    public Arrow(Vertex pos, Vertex axis, Color color)
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