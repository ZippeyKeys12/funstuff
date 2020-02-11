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

        public override Sample<float> Get(float x)
        {
            return TransformFunc(gen.Get(x));
        }

        public override Sample<float2> Get(float2 xy)
        {
            return TransformFunc(gen.Get(xy));
        }

        public override Sample<float3> Get(float3 xyz)
        {
            return TransformFunc(gen.Get(xyz));
        }
    }
}