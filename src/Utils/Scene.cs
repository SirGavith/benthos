using System.Diagnostics;
using Raylib_cs;
namespace Benthos;

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

    public Vector LightDirection = new(0, 0.707f, -0.707f);

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


    double[] zBuffer = Array.Empty<double>();

    public void Draw()
    {
        var window = new Rect(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        var transform = RenderInfo.GetTransform();

        if (zBuffer.Length != window.Width * window.Height)
            zBuffer = new double[window.Width * window.Height];

        double fovAngle = Math.PI / 2;
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
                var angleCos = Math.Max(normal.Dot(LightDirection) / normal.Magnitude(), 0);
                // var angleCos = Math.Abs(normal.z / normal.Magnitude());

                Vector4 v1 = t.v1.ToVector4() * transform;
                Vector4 v2 = t.v2.ToVector4() * transform;
                Vector4 v3 = t.v3.ToVector4() * transform;

                //backface culling
                if (Vector4.Norm(v2 - v1, v3 - v1).Z < 0)
                    continue;

                // var depth = (short)(v1.z + v2.z + v3.z);


                //divide by z to transform into screen space
                //divide by  fov to turn into screen coordinate space

                v1.X *= - 1 / (v1.Z * fov);
                v1.Y *= - 1 / (v1.Z * fov);
                v2.X *= - 1 / (v2.Z * fov);
                v2.Y *= - 1 / (v2.Z * fov);
                v3.X *= - 1 / (v3.Z * fov);
                v3.Y *= - 1 / (v3.Z * fov);

                // add 0.5 to transform into NCD space
                v1.X += 0.5f;
                v1.Y += 0.5f;
                v2.X += 0.5f;
                v2.Y += 0.5f;
                v3.X += 0.5f;
                v3.Y += 0.5f;

                //mult by screen size to transform into raster space

                v1.X *= window.Width;
                v1.Y *= window.Width;
                v2.X *= window.Width;
                v2.Y *= window.Width;
                v3.X *= window.Width;
                v3.Y *= window.Width;

                var color = RenderInfo.GetColor(t.Color, angleCos);

                // Raylib.DrawTriangle(v1.ToVector2(), v2.ToVector2(), v3.ToVector2(), color);


                int minX = Math.Max(0, (int)Math.Min(v1.X, Math.Min(v2.X, v3.X)));
                int maxX = Math.Min(window.Width, (int)Math.Max(v1.X, Math.Max(v2.X, v3.X)));
                int minY = Math.Max(0, (int)Math.Min(v1.Y, Math.Min(v2.Y, v3.Y)));
                int maxY = Math.Min(window.Height, (int)Math.Max(v1.Y, Math.Max(v2.Y, v3.Y)));

                double triangleArea = (v1.Y - v3.Y) * (v2.X - v3.X) + (v2.Y - v3.Y) * (v3.X - v1.X);


                for (int y = minY; y < maxY; y++)
                {
                    var rowIndex = y * window.Width;
                    for (int x = minX; x < maxX; x++)
                    {
                        double b1 = ((y - v3.Y) * (v2.X - v3.X) + (v2.Y - v3.Y) * (v3.X - x)) / triangleArea;
                        double b2 = ((y - v1.Y) * (v3.X - v1.X) + (v3.Y - v1.Y) * (v1.X - x)) / triangleArea;
                        double b3 = ((y - v2.Y) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v2.X - x)) / triangleArea;

                        if (b1 >= 0 && b1 <= 1 && b2 >= 0 && b2 <= 1 && b3 >= 0 && b3 <= 1)
                        {
                            // double depth = b1 * v1.z + b2 * v2.z + b3 * v3.z;

                            var depth = (short)(v1.Z + v2.Z + v3.Z);
                            if (zBuffer[rowIndex + x] < depth)
                            {
                                Raylib.DrawRectangle(x, y, 2, 2, color);

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