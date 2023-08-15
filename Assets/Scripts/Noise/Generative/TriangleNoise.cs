using Unity.Mathematics;

namespace Noise.Generative
{
    public class TriangleNoise : Generator
    {
        public TriangleNoise()
        { }

        public override Sample<float> Get(float x)
        {
            var saw = x - math.floor(x + .5f);
            return 2 * new Sample<float>(math.abs(saw), math.sign(saw));
        }

        public override Sample<float2> Get(float2 xy)
        {
            var saw = xy - math.floor(xy + .5f);

            return 2 * new Sample<float2>(math.dot(math.abs(saw), 1), math.sign(saw));
        }

        public override Sample<float3> Get(float3 xyz)
        {
            var saw = xyz - math.floor(xyz + .5f);
            return 2 * new Sample<float3>(math.dot(math.abs(saw), 1), math.sign(saw));
        }
    }
}