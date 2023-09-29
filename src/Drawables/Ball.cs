using Raylib_cs;

namespace raylibTest;

public class Ball : IDrawable
{
    Vertex _pos;
    public Vertex Pos
    {
        get => _pos;
        set {
            _pos = value;
            RegenTriangles();
        }
    }
    public byte Order = 3;
    float _radius;
    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value;
            RegenTriangles();
        }
    }
    Color _color;
    public Color Color {
        get => _color;
        set {
            _color = value;
            RegenTriangles();
        }
    }

    List<Triangle> Triangles = new ();


    public Ball(Vertex pos, float radius, Color color)
    {
        _pos = pos;
        _radius = radius;
        _color = color;
        RegenTriangles();
    }

    public void RegenTriangles()
    {
        Triangles = new List<Triangle>()
        {
            new (
                new Vertex(-1, -1, 1),
                new Vertex(1, 1, 1),
                new Vertex(-1, 1, -1), Color.BLUE),
            new (
                new Vertex(1, 1, 1),
                new Vertex(-1, -1, 1),
                new Vertex(1, -1, -1), Color.GREEN),
            new (
                new Vertex(1, -1, -1),
                new Vertex(-1, 1, -1),
                new Vertex(1, 1, 1), Color.RED),
            new (
                new Vertex(-1, 1, -1),
                new Vertex(1, -1, -1),
                new Vertex(-1, -1, 1), Color.WHITE)
        };

        for (var i = 0; i < Order; i++)
        {
            var inflated = new List<Triangle>();
            foreach (var t in Triangles)
            {
                var m1 = (t.v1 + t.v2) / 2;
                var m2 = (t.v2 + t.v3) / 2;
                var m3 = (t.v3 + t.v1) / 2;
                inflated.Add(new Triangle(t.v1, m1, m3, Color));
                inflated.Add(new Triangle(t.v2, m1, m2, Color));
                inflated.Add(new Triangle(t.v3, m2, m3, Color));
                inflated.Add(new Triangle(m1, m2, m3, Color));
            }
            foreach (var t in inflated)
            {
                foreach (var v in new Vertex[] { t.v1, t.v2, t.v3 })
                {
                    float l = (float)Math.Sqrt((v.x * v.x + v.y * v.y + v.z * v.z) / (3 * Radius * Radius));
                    v.x /= l;
                    v.y /= l;
                    v.z /= l;
                }
            }

            Triangles = inflated;
        }

        foreach (var t in Triangles)
        {
            t.v1 += Pos;
            t.v2 += Pos;
            t.v3 += Pos;
        }
    }

    public List<Triangle> GetTriangles()
    {
        return Triangles;
    }
}