using Unity.Mathematics;

namespace Noise
{
    public class SawtoothNoise : Generator
    {
        public SawtoothNoise()
        { }

        public override Sample<float> Get(float x)
        {
            return new Sample<float>(x - math.floor(x), 1);
        }

        public override Sample<float2> Get(float2 xy)
        {
            return new Sample<float2>(math.dot(xy - math.floor(xy), 1), 1);
        }

        public override Sample<float3> Get(float3 xyz)
        {
            return new Sample<float3>(math.dot(xyz - math.floor(xyz), 1), 1);
        }
    }
}