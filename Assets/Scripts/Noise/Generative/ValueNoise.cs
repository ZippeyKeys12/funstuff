using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using TweenType = Interp.TweenType;
using TweenTypes = Interp.TweenTypes;

namespace ZNoise
{
    [Serializable]
    public class ValueNoise : Generator
    {
        protected const int hashMask = 255;
        protected readonly int[] hash;
        protected readonly TweenType interp;

        public ValueNoise(int seed)
            : this(seed, TweenTypes.SmootherStep)
        { }

        public ValueNoise(int seed, TweenType interp)
        {
            var rnd = new Random(seed);
            var temp = Enumerable.Range(0, hashMask + 1).OrderBy(x => rnd.Next());
            hash = temp.Concat(temp).ToArray();
            this.interp = interp;
        }

        // Implementation thanks to Catlike Coding (https://catlikecoding.com/unity/tutorials/noise/)
        // Support them at: https://www.patreon.com/catlikecoding
        public override Sample1D Get(float x, float frequency)
        {
            int i0 = Mathf.FloorToInt(x);
            float t = x - i0;
            i0 &= hashMask;
            int i1 = i0 + 1;

            int h0 = hash[i0];
            int h1 = hash[i1];

            t = interp(t);
            return Mathf.Lerp(h0, h1, t) * (2f / hashMask) - 1;
        }

        // Implementation thanks to Catlike Coding (https://catlikecoding.com/unity/tutorials/noise/)
        // Support them at: https://www.patreon.com/catlikecoding
        public override Sample2D Get(float x, float y, float frequency)
        {
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

            tx = interp(tx);
            ty = interp(ty);
            return Mathf.Lerp(
                Mathf.Lerp(h00, h10, tx),
                Mathf.Lerp(h01, h11, tx),
                ty) * (2f / hashMask) - 1;
        }


        // Implementation thanks to Catlike Coding (https://catlikecoding.com/unity/tutorials/noise/)
        // Support them at: https://www.patreon.com/catlikecoding
        public override Sample3D Get(float x, float y, float z, float frequency)
        {
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

            tx = interp(tx);
            ty = interp(ty);
            tz = interp(tz);
            return Mathf.Lerp(
                Mathf.Lerp(Mathf.Lerp(h000, h100, tx), Mathf.Lerp(h010, h110, tx), ty),
                Mathf.Lerp(Mathf.Lerp(h001, h101, tx), Mathf.Lerp(h011, h111, tx), ty),
                tz) * (2f / hashMask) - 1;

        }
    }
}