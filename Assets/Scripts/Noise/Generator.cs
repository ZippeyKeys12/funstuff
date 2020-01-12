using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Noise
{
    public abstract class Generator
    {
        public abstract Sample<float> Get(float x, float frequency);
        public Sample<float> Get(Sample<float> x, float frequency)
        {
            var ret = Get(x, frequency);
            return new Sample<float>(ret.Value, ret.Gradient * x.Gradient);
        }

        public abstract Sample<float2> Get(float2 xy, float frequency);
        public Sample<float2> Get(Sample<float> x, Sample<float> y, float frequency)
        {
            var ret = Get(new float2(x.Value, y.Value), frequency);
            return new Sample<float2>(ret.Value, ret.Gradient * new float2(x.Gradient, y.Gradient));
        }

        public abstract Sample<float3> Get(float3 xyz, float frequency);
        public Sample<float3> Get(Sample<float> x, Sample<float> y, Sample<float> z, float frequency)
        {
            var ret = Get(new float3(x.Value, y.Value, z.Value), frequency);
            return new Sample<float3>(ret.Value, ret.Gradient * new float3(x.Gradient, y.Gradient, z.Gradient));
        }

        public Generator Scale(float scaling)
        {
            return new Scale(this, scaling);
        }

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

        public static Generator operator ^(Generator a, Generator b)
            => new Function(
                (x, f) => Maths.Pow(a.Get(x, f), b.Get(x, f)),
                (xy, f) => Maths.Pow(a.Get(xy, f), b.Get(xy, f)),
                (xyz, f) => Maths.Pow(a.Get(xyz, f), b.Get(xyz, f))
            );

        public static Generator operator ^(Generator a, float b)
            => new Function(
                (x, f) => Maths.Pow(a.Get(x, f), b),
                (xy, f) => Maths.Pow(a.Get(xy, f), b),
                (xyz, f) => Maths.Pow(a.Get(xyz, f), b)
            );

        public static Generator operator ^(float b, Generator a)
            => new Function(
                (x, f) => Maths.Pow(b, a.Get(x, f)),
                (xy, f) => Maths.Pow(b, a.Get(xy, f)),
                (xyz, f) => Maths.Pow(b, a.Get(xyz, f))
            );
    }
}