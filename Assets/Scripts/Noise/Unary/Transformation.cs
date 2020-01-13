using Unity.Mathematics;

namespace Noise
{
    public class Transformation : Generator
    {
        public Generator gen;
        public float2x2 transform1d;
        public float3x3 transform2d;
        public float4x4 transform3d;

        public Transformation(Generator gen, float2x2 transform1d, float3x3 transform2d, float4x4 transform3d)
        {
            this.gen = gen;
            this.transform1d = math.inverse(transform1d);
            this.transform2d = math.inverse(transform2d);
            this.transform3d = math.inverse(transform3d);
        }

        public override Sample<float> Get(float x, float frequency)
        {
            return gen.Get(math.mul(transform1d, new float2(x, 1)).x, frequency);
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            return gen.Get(math.mul(transform2d, new float3(xy, 1)).xy, frequency);
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            return gen.Get(math.mul(transform3d, new float4(xyz, 1)).xyz, frequency);
        }
    }
}