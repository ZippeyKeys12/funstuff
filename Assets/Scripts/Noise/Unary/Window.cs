using System.Collections.Generic;
using Unity.Mathematics;

namespace Noise
{
    public class Window : Generator
    {
        public delegate Sample<T> ReducingFunc<T>(params Sample<T>[] samples);

        protected Generator gen;

        protected int steps;
        protected float unit;
        protected ReducingFunc<float> func_a;
        protected ReducingFunc<float2> func_b;
        protected ReducingFunc<float3> func_c;

        public Window(Generator gen, float unit, int steps, ReducingFunc<float> func_a, ReducingFunc<float2> func_b, ReducingFunc<float3> func_c)
        {
            this.gen = gen;
            this.unit = unit;
            this.steps = steps;
            this.func_a = func_a;
            this.func_b = func_b;
            this.func_c = func_c;
        }

        public override Sample<float> Get(float x)
        {
            var samples = new List<Sample<float>>();
            for (var i = -steps; i < steps; i++)
            {
                samples.Add(gen.Get(x + (i * unit)));
            }

            return func_a(samples.ToArray());
        }

        public override Sample<float2> Get(float2 xy)
        {
            var samples = new List<Sample<float2>>();
            var ij = new float2(-steps);
            for (; ij.x < steps; ij.x++)
            {
                for (; ij.y < steps; ij.y++)
                {
                    samples.Add(gen.Get(xy + (ij * unit)));
                }
            }

            return func_b(samples.ToArray());
        }

        public override Sample<float3> Get(float3 xyz)
        {
            var samples = new List<Sample<float3>>();
            var ijk = new float3(-steps);
            for (; ijk.x < steps; ijk.x++)
            {
                for (; ijk.y < steps; ijk.y++)
                {
                    for (; ijk.z < steps; ijk.z++)
                    {
                        samples.Add(gen.Get(xyz + (ijk * unit)));
                    }
                }
            }

            return func_c(samples.ToArray());
        }
    }
}