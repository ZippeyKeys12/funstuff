using System;
using UnityEngine;
using Unity.Mathematics;

namespace Noise
{
    public class Function : Generator
    {
        public delegate Sample<T> MapFunc<T>(T t, float f);

        protected readonly MapFunc<float> func_a;
        protected readonly MapFunc<float2> func_b;
        protected readonly MapFunc<float3> func_c;

        public Function(MapFunc<float> func_a, MapFunc<float2> func_b, MapFunc<float3> func_c)
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