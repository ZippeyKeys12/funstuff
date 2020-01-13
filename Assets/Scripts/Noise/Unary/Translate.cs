using Unity.Mathematics;

namespace Noise
{
    public class Translate : Transformation
    {
        public Translate(Generator gen, float transformX, float transformY, float transformZ)
            : base(gen,
                new float2x2(0, transformX, 0, 1),
                new float3x3(0, 0, transformX, 0, 0, transformY, 0, 0, 1),
                new float4x4(0, 0, 0, transformX, 0, 0, 0, transformY, 0, 0, 0, transformZ, 0, 0, 0, 1))
        { }
    }
}