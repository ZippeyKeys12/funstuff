using Unity.Mathematics;

namespace Noise
{
    public class Norm : Map
    {
        public Norm(Generator gen)
            : base(gen)
        { }

        protected override Sample<T> TransformFunc<T>(Sample<T> a)
            => new Sample<T>(math.length((dynamic)a.Gradient));
    }
}