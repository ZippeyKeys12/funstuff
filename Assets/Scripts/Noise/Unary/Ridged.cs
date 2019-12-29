using System;
using UnityEngine;

namespace Noise
{
    [Serializable]
    public class Ridged : Map
    {
        public Ridged(Generator gen)
            : base(gen)
        { }

        protected override Sample<T> TransformFunc<T>(Sample<T> a)
        {
            return -2 * Maths.Abs((dynamic)a) + 1;
        }
    }
}