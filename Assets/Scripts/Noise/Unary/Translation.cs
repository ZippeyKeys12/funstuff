using Unity.Mathematics;

namespace Noise
{
    public class Translation : Generator
    {
        protected Generator gen;
        protected float3 translation;

        public Translation(Generator gen, float3 translation)
        {
            this.gen = gen;
            this.translation = translation;
        }

        public override Sample<float> Get(float x, float frequency)
            => gen.Get(x - translation.x, frequency);

        public override Sample<float2> Get(float2 xy, float frequency)
            => gen.Get(xy - translation.xy, frequency);

        public override Sample<float3> Get(float3 xyz, float frequency)
            => gen.Get(xyz - translation, frequency);
    }
}