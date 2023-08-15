using Unity.Mathematics;

namespace Noise.Unary
{
    public sealed class Translation : Generator
    {
        private readonly Generator gen;
        private readonly float3 translation;

        public Translation(Generator gen, float3 translation)
        {
            this.gen = gen;
            this.translation = translation;
        }

        public override Sample<float> Get(float x)
            => gen.Get(x - translation.x);

        public override Sample<float2> Get(float2 xy)
            => gen.Get(xy - translation.xy);

        public override Sample<float3> Get(float3 xyz)
            => gen.Get(xyz - translation);
    }
}