using System;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Random = System.Random;

namespace Noise
{
    [Serializable]
    public class WhiteNoise : Generator
    {
        protected const int hashMask = 255;
        protected readonly int[] hash;

        public WhiteNoise(int seed)
        {
            var rnd = new Random(seed);
            var temp = Enumerable.Range(0, hashMask + 1).OrderBy(x => rnd.Next());
            hash = temp.Concat(temp).ToArray();
        }

        public override Sample<float> Get(float x, float frequency)
        {
            x *= frequency;
            int i = Mathf.FloorToInt(x);
            i &= hashMask;

            return new Sample<float>(hash[i] * (2f / hashMask) - 1,
                                (x == i) ? float.NaN : 0);
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            xy *= frequency;

            var i = new int2(math.floor(xy));
            var i2 = i;

            i2.x &= hashMask;
            i2.y &= hashMask;

            // TODO: Fix gradient
            return new Sample<float2>(hash[hash[i2.x] + i2.y] * (2f / hashMask) - 1, new float2(1, 1));
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            xyz *= frequency;
            int ix = Mathf.FloorToInt(xyz.x);
            int iy = Mathf.FloorToInt(xyz.y);
            int iz = Mathf.FloorToInt(xyz.z);
            ix &= hashMask;
            iy &= hashMask;
            iz &= hashMask;
            return (Sample<float3>)(hash[hash[hash[ix] + iy] + iz] * (2f / hashMask) - 1);
        }
    }
}