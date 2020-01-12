using Unity.Mathematics;

namespace Noise
{
    public class Warp : Generator
    {
        protected int levels;
        protected float strength;

        protected Generator gen;
        protected Generator a;
        protected Generator b;
        protected Generator c;

        public Warp(Generator gen, Generator a, Generator b, Generator c, float strength, int levels)
        {
            this.gen = gen;
            this.a = a;
            this.b = b;
            this.c = c;
            this.strength = strength;
            this.levels = levels;
        }

        public override Sample<float> Get(float x, float frequency)
        {
            var p = Sample<float>.Zero;

            for (var i = 0; i < levels; i++)
            {
                p = a.Get(x + strength * p, frequency);
            }

            return gen.Get(x + strength * p, frequency);
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            var p = Sample<float>.Zero;
            var q = Sample<float>.Zero;

            for (var i = 0; i < levels; i++)
            {
                p = a.Get(xy.x + strength * p, frequency);
                q = b.Get(xy.y + strength * q, frequency);
            }

            return gen.Get(xy.x + strength * p, xy.y + strength * q, frequency);
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            var p = Sample<float>.Zero;
            var q = Sample<float>.Zero;
            var r = Sample<float>.Zero;

            for (var i = 0; i < levels; i++)
            {
                p = a.Get(xyz.x + strength * p, frequency);
                q = b.Get(xyz.y + strength * q, frequency);
                r = b.Get(xyz.z + strength * r, frequency);
            }

            return gen.Get(xyz.x + strength * p, xyz.y + strength * q, xyz.z + strength * r, frequency);
        }
    }
}