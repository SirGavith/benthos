using Raylib_cs;
namespace raylibTest;
//https://www.alibabacloud.com/blog/construct-a-simple-3d-rendering-engine-with-java_599599

partial class Program
{
    public static void Main()
    {
        Raylib.InitWindow(800, 480, "Hello World");
        Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);

        while (!Raylib.WindowShouldClose())
        {
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
            {
                var delta = Raylib.GetMouseDelta();
                RenderInfo.Heading +=  -delta.X * (float)Math.PI / Raylib.GetScreenWidth();
                RenderInfo.Pitch += delta.Y * (float)Math.PI / Raylib.GetScreenHeight();
            }

            Raylib.BeginDrawing(); 
            
            Draw();

            Raylib.EndDrawing();

            Console.WriteLine($"Frametime: {Raylib.GetFrameTime() * 1000}ms");
        }
        Raylib.CloseWindow();
    }
}