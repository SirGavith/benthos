using Raylib_cs;

namespace raylibTest;

public interface IDrawable
{
    Vertex Pos { get; set; }
    Color Color { get; set; }
    List<Triangle> GetTriangles();
}