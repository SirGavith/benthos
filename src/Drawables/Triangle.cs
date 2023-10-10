using Raylib_cs;
namespace Benthos;

public class Triangle
{
    public Vector v1;
    public Vector v2;
    public Vector v3;
    public Color Color;
    public Triangle(Vector v1, Vector v2, Vector v3, Color color)
    {
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
        this.Color = color;
    }
}