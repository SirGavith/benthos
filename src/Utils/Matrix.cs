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

    public Vector Transform(Vector v)
    {
        return new Vector(
            Values[0] * v.X + Values[1] * v.Y + Values[2] * v.Z,
            Values[3] * v.X + Values[4] * v.Y + Values[5] * v.Z,
            Values[6] * v.X + Values[7] * v.Y + Values[8] * v.Z);
    }

}
