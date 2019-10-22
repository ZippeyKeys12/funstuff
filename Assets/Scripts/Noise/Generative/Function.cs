using System;
using UnityEngine;
using Unity.Mathematics;

namespace Noise
{
    public class Function : Generator
    {
        protected readonly Func<float, float, Sample<float>> func_a;
        protected readonly Func<float2, float, Sample<float2>> func_b;
        protected readonly Func<float3, float, Sample<float3>> func_c;

        public Function(Func<float, float, Sample<float>> func_a, Func<float2, float, Sample<float2>> func_b, Func<float3, float, Sample<float3>> func_c)
        {
            this.func_a = func_a;
            this.func_b = func_b;
            this.func_c = func_c;
        }

        public override Sample<float> Get(float x, float frequency)
        {
            return func_a(x, frequency);
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            return func_b(xy, frequency);
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            return func_c(xyz, frequency);
        }
    }
}