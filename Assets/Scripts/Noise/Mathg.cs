using Unity.Mathematics;

namespace Noise
{
    public static class Mathg
    {
        public static Generator Mean(params Generator[] generators)
        {
            return new Function(
                (x, f) =>
                {
                    var sum = Sample<float>.Zero;

                    foreach (var op in generators)
                    {
                        sum += op.Get(x, f);
                    }

                    return sum / generators.Length;
                },
                (xy, f) =>
                {
                    var sum = Sample<float2>.Zero;

                    foreach (var op in generators)
                    {
                        sum += op.Get(xy, f);
                    }

                    return sum / generators.Length;
                },
                (xyz, f) =>
                {
                    var sum = Sample<float3>.Zero;

                    foreach (var op in generators)
                    {
                        sum += op.Get(xyz, f);
                    }

                    return sum / generators.Length;
                });
        }
    }
}