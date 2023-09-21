// x indicates the movement in the left and right directions
// y indicates the up-and-down movement on the screen
// z indicates depth (so the z axis is perpendicular to your screen). Positive z means "towards the observer".
public class Vertex
{
    public float x;
    public float y;
    public float z;
    public Vertex(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Vertex operator +(Vertex a, Vertex b)
    {
        return new Vertex(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static float SameSide(Vertex A, Vertex B, float Px, float Py)
    {
        //var V1V2 = new Vertex(B.x - A.x, B.y - A.y, B.z - A.z);
        //var V1P = new Vertex(this.x - A.x, this.y - A.y, this.z - A.z);
        //only care about z component of cross prod
        return (B.x - A.x) * (Py - A.y) - (B.y - A.y) * (Px - A.x);
    }
}