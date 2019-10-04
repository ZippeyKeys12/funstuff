using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

namespace Noise
{
    public abstract class Generator
    {
        public abstract Sample<float> Get(float x, float frequency);
        public abstract Sample<Vector2> Get(float x, float y, float frequency);
        public abstract Sample<Vector3> Get(float x, float y, float z, float frequency);

        public static Generator operator -(Generator a)
        {
            return new Function(
                (x, f) => -a.Get(x, f),
                (x, y, f) => -a.Get(x, y, f),
                (x, y, z, f) => -a.Get(x, y, z, f));
        }

        public static Generator operator +(Generator a, Generator b)
        {
            return new Function(
                (x, f) => a.Get(x, f) + b.Get(x, f),
                (x, y, f) => a.Get(x, y, f) + b.Get(x, y, f),
                (x, y, z, f) => a.Get(x, y, z, f) + b.Get(x, y, z, f));
        }

        public static Generator operator +(Generator a, float b)
        {
            return new Function(
                (x, f) => a.Get(x, f) + b,
                (x, y, f) => a.Get(x, y, f) + b,
                (x, y, z, f) => a.Get(x, y, z, f) + b);
        }

        public static Generator operator +(float b, Generator a)
        {
            return a + b;
        }

        public static Generator operator -(Generator a, Generator b)
        {
            return a + -b;
        }

        public static Generator operator -(Generator a, float b)
        {
            return a + -b;
        }

        public static Generator operator -(float b, Generator a)
        {
            return b + -a;
        }

        public static Generator operator *(Generator a, Generator b)
        {
            return new Function(
                (x, f) => a.Get(x, f) * b.Get(x, f),
                (x, y, f) => a.Get(x, y, f) * b.Get(x, y, f),
                (x, y, z, f) => a.Get(x, y, z, f) * b.Get(x, y, z, f));
        }

        public static Generator operator *(Generator a, float b)
        {
            return new Function(
                (x, f) => a.Get(x, f) * b,
                (x, y, f) => a.Get(x, y, f) * b,
                (x, y, z, f) => a.Get(x, y, z, f) * b);
        }

        public static Generator operator *(float b, Generator a)
        {
            return a * b;
        }

        public static Generator operator /(Generator a, Generator b)
        {
            return new Function(
                (x, f) => a.Get(x, f) / b.Get(x, f),
                (x, y, f) => a.Get(x, y, f) / b.Get(x, y, f),
                (x, y, z, f) => a.Get(x, y, z, f) / b.Get(x, y, z, f));
        }

        public static Generator operator /(Generator a, float b)
        {
            return new Function(
                (x, f) => a.Get(x, f) / b,
                (x, y, f) => a.Get(x, y, f) / b,
                (x, y, z, f) => a.Get(x, y, z, f) / b);
        }

        //public static Generator operator /(float b, Generator a) {
        //    return new Function(
        //        (x, f) => b / a.Get(x, f),
        //        (x, y, f) => b / a.Get(x, y, f),
        //        (x, y, z, f) => b / a.Get(x, y, z, f));
        //}
    }
}