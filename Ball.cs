using Raylib_cs;

namespace raylibTest;

public class Ball : IDrawable
{
    public Vertex Pos { get; set; }
    public float Radius;
    public byte Order = 5;
    Color _color;
    public Color Color {
        get => _color;
        set {
            _color = value;
            foreach (var t in Triangles)
                t.Color = value;
        }
    }


    public Ball(Vertex pos, float radius, Color color)
    {
        Pos = pos;
        Radius = radius;
        Color = color;
    }

    //TODO: rework how triangles are stored / when they are generated for easy changing of radius and color
    private List<Triangle> Triangles = new()
        {
            new Triangle(
                    new Vertex(100, 100, 100),
                    new Vertex(-100, -100, 100),
                    new Vertex(-100, 100, -100), Color.WHITE),
            new Triangle(
                    new Vertex(100, 100, 100),
                    new Vertex(-100, -100, 100),
                    new Vertex(100, -100, -100), Color.WHITE),
            new Triangle(
                    new Vertex(-100, 100, -100),
                    new Vertex(100, -100, -100),
                    new Vertex(100, 100, 100), Color.WHITE),
            new Triangle(
                    new Vertex(-100, 100, -100),
                    new Vertex(100, -100, -100),
                    new Vertex(-100, -100, 100), Color.WHITE)
        };

    public List<Triangle> GetTriangles()
    {
        for (var i = 0; i < Order; i++)
        {
            var inflated = new List<Triangle>();
            foreach (var t in Triangles)
            {
                var m1 = (t.v1 + t.v2) / 2;
                var m2 = (t.v2 + t.v3) / 2;
                var m3 = (t.v3 + t.v1) / 2;
                inflated.Add(new Triangle(t.v1, m1, m3, t.Color));
                inflated.Add(new Triangle(t.v2, m1, m2, t.Color));
                inflated.Add(new Triangle(t.v3, m2, m3, t.Color));
                inflated.Add(new Triangle(m1, m2, m3,   t.Color));

            }
            foreach (var t in inflated)
            {
                foreach (var v in new Vertex[] { t.v1, t.v2, t.v3 })
                {
                    float l = (float)(Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z) / Math.Sqrt(30000));
                    v.x /= l;
                    v.y /= l;
                    v.z /= l;
                }
            }

            Triangles = inflated;
        }
        return Triangles;
    }
}