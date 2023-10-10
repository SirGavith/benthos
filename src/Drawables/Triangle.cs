using Raylib_cs;
namespace Benthos;

public class Triangle
{
    public Vector V1;
    public Vector V2;
    public Vector V3;
    public Color Color;
    public Triangle(Vector v1, Vector v2, Vector v3, Color color)
    {
        V1 = v1;
        V2 = v2;
        V3 = v3;
        Color = color;
    }
}