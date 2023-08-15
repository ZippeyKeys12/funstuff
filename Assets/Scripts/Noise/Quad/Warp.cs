using Unity.Mathematics;

namespace Noise.Quad
{
    public class Warp : Generator
    {
        protected int levels;
        protected float strength;

        protected Generator gen;
        protected Generator a;
        protected Generator b;
        protected Generator c;

        public Warp(Generator gen, int levels)
            : this(gen, 4, levels)
        { }

        public Warp(Generator gen, float strength, int levels)
            : this(gen, gen, strength, levels)
        { }

        public Warp(Generator gen, Generator shift, int levels)
            : this(gen, shift, 4, levels)
        { }

        public Warp(Generator gen, Generator shift, float strength, int levels)
            : this(gen, shift.Translate(5.2f), shift.Translate(1.3f), shift.Translate(9.2f), strength, levels)
        { }

        public Warp(Generator gen, Generator a, Generator b, Generator c, int levels)
            : this(gen, a, b, c, 4, levels)
        { }

        public Warp(Generator gen, Generator a, Generator b, Generator c, float strength, int levels)
        {
            this.gen = gen;
            this.a = a;
            this.b = b;
            this.c = c;
            this.strength = strength;
            this.levels = levels;
        }

        public override Sample<float> Get(float x)
        {
            var p = Sample<float>.Zero;

            for (var i = 0; i < levels; i++)
            {
                p = a.Get(x + strength * p);
            }

            return gen.Get(x + strength * p);
        }

        public override Sample<float2> Get(float2 xy)
        {
            var p = Sample<float>.Zero;
            var q = Sample<float>.Zero;

            for (var i = 0; i < levels; i++)
            {
                p = a.Get(xy.x + strength * p);
                q = b.Get(xy.y + strength * q);
            }

            return gen.Get(xy.x + strength * p, xy.y + strength * q);
        }

        public override Sample<float3> Get(float3 xyz)
        {
            var p = Sample<float>.Zero;
            var q = Sample<float>.Zero;
            var r = Sample<float>.Zero;

            for (var i = 0; i < levels; i++)
            {
                p = a.Get(xyz.x + strength * p);
                q = b.Get(xyz.y + strength * q);
                r = b.Get(xyz.z + strength * r);
            }

            return gen.Get(xyz.x + strength * p, xyz.y + strength * q, xyz.z + strength * r);
        }
    }
}