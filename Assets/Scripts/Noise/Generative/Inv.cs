using System;
using Unity.Mathematics;

namespace Noise.Generative
{
    [System.Serializable]
    public class InvDistNoise : Generator
    {
        protected Sampler sampler;

        public InvDistNoise(Sampler sampler)
        {
            this.sampler = sampler;
        }

        public override Sample<float> Get(float x)
        {
            throw new NotImplementedException();
        }

        public override Sample<float2> Get(float2 xy)
        {
            throw new NotImplementedException();
        }

        public override Sample<float3> Get(float3 xyz)
        {
            throw new NotImplementedException();
        }
    }
}