using Unity.Mathematics;

namespace Noise
{
    public class Translation : Transformation
    {
        public Translation(Generator gen, float3 translation)
            : base(gen,
                new float2x2(0, translation.x, 0, 1),
                new float3x3(0, 0, translation.x, 0, 0, translation.y, 0, 0, 1),
                float4x4.Translate(translation))
        { }
    }
}