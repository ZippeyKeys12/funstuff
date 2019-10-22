namespace Noise
{
    public static class Mathg
    {
        public static Generator Min(params Generator[] generators)
        {
            return new Function(
                (x, f) =>
                {
                    var min = Sample1D.MaxValue;

                    foreach (var op in generators)
                    {
                        min = Maths.Min(min, op.Get(x, f));
                    }

                    return min;
                },
                (xy, f) =>
                {
                    var min = Sample2D.MaxValue;

                    foreach (var op in generators)
                    {
                        min = Maths.Min(min, op.Get(xy, f));
                    }

                    return min;
                },
                (xyz, f) =>
                {
                    var min = Sample3D.MaxValue;

                    foreach (var op in generators)
                    {
                        min = Maths.Min(min, op.Get(xyz, f));
                    }

                    return min;
                });
        }

        public static Generator Max(params Generator[] generators)
        {
            return new Function(
                (x, f) =>
                {
                    var max = Sample1D.MinValue;

                    foreach (var op in generators)
                    {
                        max = Maths.Max(max, op.Get(x, f));
                    }

                    return max;
                },
                (xy, f) =>
                {
                    var max = Sample2D.MinValue;

                    foreach (var op in generators)
                    {
                        max = Maths.Max(max, op.Get(xy, f));
                    }

                    return max;
                },
                (xyz, f) =>
                {
                    var max = Sample3D.MinValue;

                    foreach (var op in generators)
                    {
                        max = Maths.Max(max, op.Get(xyz, f));
                    }

                    return max;
                });
        }

        public static Generator Mean(params Generator[] generators)
        {
            return new Function(
                (x, f) =>
                {
                    var sum = Sample1D.Zero;

                    foreach (var op in generators)
                    {
                        sum += op.Get(x, f);
                    }

                    return sum / generators.Length;
                },
                (xy, f) =>
                {
                    var sum = Sample2D.Zero;

                    foreach (var op in generators)
                    {
                        sum += op.Get(xy, f);
                    }

                    return sum / generators.Length;
                },
                (xyz, f) =>
                {
                    var sum = Sample3D.Zero;

                    foreach (var op in generators)
                    {
                        sum += op.Get(xyz, f);
                    }

                    return sum / generators.Length;
                });
        }
    }
}