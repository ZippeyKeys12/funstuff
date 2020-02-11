using System.Linq;
using Unity.Mathematics;

namespace Noise
{
    public class Min : Generator
    {
        protected readonly Generator[] operands;

        public Min(params Generator[] operands)
        {
            this.operands = operands;
        }

        public override Sample<float> Get(float x)
        {
            return Maths.Min(operands.Select(i => i.Get(x)).ToArray());
        }

        public override Sample<float2> Get(float2 xy)
        {
            return Maths.Min(operands.Select(i => i.Get(xy)).ToArray());
        }

        public override Sample<float3> Get(float3 xyz)
        {
            return Maths.Min(operands.Select(i => i.Get(xyz)).ToArray());
        }
    }
}