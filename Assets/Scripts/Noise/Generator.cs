using Noise.Generative;
using Noise.Unary;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Noise
{
    public abstract class Generator
    {
        public abstract Sample<float> Get(float x);
        public Sample<float> Get(Sample<float> x)
        {
            var ret = Get(x.Value);
            return new Sample<float>(ret.Value, ret.Gradient * x.Gradient);
        }

        public abstract Sample<float2> Get(float2 xy);
        public Sample<float2> Get(Sample<float> x, Sample<float> y)
        {
            var ret = Get(new float2(x.Value, y.Value));
            return new Sample<float2>(ret.Value, ret.Gradient * new float2(x.Gradient, y.Gradient));
        }

        public abstract Sample<float3> Get(float3 xyz);
        public Sample<float3> Get(Sample<float> x, Sample<float> y, Sample<float> z)
        {
            var ret = Get(new float3(x.Value, y.Value, z.Value));
            return new Sample<float3>(ret.Value, ret.Gradient * new float3(x.Gradient, y.Gradient, z.Gradient));
        }

        public Generator Scale(float scaling)
        {
            return new Scale(this, scaling);
        }

        public static Generator operator -(Generator a)
        {
            return new Function(
                x => -a.Get(x),
                xy => -a.Get(xy),
                xyz => -a.Get(xyz));
        }

        public static Generator operator +(Generator a, Generator b)
        {
            return new Function(
                x => a.Get(x) + b.Get(x),
                xy => a.Get(xy) + b.Get(xy),
                xyz => a.Get(xyz) + b.Get(xyz));
        }

        public static Generator operator +(Generator a, float b)
        {
            return new Function(
                x => a.Get(x) + b,
                xy => a.Get(xy) + b,
                xyz => a.Get(xyz) + b);
        }

        public static Generator operator +(float b, Generator a)
        {
            return a + b;
        }

        public static Generator operator -(Generator a, Generator b)
        {
            return new Function(
                x => a.Get(x) - b.Get(x),
                xy => a.Get(xy) - b.Get(xy),
                xyz => a.Get(xyz) - b.Get(xyz));
        }

        public static Generator operator -(Generator a, float b)
        {
            return new Function(
                x => a.Get(x) - b,
                xy => a.Get(xy) - b,
                xyz => a.Get(xyz) - b);
        }

        public static Generator operator -(float b, Generator a)
        {
            return new Function(
                x => b - a.Get(x),
                xy => b - a.Get(xy),
                xyz => b - a.Get(xyz));
        }

        public static Generator operator *(Generator a, Generator b)
        {
            return new Function(
                x => a.Get(x) * b.Get(x),
                xy => a.Get(xy) * b.Get(xy),
                xyz => a.Get(xyz) * b.Get(xyz));
        }

        public static Generator operator *(Generator a, float b)
        {
            return new Function(
                x => a.Get(x) * b,
                xy => a.Get(xy) * b,
                xyz => a.Get(xyz) * b);
        }

        public static Generator operator *(float b, Generator a)
        {
            return a * b;
        }

        public static Generator operator /(Generator a, Generator b)
        {
            return new Function(
                x => a.Get(x) / b.Get(x),
                xy => a.Get(xy) / b.Get(xy),
                xyz => a.Get(xyz) / b.Get(xyz));
        }

        public static Generator operator /(Generator a, float b)
        {
            return new Function(
                x => a.Get(x) / b,
                xy => a.Get(xy) / b,
                xyz => a.Get(xyz) / b);
        }

        public static Generator operator /(float b, Generator a)
        {
            return new Function(
                x => b / a.Get(x),
                xy => b / a.Get(xy),
                xyz => b / a.Get(xyz));
        }

        public static Generator operator ^(Generator a, Generator b)
            => new Function(
                x => Maths.Pow(a.Get(x), b.Get(x)),
                xy => Maths.Pow(a.Get(xy), b.Get(xy)),
                xyz => Maths.Pow(a.Get(xyz), b.Get(xyz))
            );

        public static Generator operator ^(Generator a, float b)
            => new Function(
                x => Maths.Pow(a.Get(x), b),
                xy => Maths.Pow(a.Get(xy), b),
                xyz => Maths.Pow(a.Get(xyz), b)
            );

        public static Generator operator ^(float b, Generator a)
            => new Function(
                x => Maths.Pow(b, a.Get(x)),
                xy => Maths.Pow(b, a.Get(xy)),
                xyz => Maths.Pow(b, a.Get(xyz))
            );
    }
}