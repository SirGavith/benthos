public class Matrix3x3
{
    public float[] Values = new float[9];

    public Matrix3x3() { }
    public Matrix3x3(float[] values)
    {
        Values = values;
    }

    public Matrix3x3 Multiply(Matrix3x3 matrix)
    {
        var ret = new Matrix3x3();
        for (byte i = 0; i < 9; i++)
        {
            for (byte j = 0; j < 3; j++)
            {
                ret.Values[i] += this.Values[(i / 3) * 3 + j] * matrix.Values[(j * 3) + (i % 3)];
            }
        }
        return ret;
    }

    public Boolean Equals(Matrix3x3 other)
    {
        return Enumerable.SequenceEqual(Values, other.Values);
    }

    public Vertex Transform(Vertex v)
    {
        return new Vertex(
            Values[0] * v.x + Values[1] * v.y + Values[2] * v.z,
            Values[3] * v.x + Values[4] * v.y + Values[5] * v.z,
            Values[6] * v.x + Values[7] * v.y + Values[8] * v.z);
    }

}