using Raylib_cs;
namespace raylibTest;

static class RenderInfo
{
    public static List<Triangle> tris = new()
        {
            new Triangle(
                    new Vertex(100, 100, 100),
                    new Vertex(-100, -100, 100),
                    new Vertex(-100, 100, -100),
                new Color(255, 255, 255, 255)),
            new Triangle(
                    new Vertex(100, 100, 100),
                    new Vertex(-100, -100, 100),
                    new Vertex(100, -100, -100),
                new Color(255, 0, 0, 255)),
            new Triangle(
                    new Vertex(-100, 100, -100),
                    new Vertex(100, -100, -100),
                    new Vertex(100, 100, 100),
                new Color(0, 255, 0, 255)),
            new Triangle(
                    new Vertex(-100, 100, -100),
                    new Vertex(100, -100, -100),
                    new Vertex(-100, -100, 100),
                new Color(0, 0, 255, 255))
        };

    public static Matrix3x3 HeadingTransform(float Heading)
    {
        return new Matrix3x3(new float[] {
            (float) Math.Cos(Heading), 0, -(float) Math.Sin(Heading),
            0, 1, 0,
            (float) Math.Sin(Heading), 0, (float) Math.Cos(Heading)
        });
    }

    public static Matrix3x3 PitchTransform(float Pitch)
    {
        return new Matrix3x3(new float[] {
            1, 0, 0,
            0, (float) Math.Cos(Pitch), (float) Math.Sin(Pitch),
            0, (float) -Math.Sin(Pitch), (float) Math.Cos(Pitch)
        });
    }
    public static float Heading = 0;
    public static float Pitch = 0;

    static float GetHeading()
    {
        return -Raylib.GetMouseX() * (float)Math.PI / Raylib.GetScreenWidth();
    }
    static float GetPitch()
    {
        return -Raylib.GetMouseY() * (float)Math.PI / Raylib.GetScreenHeight();
    }

    public static Matrix3x3 GetTransform()
    {
        return HeadingTransform(Heading)
            .Multiply(PitchTransform(Pitch));
    }
}