using Unity.Mathematics;

namespace Noise
{
    public abstract class Sampler
    {
        public abstract float[] GetPoints(float x, float freq, int size, int R);
        public abstract float2[] GetPoints(float2 xy, float freq, int2 size, int R);
        public abstract float3[] GetPoints(float3 xyz, float freq, int3 size, int R);
    }
}