using System;
using System.Linq;
using Unity.Mathematics;
using Random = System.Random;

namespace Noise.Generative
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

        public override Sample<float> Get(float x)
        {
            var i = (int)math.floor(x) & hashMask;

            return new Sample<float>(hash[i] * 1f / hashMask);
        }

        public override Sample<float2> Get(float2 xy)
        {
            var i = new int2(math.floor(xy)) & hashMask;

            return new Sample<float2>(hash[hash[i.x] + i.y] * 1f / hashMask);
        }

        public override Sample<float3> Get(float3 xyz)
        {
            var i = new int3(math.floor(xyz)) & hashMask;

            return (Sample<float3>)(hash[hash[hash[i.x] + i.y] + i.z] * 1f / hashMask);
        }
    }
}