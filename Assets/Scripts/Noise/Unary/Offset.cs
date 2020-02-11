using Unity.Mathematics;

namespace Noise
{
    public class Offset : Generator
    {
        float offset1d;
        float2 offset2d;
        float3 offset3d;
        Generator gen;

        public Offset(Generator gen, float offset1d, float2 offset2d, float3 offset3d)
        {
            this.gen = gen;
            this.offset1d = offset1d;
            this.offset2d = offset2d;
            this.offset3d = offset3d;
        }

        public override Sample<float> Get(float x)
            => gen.Get(x - offset1d);

        public override Sample<float2> Get(float2 xy)
            => gen.Get(xy - offset2d);

        public override Sample<float3> Get(float3 xyz)
            => gen.Get(xyz - offset3d);
    }
}