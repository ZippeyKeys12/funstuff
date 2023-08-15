using System;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Random = System.Random;
using TweenType = Algorithms.Interp.TweenType;
using TweenTypes = Algorithms.Interp.TweenTypes;

namespace Noise
{
    // Implementation based on Ken Perlin's (https://mrl.nyu.edu/~perlin/noise/)
    [Serializable]
    public sealed class PerlinNoise : Generator
    {
        private const int HashMask = 255;
        private readonly int[] perm;
        private readonly TweenType interp;

        public PerlinNoise(int seed)
            : this(seed, TweenTypes.SmootherStep) { }

        public PerlinNoise(int seed, TweenType interp)
        {
            var rnd = new Random(seed);
            var temp = Enumerable.Range(0, HashMask + 1).OrderBy(x => rnd.Next());
            perm = temp.Concat(temp).ToArray();
            this.interp = interp;
        }

        public override Sample<float> Get(float x)
        {
            var ix = (int)math.floor(x) & HashMask;
            x -= math.floor(x);
            var u = interp(new Sample<float>(x, 1)).Value;

            return new Sample<float>(math.lerp(Grad(perm[ix], x), Grad(perm[ix + 1], x - 1), u)) / 2 + .5f;
        }

        public override Sample<float2> Get(float2 xy)
        {
            var ix = (int)math.floor(xy.x) & HashMask;
            var iy = (int)math.floor(xy.y) & HashMask;

            xy -= math.floor(xy);

            var x = xy.x; // TODO: Remove and replace
            var y = xy.y;
            float u = interp(new Sample<float>(x, 1)).Value,
                  v = interp(new Sample<float>(y, 1)).Value;

            int a = (perm[ix] + iy) & HashMask,
                b = (perm[ix + 1] + iy) & HashMask;

            return new Sample<float2>(math.lerp(math.lerp(Grad(perm[a], x, y), Grad(perm[b], x - 1, y), u),
                                math.lerp(Grad(perm[a + 1], x, y - 1), Grad(perm[b + 1], x - 1, y - 1), u), v)) / 2 + .5f;
        }

        // TODO: This is broken
        public override Sample<float3> Get(float3 xyz)
        {
            var ix = (int)math.floor(xyz.x) & HashMask;
            var iy = (int)math.floor(xyz.y) & HashMask;
            var iz = (int)math.floor(xyz.z) & HashMask;

            xyz -= math.floor(xyz);

            var x = xyz.x; // TODO: Remove and replace
            var y = xyz.y;
            var z = xyz.z;
            float u = interp(new Sample<float>(x, 1)).Value,
                  v = interp(new Sample<float>(y, 1)).Value,
                  w = interp(new Sample<float>(z, 1)).Value;

            int a = (perm[ix] + iy) & HashMask, aa = (perm[a] + iz) & HashMask, ab = (perm[a + 1] + iz) & HashMask,
                b = (perm[ix + 1] + iy) & HashMask, ba = (perm[b] + iz) & HashMask, bb = (perm[b + 1] + iz) & HashMask;

            return new Sample<float3>(math.lerp(math.lerp(math.lerp(Grad(perm[aa], x, y, z), Grad(perm[ba], x - 1, y, z), u),
                                          math.lerp(Grad(perm[ab], x, y - 1, z), Grad(perm[bb], x - 1, y - 1, z), u), u),
                                math.lerp(math.lerp(Grad(perm[aa + 1], x, y, z - 1), Grad(perm[ba + 1], x - 1, y, z - 1), u),
                                          math.lerp(Grad(perm[ab + 1], x, y - 1, z - 1), Grad(perm[bb + 1], x - 1, y - 1, z - 1), u), v), w)) / 2 + .5f;
        }

        static float Grad(int hash, float x)
        {
            return (hash & 1) == 0 ? x : -x;
        }

        static float Grad(int hash, float x, float y)
        {
            return ((hash & 1) == 0 ? x : -x) + ((hash & 2) == 0 ? y : -y);
        }

        static float Grad(int hash, float x, float y, float z)
        {
            var h = hash & 15;
            var u = h < 8 ? x : y;
            var v = h < 4 ? y : (h == 12 || h == 14 ? x : z);
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }
    }
}