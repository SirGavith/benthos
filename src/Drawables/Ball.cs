using Raylib_cs;

namespace Benthos;

public class Ball : IDrawable
{
    Vector _pos;
    public Vector Pos
    {
        get => _pos;
        set {
            _pos = value;
            RegenTriangles();
        }
    }
    public byte _order = 3;
    public byte Order
    {
        get => _order;
        set 
        {
            _order = value;
            RegenTriangles();
        }
    }
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

    public Ball(Vector pos, float radius, Color color)
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
                new Vector(-1, -1, 1),
                new Vector(1, 1, 1),
                new Vector(-1, 1, -1), Color.BLUE),
            new (
                new Vector(1, 1, 1),
                new Vector(-1, -1, 1),
                new Vector(1, -1, -1), Color.GREEN),
            new (
                new Vector(1, -1, -1),
                new Vector(-1, 1, -1),
                new Vector(1, 1, 1), Color.RED),
            new (
                new Vector(-1, 1, -1),
                new Vector(1, -1, -1),
                new Vector(-1, -1, 1), Color.WHITE)
        };

        for (var i = 0; i < Order; i++)
        {
            var inflated = new List<Triangle>();
            foreach (var t in Triangles)
            {
                var m1 = (t.V1 + t.V2) / 2;
                var m2 = (t.V2 + t.V3) / 2;
                var m3 = (t.V3 + t.V1) / 2;
                inflated.Add(new Triangle(t.V1, m1, m3, Color));
                inflated.Add(new Triangle(m1, t.V2, m2, Color));
                inflated.Add(new Triangle(m2, t.V3, m3, Color));
                inflated.Add(new Triangle(m1,  m2,  m3, Color));
            }
            foreach (var t in inflated)
            {
                foreach (var v in new Vector[] { t.V1, t.V2, t.V3 })
                {
                    float l = (float)Math.Sqrt((v.X * v.X + v.Y * v.Y + v.Z * v.Z) / (3 * Radius * Radius));
                    v.X /= l;
                    v.Y /= l;
                    v.Z /= l;
                }
            }

            Triangles = inflated;
        }

        foreach (var t in Triangles)
        {
            t.V1 += Pos;
            t.V2 += Pos;
            t.V3 += Pos;
        }
    }

    public List<Triangle> GetTriangles()
    {
        return Triangles;
    }
}