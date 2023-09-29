using System.Diagnostics;
using Raylib_cs;
namespace raylibTest;

public class Scene
{
    public string Name;
    public List<IDrawable> Drawables = new();
    public float ScaleFactor = 10;
    public Action<double> OnTick = (deltaT) => {};
    // public int TargetFPS = 60;
    public Stopwatch TotalTime = new();
    double LastFrameTime = 0;
    public bool Log = true;

    public Vertex LightDirection = new(0, 0.707f, -0.707f);

    public Scene(string name)
    {
        Name = name;
    }

    public void Play()
    {
        Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.InitWindow(1000, 600, Name);
        zBuffer = new double[Raylib.GetScreenWidth() * Raylib.GetScreenHeight()];
        // FrameTimer = new PeriodicTimer(TimeSpan.FromSeconds(1 / (double)TargetFPS));
        if (Log) Console.WriteLine($"Initialized window {Name}");
        // var timer = new Timer(Tick, null, 0, 2000);
        TotalTime.Start();
        while (!Raylib.WindowShouldClose())
        {
            Tick();
        }
        Console.WriteLine($"{RenderInfo.CacheHits} hits, {RenderInfo.CacheMisses} misses");
        Raylib.CloseWindow();
    }

    void Tick()
    {
        RenderInfo.UpdateCamera();
        OnTick(0.01);
        try
        {
            Draw();
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine("Draw failed");
        }
        
        // if (Log) Console.WriteLine($"Frametime: {Raylib.GetFrameTime() * 1000}ms");
        // LastFrameTime = TotalTime.Elapsed.TotalSeconds;
    }


    double[] zBuffer = new double[0];

    public void Draw()
    {
        var window = new Rect(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        var transform = RenderInfo.GetTransform();

        if (zBuffer.Length != window.Width * window.Height)
            zBuffer = new double[window.Width * window.Height];

        double fovAngle = Math.PI / 3;
        float fov = (float)Math.Tan(fovAngle / 2);

        Raylib.BeginDrawing();
        Raylib.ClearBackground(new Color(60, 20, 20, 255));

        for (var i = 0; i < zBuffer.Length; i++)
            zBuffer[i] = short.MinValue;


        foreach (var drawable in Drawables)
        {
            var triangles = drawable.GetTriangles();
            // if (Log) Console.WriteLine($"Drawing {triangles.Count} triangles");
            foreach (var t in triangles)
            {

                //transform into camera space

                var normal = (t.v2 - t.v1).Cross(t.v3 - t.v1);
                var angleCos = Math.Abs(normal.Dot(LightDirection) / normal.Magnitude());


                // var angleCos = Math.Abs(normal.z / normal.Magnitude());

                Vertex4 v1 = t.v1.ToVertex4() * transform;
                Vertex4 v2 = t.v2.ToVertex4() * transform;
                Vertex4 v3 = t.v3.ToVertex4() * transform;


                //backface culling maybe
                if (Vertex4.Norm(v2 - v1, v3 - v1).z < 0)
                    continue;

                // var depth = (short)(v1.z + v2.z + v3.z);

                

                //divide by z to transform into screen space
                //divide by  fov to turn into screen coordinate space

                v1.x *= - 1 / (v1.z * fov);
                v1.y *= - 1 / (v1.z * fov);
                v2.x *= - 1 / (v2.z * fov);
                v2.y *= - 1 / (v2.z * fov);
                v3.x *= - 1 / (v3.z * fov);
                v3.y *= - 1 / (v3.z * fov);

                // add 0.5 to transform into NCD space
                v1.x += 0.5f;
                v1.y += 0.5f;
                v2.x += 0.5f;
                v2.y += 0.5f;
                v3.x += 0.5f;
                v3.y += 0.5f;

                //mult by screen size to transform into raster space

                v1.x *= window.Width;
                v1.y *= window.Width;
                v2.x *= window.Width;
                v2.y *= window.Width;
                v3.x *= window.Width;
                v3.y *= window.Width;


                int minX = Math.Max(0, (int)Math.Min(v1.x, Math.Min(v2.x, v3.x)));
                int maxX = Math.Min(window.Width, (int)Math.Max(v1.x, Math.Max(v2.x, v3.x)));
                int minY = Math.Max(0, (int)Math.Min(v1.y, Math.Min(v2.y, v3.y)));
                int maxY = Math.Min(window.Height, (int)Math.Max(v1.y, Math.Max(v2.y, v3.y)));

                double triangleArea = (v1.y - v3.y) * (v2.x - v3.x) + (v2.y - v3.y) * (v3.x - v1.x);

                var color = RenderInfo.GetColor(t.Color, angleCos);

                for (int y = minY; y < maxY; y++)
                {
                    var rowIndex = y * window.Width;
                    for (int x = minX; x < maxX; x++)
                    {
                        double b1 = ((y - v3.y) * (v2.x - v3.x) + (v2.y - v3.y) * (v3.x - x)) / triangleArea;
                        double b2 = ((y - v1.y) * (v3.x - v1.x) + (v3.y - v1.y) * (v1.x - x)) / triangleArea;
                        double b3 = ((y - v2.y) * (v1.x - v2.x) + (v1.y - v2.y) * (v2.x - x)) / triangleArea;

                        if (b1 >= 0 && b1 <= 1 && b2 >= 0 && b2 <= 1 && b3 >= 0 && b3 <= 1)
                        {
                            // double depth = b1 * v1.z + b2 * v2.z + b3 * v3.z;

                            var depth = (short)(v1.z + v2.z + v3.z);
                            if (zBuffer[rowIndex + x] < depth)
                            {
                                Raylib.DrawPixel(x, y, color);
                                Raylib.DrawPixel(x, y + 1, color);
                                Raylib.DrawPixel(x + 1, y, color);
                                Raylib.DrawPixel(x + 1, y + 1, color);

                                zBuffer[rowIndex + x] = depth;
                            }
                        }
                    }
                }
            }
        }
        Raylib.EndDrawing();
    }
}