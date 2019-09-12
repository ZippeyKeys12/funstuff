using System;

namespace ZNoise
{
    public class Function : Generator
    {
        protected readonly Func<float, float, Sample1D> func_a;
        protected readonly Func<float, float, float, Sample2D> func_b;
        protected readonly Func<float, float, float, float, Sample3D> func_c;

        public Function(Func<float, float, Sample1D> func_a, Func<float, float, float, Sample2D> func_b, Func<float, float, float, float, Sample3D> func_c)
        {
            this.func_a = func_a;
            this.func_b = func_b;
            this.func_c = func_c;
        }

        public override Sample1D Get(float x, float frequency)
        {
            if (func_a != null)
                return func_a(x, frequency);

            throw new NotImplementedException();
        }

        public override Sample2D Get(float x, float y, float frequency)
        {
            if (func_b != null)
                return func_b(x, y, frequency);

            throw new NotImplementedException();
        }

        public override Sample3D Get(float x, float y, float z, float frequency)
        {
            if (func_c != null)
                return func_c(x, y, z, frequency);

            throw new NotImplementedException();
        }
    }
}