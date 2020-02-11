using System.Linq;
using Unity.Mathematics;

namespace Noise
{
    public class Max : Generator
    {
        protected readonly Generator[] operands;

        public Max(params Generator[] operands)
        {
            this.operands = operands;
        }

        public override Sample<float> Get(float x)
        {
            return Maths.Max(operands.Select(i => i.Get(x)).ToArray());
        }

        public override Sample<float2> Get(float2 xy)
        {
            return Maths.Max(operands.Select(i => i.Get(xy)).ToArray());
        }

        public override Sample<float3> Get(float3 xyz)
        {
            return Maths.Max(operands.Select(i => i.Get(xyz)).ToArray());
        }
    }
}