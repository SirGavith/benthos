using System.Numerics;
using Raylib_cs;
namespace Benthos;

static class RenderInfo
{
    public static Vector CameraPos = new (0, 0, -10);
    public static Vector2 CameraFacing = new (0, -1);
    public static float Heading = 0;
    public static float Pitch = 0;

    public static float MovementSpeed = 0.05f;


    // public static float Roll = 0;

    public static Matrix4x4 HeadingTransform(Vector2 CameraFacing)
    {
        return new Matrix4x4(
            CameraFacing.Y, 0, -CameraFacing.X, 0,
            0, 1, 0, 0,
            CameraFacing.X, 0, CameraFacing.Y, 0,
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

    public static Matrix4x4 CameraPosTransform(Vector CameraPos)
    {
        return new Matrix4x4(
            1, 0, 0, CameraPos.X,
            0, 1, 0, CameraPos.Y,
            0, 0, 1, CameraPos.Z,
            0, 0, 0, 1
        );
    }

    public static int CacheHits = 0;
    public static int CacheMisses = 0;

    public static Matrix4x4 GetTransform()
    {
        return  
                HeadingTransform(CameraFacing) *
                // RollTransform(Roll) *
                CameraPosTransform(CameraPos) *
                PitchTransform(Pitch);

        // return (((CameraPosTransform(CameraPos) *
        //         HeadingTransform(Heading)) *
        //         PitchTransform(Pitch)) *
        //         RollTransform(Roll));

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

            var dHeading = -delta.X * (float)Math.PI / Raylib.GetScreenWidth();
            // Heading += dHeading;

            (double sin, double cos) = Math.SinCos(Heading);
            CameraFacing.X = (float)sin;
            CameraFacing.Y = -(float)cos;

            Pitch = (float)Math.Clamp(Pitch + delta.Y * (float)Math.PI / Raylib.GetScreenHeight(), -Math.PI / 2, Math.PI / 2);
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
        {
            CameraPos.X += CameraFacing.X * MovementSpeed;
            CameraPos.Z += CameraFacing.Y * MovementSpeed;
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
        {
            CameraPos.X -= CameraFacing.X * MovementSpeed;
            CameraPos.Z -= CameraFacing.Y * MovementSpeed;
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            CameraPos.X += -CameraFacing.Y * MovementSpeed;
            CameraPos.Z += CameraFacing.X * MovementSpeed;
        }
        	// CameraPos -= CameraFacing.Cross(new Vector(0, 1, 0)) * MovementSpeed;

        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            CameraPos.X += CameraFacing.Y * MovementSpeed;
            CameraPos.Z += -CameraFacing.X * MovementSpeed;
        }
        	// CameraPos += CameraFacing.Cross(new Vector(0, 1, 0)) * MovementSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
        	CameraPos.Y -= MovementSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
        	CameraPos.Y += MovementSpeed;
        
    }

    public static Color GetColor(Color color, double shade)
    {
        shade = Math.Max(0.02, shade);
        var redLinear = Math.Pow(color.r, 2.2) * shade;
        var greenLinear = Math.Pow(color.g, 2.2) * shade;
        var blueLinear = Math.Pow(color.b, 2.2) * shade;

        int red = (int)Math.Pow(redLinear, 1 / 2.2);
        int green = (int)Math.Pow(greenLinear, 1 / 2.2);
        int blue = (int)Math.Pow(blueLinear, 1 / 2.2);

        return new Color(red, green, blue, 255);
    }

    public static Color GetRandColor()
    {
        var rng = new Random();
        return new Color((byte)(rng.NextDouble() * 255), (byte)(rng.Next() * 255), (byte)(rng.Next() * 255), (byte)255);
    }
}