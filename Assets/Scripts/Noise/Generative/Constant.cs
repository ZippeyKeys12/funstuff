﻿using UnityEngine;
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
            => (Sample1D)a;


        public override Sample<float2> Get(float x, float y, float frequency)
            => (Sample2D)a;


        public override Sample<float3> Get(float x, float y, float z, float frequency)
            => (Sample3D)a;

    }
}