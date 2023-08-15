using Unity.Mathematics;

namespace Noise.Unary
{
    public class Norm : Unary.Map
    {
        public Norm(Generator gen)
            : base(gen)
        { }

        protected override Sample<T> TransformFunc<T>(Sample<T> a)
            => new Sample<T>(math.length((dynamic)a.Gradient));
    }
}