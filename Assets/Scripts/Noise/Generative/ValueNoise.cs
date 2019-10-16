﻿using System;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Random = System.Random;
using TweenType = Interp.TweenType;
using TweenTypes = Interp.TweenTypes;

namespace Noise
{
    [Serializable]
    public class ValueNoise : Generator
    {
        protected const int hashMask = 255;
        protected readonly int[] hash;
        protected readonly TweenType interp;

        public ValueNoise(int seed)
            : this(seed, TweenTypes.SmootherStep) { }

        public ValueNoise(int seed, TweenType interp)
        {
            var rnd = new Random(seed);
            var temp = Enumerable.Range(0, hashMask + 1).OrderBy(x => rnd.Next());
            hash = temp.Concat(temp).ToArray();
            this.interp = interp;
        }

        // Implementation thanks to Catlike Coding (https://catlikecoding.com/unity/tutorials/noise/)
        // Support them at: https://www.patreon.com/catlikecoding
        public override Sample<float> Get(float x, float frequency)
        {
            x *= frequency;

            int i0 = Mathf.FloorToInt(x);
            float t = x - i0;
            i0 &= hashMask;
            int i1 = i0 + 1;

            int h0 = hash[i0];
            int h1 = hash[i1];

            var t0 = interp((Sample1D)t);

            float a = h0;
            float b = h1 - h0;

            return new Sample1D(a + b * t0.Value, b * t0.Gradient) * 2f / hashMask - 1;
        }

        // Implementation thanks to Catlike Coding (https://catlikecoding.com/unity/tutorials/noise/)
        // Support them at: https://www.patreon.com/catlikecoding
        public override Sample<float2> Get(float x, float y, float frequency)
        {
            x *= frequency;
            y *= frequency;

            int ix0 = Mathf.FloorToInt(x);
            int iy0 = Mathf.FloorToInt(y);
            float tx = x - ix0;
            float ty = y - iy0;
            ix0 &= hashMask;
            iy0 &= hashMask;
            int ix1 = ix0 + 1;
            int iy1 = iy0 + 1;

            int h0 = hash[ix0];
            int h1 = hash[ix1];
            int h00 = hash[h0 + iy0];
            int h10 = hash[h1 + iy0];
            int h01 = hash[h0 + iy1];
            int h11 = hash[h1 + iy1];

            var tx0 = interp((Sample1D)tx);
            var ty0 = interp((Sample1D)ty);

            tx = tx0.Value;
            ty = ty0.Value;

            float a = h00;
            float b = h10 - h00;
            float c = h01 - h00;
            float d = h11 - h01 - h10 + h00;

            return new Sample2D(a + b * tx + (c + d * tx) * ty,
                new float2((b + d * ty) * tx0.Gradient, (c + d * tx) * ty0.Gradient)) * 2f / hashMask - 1;
        }


        // Implementation thanks to Catlike Coding (https://catlikecoding.com/unity/tutorials/noise/)
        // Support them at: https://www.patreon.com/catlikecoding
        public override Sample<float3> Get(float x, float y, float z, float frequency)
        {
            x *= frequency;
            y *= frequency;
            z *= frequency;

            int ix0 = Mathf.FloorToInt(x);
            int iy0 = Mathf.FloorToInt(y);
            int iz0 = Mathf.FloorToInt(z);
            float tx = x - ix0;
            float ty = y - iy0;
            float tz = z - iz0;
            ix0 &= hashMask;
            iy0 &= hashMask;
            iz0 &= hashMask;
            int ix1 = ix0 + 1;
            int iy1 = iy0 + 1;
            int iz1 = iz0 + 1;

            int h0 = hash[ix0];
            int h1 = hash[ix1];
            int h00 = hash[h0 + iy0];
            int h10 = hash[h1 + iy0];
            int h01 = hash[h0 + iy1];
            int h11 = hash[h1 + iy1];
            int h000 = hash[h00 + iz0];
            int h100 = hash[h10 + iz0];
            int h010 = hash[h01 + iz0];
            int h110 = hash[h11 + iz0];
            int h001 = hash[h00 + iz1];
            int h101 = hash[h10 + iz1];
            int h011 = hash[h01 + iz1];
            int h111 = hash[h11 + iz1];

            var tx0 = interp((Sample1D)tx);
            var ty0 = interp((Sample1D)ty);
            var tz0 = interp((Sample1D)tz);

            tx = tx0.Value;
            ty = ty0.Value;
            tz = tz0.Value;

            float a = h000;
            float b = h100 - h000;
            float c = h010 - h000;
            float d = h001 - h000;
            float e = h110 - h010 - h100 + h000;
            float f = h101 - h001 - h100 + h000;
            float g = h011 - h001 - h010 + h000;
            float h = h111 - h011 - h101 + h001 - h110 + h010 + h100 - h000;

            return new Sample3D(a + b * tx + (c + e * tx) * ty + (d + f * tx + (g + h * tx) * ty) * tz,
                new float3((b + e * ty + (f + h * ty) * tz) * tx0.Gradient,
                    (c + e * tx + (g + h * tx) * tz) * ty0.Gradient,
                    (d + f * tx + (g + h * tx) * ty) * tz0.Gradient)) * 2f / hashMask - 1;

        }
    }
}