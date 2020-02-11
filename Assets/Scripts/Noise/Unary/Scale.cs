using Unity.Mathematics;

namespace Noise
{
    public sealed class Scale : Generator
    {
        private readonly float scalingFactor;
        private readonly Generator gen;

        public Scale(Generator gen, float scalingFactor)
        {
            this.gen = gen;
            this.scalingFactor = scalingFactor;
        }

        public override Sample<float> Get(float x)
        {
            return gen.Get(x / scalingFactor);
        }

        public override Sample<float2> Get(float2 xy)
        {
            return gen.Get(xy / scalingFactor);
        }

        public override Sample<float3> Get(float3 xyz)
        {
            return gen.Get(xyz / scalingFactor);
        }
    }
}