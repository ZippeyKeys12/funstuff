using UnityEngine;
using Unity.Mathematics;

namespace Noise
{
    public class Constant : Generator
    {
        private readonly float a;

        public Constant(float a)
        {
            this.a = a;
        }

        public override Sample<float> Get(float x, float frequency)
            => (Sample<float>)a;


        public override Sample<float2> Get(float2 xy, float frequency)
            => (Sample<float2>)a;


        public override Sample<float3> Get(float3 xyz, float frequency)
            => (Sample<float3>)a;

    }
}