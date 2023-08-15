namespace Noise.Generative
{
    public class SquareNoise : Unary.Map
    {
        public SquareNoise()
            : this(new SinNoise())
        { }

        public SquareNoise(Generator gen)
            : base(gen)
        { }

        protected override Sample<T> TransformFunc<T>(Sample<T> a)
        {
            return Maths.Sign(2 * a - 1) / 2 + .5f;
        }
    }
}