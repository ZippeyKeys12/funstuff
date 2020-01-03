using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Noise
{
    public abstract class Generator
    {
        public abstract Sample<float> Get(float x, float frequency);
        public abstract Sample<float2> Get(float2 xy, float frequency);
        public abstract Sample<float3> Get(float3 xyz, float frequency);

        public static Generator operator -(Generator a)
        {
            return new Function(
                (x, f) => -a.Get(x, f),
                (xy, f) => -a.Get(xy, f),
                (xyz, f) => -a.Get(xyz, f));
        }

        public static Generator operator +(Generator a, Generator b)
        {
            return new Function(
                (x, f) => a.Get(x, f) + b.Get(x, f),
                (xy, f) => a.Get(xy, f) + b.Get(xy, f),
                (xyz, f) => a.Get(xyz, f) + b.Get(xyz, f));
        }

        public static Generator operator +(Generator a, float b)
        {
            return new Function(
                (x, f) => a.Get(x, f) + b,
                (xy, f) => a.Get(xy, f) + b,
                (xyz, f) => a.Get(xyz, f) + b);
        }

        public static Generator operator +(float b, Generator a)
        {
            return a + b;
        }

        public static Generator operator -(Generator a, Generator b)
        {
            return new Function(
                (x, f) => a.Get(x, f) - b.Get(x, f),
                (xy, f) => a.Get(xy, f) - b.Get(xy, f),
                (xyz, f) => a.Get(xyz, f) - b.Get(xyz, f));
        }

        public static Generator operator -(Generator a, float b)
        {
            return new Function(
                (x, f) => a.Get(x, f) - b,
                (xy, f) => a.Get(xy, f) - b,
                (xyz, f) => a.Get(xyz, f) - b);
        }

        public static Generator operator -(float b, Generator a)
        {
            return new Function(
                (x, f) => b - a.Get(x, f),
                (xy, f) => b - a.Get(xy, f),
                (xyz, f) => b - a.Get(xyz, f));
        }

        public static Generator operator *(Generator a, Generator b)
        {
            return new Function(
                (x, f) => a.Get(x, f) * b.Get(x, f),
                (xy, f) => a.Get(xy, f) * b.Get(xy, f),
                (xyz, f) => a.Get(xyz, f) * b.Get(xyz, f));
        }

        public static Generator operator *(Generator a, float b)
        {
            return new Function(
                (x, f) => a.Get(x, f) * b,
                (xy, f) => a.Get(xy, f) * b,
                (xyz, f) => a.Get(xyz, f) * b);
        }

        public static Generator operator *(float b, Generator a)
        {
            return a * b;
        }

        public static Generator operator /(Generator a, Generator b)
        {
            return new Function(
                (x, f) => a.Get(x, f) / b.Get(x, f),
                (xy, f) => a.Get(xy, f) / b.Get(xy, f),
                (xyz, f) => a.Get(xyz, f) / b.Get(xyz, f));
        }

        public static Generator operator /(Generator a, float b)
        {
            return new Function(
                (x, f) => a.Get(x, f) / b,
                (xy, f) => a.Get(xy, f) / b,
                (xyz, f) => a.Get(xyz, f) / b);
        }

        public static Generator operator /(float b, Generator a)
        {
            return new Function(
                (x, f) => b / a.Get(x, f),
                (xy, f) => b / a.Get(xy, f),
                (xyz, f) => b / a.Get(xyz, f));
        }
    }
}