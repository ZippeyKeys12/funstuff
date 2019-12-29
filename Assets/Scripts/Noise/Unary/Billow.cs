using System;

namespace Noise
{
    [Serializable]
    public class Billow : Map
    {
        public Billow(Generator gen)
            : base(gen)
        { }

        protected override Sample<T> TransformFunc<T>(Sample<T> a)
        {
            return 2 * Maths.Abs((dynamic)a) - 1;
        }
    }
}