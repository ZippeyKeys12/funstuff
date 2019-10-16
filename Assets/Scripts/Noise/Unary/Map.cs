using System;
using UnityEngine;
using Unity.Mathematics;

namespace Noise
{
    public class Map : Generator
    {
        protected readonly Func<Sample<float>, Sample<float>> func_a;
        protected readonly Func<Sample<float2>, Sample<float2>> func_b;
        protected readonly Func<Sample<float3>, Sample<float3>> func_c;
        protected readonly Generator a;

        public Map(Generator a, Func<Sample<float>, Sample<float>> func_a, Func<Sample<float2>, Sample<float2>> func_b, Func<Sample<float3>, Sample<float3>> func_c)
        {
            this.a = a;
            this.func_a = func_a;
            this.func_b = func_b;
            this.func_c = func_c;
        }

        public override Sample<float> Get(float x, float frequency)
        {
            return func_a(a.Get(x, frequency));
        }

        public override Sample<float2> Get(float x, float y, float frequency)
        {
            return func_b(a.Get(x, y, frequency));
        }

        public override Sample<float3> Get(float x, float y, float z, float frequency)
        {
            return func_c(a.Get(x, y, z, frequency));
        }
    }
}