using UnityEngine;
using Unity.Mathematics;

namespace Noise
{
    public static class Maths
    {
        public static Sample<T> Lerp<T>(Sample<T> a, Sample<T> b, Sample<T> t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static Sample<T> Lerp<T>(Sample<T> a, Sample<T> b, float t)
        {
            return a + (b - a) * Mathf.Clamp01(t);
        }

        public static Sample<T> Clamp01<T>(Sample<T> value)
        {
            if (value < 0)
                return value.Of(0);
            else if (value > 1)
                return value.Of(1);
            else
                return value;
        }

        public static Sample<float> Pow(Sample<float> a, Sample<float> b)
        {
            return new Sample1D(Mathf.Pow(a.Value, b.Value),
                Mathf.Pow(a.Value, b.Value - 1) * (b.Value * a.Gradient + a.Value * Mathf.Log10(a.Value) * b.Gradient));
        }

        public static Sample<float> Pow(float a, Sample<float> b)
        {
            return new Sample1D(Mathf.Pow(a, b.Value), Mathf.Log(a) * Mathf.Pow(a, b.Value) * b.Gradient);
        }

        public static Sample<float> Pow(Sample<float> a, float b)
        {
            return new Sample1D(Mathf.Pow(a.Value, b), Mathf.Pow(a.Value, b - 1) * (b * a.Gradient));
        }

        public static Sample<float2> Pow(Sample<float2> a, Sample<float2> b)
        {
            return new Sample2D(Mathf.Pow(a.Value, b.Value),
                Mathf.Pow(a.Value, b.Value - 1) * (b.Value * a.Gradient + a.Value * Mathf.Log10(a.Value) * b.Gradient));
        }

        public static Sample<float2> Pow(Sample<float2> a, float b)
        {
            return new Sample2D(Mathf.Pow(a.Value, b), Mathf.Pow(a.Value, b - 1) * (b * a.Gradient));
        }

        public static Sample<float3> Pow(Sample<float3> a, Sample<float3> b)
        {
            return new Sample3D(Mathf.Pow(a.Value, b.Value),
                Mathf.Pow(a.Value, b.Value - 1) * (b.Value * a.Gradient + a.Value * Mathf.Log10(a.Value) * b.Gradient));
        }

        public static Sample<float3> Pow(Sample<float3> a, float b)
        {
            return new Sample3D(Mathf.Pow(a.Value, b), Mathf.Pow(a.Value, b - 1) * (b * a.Gradient));
        }

        public static Sample<float> Exp(Sample<float> a)
        {
            return new Sample1D(Mathf.Exp(a.Value), Mathf.Exp(a.Value) * a.Gradient);
        }

        public static Sample<float2> Exp(Sample<float2> a)
        {
            return new Sample2D(Mathf.Exp(a.Value), Mathf.Exp(a.Value) * a.Gradient);
        }

        public static Sample<float3> Exp(Sample<float3> a)
        {
            return new Sample3D(Mathf.Exp(a.Value), Mathf.Exp(a.Value) * a.Gradient);
        }

        public static Sample<float> Sqrt(Sample<float> a)
        {
            return new Sample1D(Mathf.Sqrt(a.Value), a.Gradient / (2 * Mathf.Sqrt(a.Value)));
        }

        public static Sample<float2> Sqrt(Sample<float2> a)
        {
            return new Sample2D(Mathf.Sqrt(a.Value), a.Gradient / (2 * Mathf.Sqrt(a.Value)));
        }

        public static Sample<float3> Sqrt(Sample<float3> a)
        {
            return new Sample3D(Mathf.Sqrt(a.Value), a.Gradient / (2 * Mathf.Sqrt(a.Value)));
        }

        public static Sample<float> Sin(Sample<float> a)
        {
            return new Sample1D(Mathf.Sin(a.Value), a.Gradient * Mathf.Cos(a.Value));
        }

        public static Sample<float2> Sin(Sample<float2> a)
        {
            return new Sample2D(Mathf.Sin(a.Value), a.Gradient * Mathf.Cos(a.Value));
        }

        public static Sample<float3> Sin(Sample<float3> a)
        {
            return new Sample3D(Mathf.Sin(a.Value), a.Gradient * Mathf.Cos(a.Value));
        }

        public static Sample<float> Cos(Sample<float> a)
        {
            return new Sample1D(Mathf.Cos(a.Value), -a.Gradient * Mathf.Sin(a.Value));
        }

        public static Sample<float2> Cos(Sample<float2> a)
        {
            return new Sample2D(Mathf.Cos(a.Value), -a.Gradient * Mathf.Sin(a.Value));
        }

        public static Sample<float3> Cos(Sample<float3> a)
        {
            return new Sample3D(Mathf.Cos(a.Value), -a.Gradient * Mathf.Sin(a.Value));
        }

        public static Sample<float> Tan(Sample<float> a)
        {
            return new Sample1D(Mathf.Tan(a.Value), a.Gradient * Mathf.Pow(1 / Mathf.Cos(a.Value), 2));
        }

        public static Sample<float2> Tan(Sample<float2> a)
        {
            return new Sample2D(Mathf.Tan(a.Value), a.Gradient * Mathf.Pow(1 / Mathf.Cos(a.Value), 2));
        }

        public static Sample<float3> Tan(Sample<float3> a)
        {
            return new Sample3D(Mathf.Tan(a.Value), a.Gradient * Mathf.Pow(1 / Mathf.Cos(a.Value), 2));
        }

        public static Sample<float> Log(Sample<float> a)
        {
            return new Sample1D(Mathf.Log(a.Value), a.Gradient / a.Value);
        }

        public static Sample<float> Log10(Sample<float> a)
        {
            return new Sample1D(Mathf.Log10(a.Value), a.Gradient / (Mathf.Log(10) * a.Value));
        }

        public static Sample<float> Log(Sample<float> a, Sample<float> b)
        {
            return new Sample1D(Mathf.Log(a.Value, b.Value),
                (a.Gradient * Mathf.Log(b.Value) / a.Value - b.Gradient * Mathf.Log(a.Value) / b.Value)
                / Mathf.Pow(Mathf.Log(b.Value), 2));
        }

        public static Sample<float> Log(Sample<float> a, float b)
        {
            return new Sample1D(Mathf.Log(a.Value, b),
                (a.Gradient * Mathf.Log(b) / a.Value) / Mathf.Pow(Mathf.Log(b), 2));
        }

        public static Sample<float2> Log(Sample<float2> a)
        {
            return new Sample2D(Mathf.Log(a.Value), a.Gradient / a.Value);
        }

        public static Sample<float2> Log(Sample<float2> a, Sample<float2> b)
        {
            return new Sample2D(Mathf.Log(a.Value, b.Value),
                (a.Gradient * Mathf.Log(b.Value) / a.Value - b.Gradient * Mathf.Log(a.Value) / b.Value)
                / Mathf.Pow(Mathf.Log(b.Value), 2));
        }

        public static Sample<float3> Log(Sample<float3> a)
        {
            return new Sample3D(Mathf.Log(a.Value), a.Gradient / a.Value);
        }

        public static Sample<float3> Log(Sample<float3> a, Sample<float3> b)
        {
            return new Sample3D(Mathf.Log(a.Value, b.Value),
                (a.Gradient * Mathf.Log(b.Value) / a.Value - b.Gradient * Mathf.Log(a.Value) / b.Value)
                / Mathf.Pow(Mathf.Log(b.Value), 2));
        }

        public static Sample<float> Abs(Sample<float> a)
        {
            return new Sample1D(Mathf.Abs(a.Value), a.Value * a.Gradient / Mathf.Abs(a.Value));
        }

        public static Sample<float2> Abs(Sample<float2> a)
        {
            return new Sample2D(Mathf.Abs(a.Value), a.Value * a.Gradient / Mathf.Abs(a.Value));
        }

        public static Sample<float3> Abs(Sample<float3> a)
        {
            return new Sample3D(Mathf.Abs(a.Value), a.Value * a.Gradient / Mathf.Abs(a.Value));
        }

        public static Sample<T> Max<T>(params Sample<T>[] samples)
        {
            var max = samples[0];

            for (var i = 1; i < samples.Length; i++)
            {
                if (max < samples[i])
                {
                    max = samples[i];
                }
            }

            return max;
        }

        public static Sample<T> Min<T>(params Sample<T>[] samples)
        {
            var min = samples[0];

            for (var i = 1; i < samples.Length; i++)
            {
                if (samples[i] < min)
                {
                    min = samples[i];
                }
            }

            return min;
        }
    }
}
