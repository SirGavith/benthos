using Raylib_cs;
namespace raylibTest;

partial class Program
{
    public static short[] zBuffer = new short[Raylib.GetScreenWidth() * Raylib.GetScreenHeight()];

    public static void Draw()
    {
        var window = new Rect(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        var ScreenMiddle = new Vertex(window.Width / 2, window.Height / 2, 0);
        var transform = RenderInfo.GetTransform();

        Raylib.ClearBackground(new Color(60, 20, 20, 255));

        var box = new Box(new Vertex(-50, -50, -50), new Vertex (100, 100, 100), Color.GOLD);


        for (var i = 0; i < zBuffer.Length; i++)
            zBuffer[i] = short.MinValue;

        foreach (Triangle t in box.GetTriangles())
        {
            Vertex v1 = transform.Transform(t.v1) + ScreenMiddle;
            Vertex v2 = transform.Transform(t.v2) + ScreenMiddle;
            Vertex v3 = transform.Transform(t.v3) + ScreenMiddle;
            var depth = (short)(v1.z + v2.z + v3.z);

            int minX = Math.Max(0,              (int)Math.Min(v1.x, Math.Min(v2.x, v3.x)));
            int maxX = Math.Min(window.Width,   (int)Math.Max(v1.x, Math.Max(v2.x, v3.x)));
            int minY = Math.Max(0,              (int)Math.Min(v1.y, Math.Min(v2.y, v3.y)));
            int maxY = Math.Min(window.Height,  (int)Math.Max(v1.y, Math.Max(v2.y, v3.y)));


            for (int y = minY; y < maxY; y++)
            {
                var rowIndex = y * window.Width;

                for (int x = minX; x < maxX; x++)
                {
                    var sign1 = Vertex.SameSide(v1, v2, x, y) < 0;
                    var sign2 = Vertex.SameSide(v2, v3, x, y) < 0;
                    var sign3 = Vertex.SameSide(v3, v1, x, y) < 0;

                    if (sign1 == sign2 && sign2 == sign3 && zBuffer[rowIndex + x] < depth)
                    {
                        Raylib.DrawPixel(x, y, t.color);

                        zBuffer[rowIndex + x] = depth;
                    }
                }
            }
        }
    }
}