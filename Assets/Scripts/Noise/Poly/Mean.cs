using Unity.Mathematics;

namespace Noise
{
    public class Mean : Generator
    {
        protected readonly Generator[] operands;

        public Mean(params Generator[] operands)
        {
            this.operands = operands;
        }

        public override Sample<float> Get(float x, float frequency)
        {
            var sum = Sample<float>.Zero;

            foreach (var op in operands)
            {
                sum += op.Get(x, frequency);
            }

            return sum / operands.Length;
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            var sum = Sample<float2>.Zero;

            foreach (var op in operands)
            {
                sum += op.Get(xy, frequency);
            }

            return sum / operands.Length;
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            var sum = Sample<float3>.Zero;

            foreach (var op in operands)
            {
                sum += op.Get(xyz, frequency);
            }

            return sum / operands.Length;
        }
    }
}