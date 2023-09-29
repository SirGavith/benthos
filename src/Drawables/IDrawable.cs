using Raylib_cs;

namespace raylibTest;

public interface IDrawable
{
    Vector Pos { get; set; }
    Color Color { get; set; }
    List<Triangle> GetTriangles();
}