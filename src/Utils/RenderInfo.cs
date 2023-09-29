using System.Numerics;
using Raylib_cs;
namespace raylibTest;

static class RenderInfo
{
    public static Vertex CameraPos = new (0, 0, -10);
    public static Vertex CameraFacing = new (0, 0, 1);


    public static float MovementSpeed = 0.1f;


    public static float Heading = 0;
    public static float Pitch = (float)Math.PI;
    public static float Roll = 0;

    public static Matrix4x4 HeadingTransform(float Heading)
    {
        (double sin, double cos) = Math.SinCos(Heading);
        return new Matrix4x4(
            (float)cos, 0, -(float)sin, 0,
            0, 1, 0, 0,
            (float)sin, 0, (float)cos, 0,
            0, 0, 0, 1
        );
    }

    public static Matrix4x4 PitchTransform(float Pitch)
    {
        (double sin, double cos) = Math.SinCos(Pitch);
        return new Matrix4x4(
            1, 0, 0, 0,
            0, (float)cos, (float)sin, 0,
            0, (float)-sin, (float)cos, 0,
            0, 0, 0, 1
        );
    }

    public static Matrix4x4 RollTransform(float Roll)
    {
        (double sin, double cos) = Math.SinCos(Roll);
        return new Matrix4x4(
            (float)cos, (float)-sin, 0, 0,
            (float)sin, (float)cos, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        );
    }

    public static Matrix4x4 CameraPosTransform(Vertex CameraPos)
    {
        return new Matrix4x4(
            1, 0, 0, CameraPos.x,
            0, 1, 0, CameraPos.y,
            0, 0, 1, CameraPos.z,
            0, 0, 0, 1
        );
    }

    public static int CacheHits = 0;
    public static int CacheMisses = 0;

    public static Matrix4x4 GetTransform()
    {
        return CameraPosTransform(CameraPos) *
                HeadingTransform(Heading) *
                PitchTransform(Pitch) *
                RollTransform(Roll);

        // Matrix4x4 transform;
        // if (!TransformTable.TryGetValue((Heading, Pitch, Roll), out transform))
        // {
        //     transform = PanOutTransform() * 
        //         HeadingTransform(Heading) *
        //         PitchTransform(Pitch) *
        //         RollTransform(Roll);
        //     TransformTable.Add((Heading, Pitch, Roll), transform);
        //     CacheMisses++;
        // }
        // else CacheHits++;
        // return transform;
    }

    public static Dictionary<(float, float, float), Matrix4x4> TransformTable = new();

    public static void UpdateCamera() {
        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
        {
            var delta = Raylib.GetMouseDelta();

            Heading += -delta.X * (float)Math.PI / Raylib.GetScreenWidth();
            Pitch += delta.Y * (float)Math.PI / Raylib.GetScreenHeight();
            // Pitch += (float)Math.Cos(Heading) * (delta.Y * (float)Math.PI / Raylib.GetScreenHeight());
            // Roll += -(float)Math.Sin(Heading) * (delta.Y * (float)Math.PI / Raylib.GetScreenHeight());

        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
        	CameraPos += CameraFacing * MovementSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
        	CameraPos -= CameraFacing * MovementSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        	CameraPos -= CameraFacing.Cross(new Vertex(0, 1, 0)) * MovementSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        	CameraPos += CameraFacing.Cross(new Vertex(0, 1, 0)) * MovementSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
        	CameraPos -= new Vertex(0, 1, 0) * MovementSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
        	CameraPos += new Vertex(0, 1, 0) * MovementSpeed;
        
    }

    public static Color GetColor(Color color, double shade)
    {
        shade = Math.Max(0.1, shade);
        var redLinear = Math.Pow(color.r, 2.2) * shade;
        var greenLinear = Math.Pow(color.g, 2.2) * shade;
        var blueLinear = Math.Pow(color.b, 2.2) * shade;

        int red = (int)Math.Pow(redLinear, 1 / 2.2);
        int green = (int)Math.Pow(greenLinear, 1 / 2.2);
        int blue = (int)Math.Pow(blueLinear, 1 / 2.2);

        return new Color(red, green, blue, 255);
    }
}