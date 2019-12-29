using System;
using UnityEngine;
using Unity.Mathematics;

namespace Noise
{
    public abstract class Map : Generator
    {
        protected readonly Generator gen;

        public Map(Generator gen)
        {
            this.gen = gen;
        }

        protected abstract Sample<T> TransformFunc<T>(Sample<T> a);

        public override Sample<float> Get(float x, float frequency)
        {
            return TransformFunc(gen.Get(x, frequency));
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            return TransformFunc(gen.Get(xy, frequency));
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            return TransformFunc(gen.Get(xyz, frequency));
        }
    }
}