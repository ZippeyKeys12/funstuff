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
            var i = (int)math.floor(x) & hashMask;

            return new Sample<float>(hash[i] * (2f / hashMask) - 1);
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            xy *= frequency;

            var i = new int2(math.floor(xy)) & hashMask;

            return new Sample<float2>(hash[hash[i.x] + i.y] * (2f / hashMask) - 1);
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            xyz *= frequency;

            var i = new int3(math.floor(xyz)) & hashMask;

            return (Sample<float3>)(hash[hash[hash[i.x] + i.y] + i.z] * (2f / hashMask) - 1);
        }
    }
}