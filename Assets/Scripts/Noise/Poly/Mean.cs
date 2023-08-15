using Unity.Mathematics;

namespace Noise.Poly
{
    public class Mean : Generator
    {
        protected readonly Generator[] operands;

        public Mean(params Generator[] operands)
        {
            this.operands = operands;
        }

        public override Sample<float> Get(float x)
        {
            var sum = Sample<float>.Zero;

            foreach (var op in operands)
            {
                sum += op.Get(x);
            }

            return sum / operands.Length;
        }

        public override Sample<float2> Get(float2 xy)
        {
            var sum = Sample<float2>.Zero;

            foreach (var op in operands)
            {
                sum += op.Get(xy);
            }

            return sum / operands.Length;
        }

        public override Sample<float3> Get(float3 xyz)
        {
            var sum = Sample<float3>.Zero;

            foreach (var op in operands)
            {
                sum += op.Get(xyz);
            }

            return sum / operands.Length;
        }
    }
}