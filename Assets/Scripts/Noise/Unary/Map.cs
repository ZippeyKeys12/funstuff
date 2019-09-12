using System;

namespace ZNoise
{
    public class Map : Generator
    {
        protected readonly Func<Sample1D, Sample1D> func_a;
        protected readonly Func<Sample2D, Sample2D> func_b;
        protected readonly Func<Sample3D, Sample3D> func_c;
        protected readonly Generator a;

        public Map(Generator a, Func<Sample1D, Sample1D> func_a, Func<Sample2D, Sample2D> func_b, Func<Sample3D, Sample3D> func_c)
        {
            this.a = a;
            this.func_a = func_a;
            this.func_b = func_b;
            this.func_c = func_c;
        }

        public override Sample1D Get(float x, float frequency)
        {
            return func_a(a.Get(x, frequency));
        }

        public override Sample2D Get(float x, float y, float frequency)
        {
            return func_b(a.Get(x, y, frequency));
        }

        public override Sample3D Get(float x, float y, float z, float frequency)
        {
            return func_c(a.Get(x, y, z, frequency));
        }
    }
}