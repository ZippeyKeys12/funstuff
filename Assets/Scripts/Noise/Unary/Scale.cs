using Unity.Mathematics;

namespace Noise
{
    public class Scale : Generator
    {
        float scalingFactor;
        Generator gen;

        public Scale(Generator gen, float scalingFactor)
        {
            this.gen = gen;
            this.scalingFactor = scalingFactor;
        }

        public override Sample<float> Get(float x, float frequency)
        {
            return gen.Get(x / scalingFactor, frequency);
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            return gen.Get(xy / scalingFactor, frequency);
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            return gen.Get(xyz / scalingFactor, frequency);
        }
    }
}