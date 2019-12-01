using System;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Random = System.Random;
using TweenType = Interp.TweenType;
using TweenTypes = Interp.TweenTypes;

namespace Noise
{
    // Implementation based on Ken Perlin's (https://mrl.nyu.edu/~perlin/noise/)
    [Serializable]
    public class PerlinNoise : Generator
    {
        protected const int hashMask = 255;
        protected readonly int[] perm;
        protected readonly TweenType interp;

        public PerlinNoise(int seed)
            : this(seed, TweenTypes.SmootherStep) { }

        public PerlinNoise(int seed, TweenType interp)
        {
            var rnd = new Random(seed);
            var temp = Enumerable.Range(0, hashMask + 1).OrderBy(x => rnd.Next());
            perm = temp.Concat(temp).ToArray();
            this.interp = interp;
        }

        public override Sample<float> Get(float x, float frequency)
        {
            x *= frequency;

            var X = (int)math.floor(x) & hashMask;
            x -= math.floor(x);
            var u = interp((Sample1D)x).Value;

            return new Sample1D(math.lerp(Grad(perm[X], x), Grad(perm[X + 1], x - 1), u) * 2);
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            xy *= frequency;

            var X = (int)math.floor(xy.x) & hashMask;
            var Y = (int)math.floor(xy.y) & hashMask;

            xy -= math.floor(xy);

            var x = xy.x; // TODO: Remove and replace
            var y = xy.y;
            float u = interp((Sample1D)x).Value,
                  v = interp((Sample1D)y).Value;

            int A = (perm[X] + Y) & hashMask,
                B = (perm[X + 1] + Y) & hashMask;

            return new Sample2D(math.lerp(math.lerp(Grad(perm[A], x, y), Grad(perm[B], x - 1, y), u),
                                math.lerp(Grad(perm[A + 1], x, y - 1), Grad(perm[B + 1], x - 1, y - 1), u), v));
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            var X = (int)math.floor(xyz.x) & hashMask;
            var Y = (int)math.floor(xyz.y) & hashMask;
            var Z = (int)math.floor(xyz.z) & hashMask;

            xyz -= math.floor(xyz);

            var x = xyz.x; // TODO: Remove and replace
            var y = xyz.y;
            var z = xyz.z;
            float u = interp((Sample1D)x).Value,
                  v = interp((Sample1D)y).Value,
                  w = interp((Sample1D)z).Value;

            int A = (perm[X] + Y) & hashMask, AA = (perm[A] + Z) & hashMask, AB = (perm[A + 1] + Z) & hashMask,
                B = (perm[X + 1] + Y) & hashMask, BA = (perm[B] + Z) & hashMask, BB = (perm[B + 1] + Z) & hashMask;

            return new Sample3D(math.lerp(math.lerp(math.lerp(Grad(perm[AA], x, y, z), Grad(perm[BA], x - 1, y, z), u),
                                          math.lerp(Grad(perm[AB], x, y - 1, z), Grad(perm[BB], x - 1, y - 1, z), u), u),
                                math.lerp(math.lerp(Grad(perm[AA + 1], x, y, z - 1), Grad(perm[BA + 1], x - 1, y, z - 1), u),
                                          math.lerp(Grad(perm[AB + 1], x, y - 1, z - 1), Grad(perm[BB + 1], x - 1, y - 1, z - 1), u), v), w));
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