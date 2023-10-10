using Raylib_cs;

namespace Benthos;

public interface IDrawable
{
    Vector Pos { get; set; }
    Color Color { get; set; }
    List<Triangle> GetTriangles();
}