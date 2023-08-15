namespace Noise.Unary
{
    public class Bias : Map
    {
        protected float bias;

        public Bias(Generator gen, float bias)
            : base(gen)
        {
            this.bias = bias;
        }

        protected override Sample<T> TransformFunc<T>(Sample<T> a)
        {
            a = Maths.Clamp(2 * a - 1, -1, 1);
            return 2 * (a + 1) / (bias * (a - 1) + 2) - 1;
        }
    }
}