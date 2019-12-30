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

        public static Sample<T> Unlerp<T>(Sample<T> a, Sample<T> b, Sample<T> v)
        {
            return (v - a) / (b - a);
        }

        public static Sample<T> Unlerp<T>(Sample<T> a, Sample<T> b, float v)
        {
            return (v - a) / (b - a);
        }

        public static Sample<T> Remap<T>(Sample<T> oMin, Sample<T> oMax, Sample<T> nMin, Sample<T> nMax, Sample<T> v)
        {
            return Lerp(nMin, nMax, Unlerp(oMin, oMax, v));
        }

        public static Sample<T> Remap<T>(Sample<T> oMin, Sample<T> oMax, Sample<T> nMin, Sample<T> nMax, float v)
        {
            return Lerp(nMin, nMax, Unlerp(oMin, oMax, v));
        }

        public static Sample<T> Floor<T>(Sample<T> v)
        {
            return FloorImpl.Call((dynamic)v);
        }

        public static partial class FloorImpl
        {
            public static Sample<float> Call(Sample<float> v)
            {
                return new Sample<float>(math.floor(v.Value), 0);
            }

            public static Sample<float2> Call(Sample<float2> v)
            {
                return new Sample<float2>(math.floor(v.Value), float2.zero);
            }

            public static Sample<float3> Call(Sample<float3> v)
            {
                return new Sample<float3>(math.floor(v.Value), float3.zero);
            }
        }

        public static Sample<T> Ceil<T>(Sample<T> v)
        {
            return new Sample<T>(math.ceil(v.Value), Sample<T>.ZeroGradient());
        }

        public static Sample<T> Round<T>(Sample<T> v)
        {
            return Floor(v + new Sample<T>(.5f));
        }

        public static Sample<T> Clamp<T>(Sample<T> value, Sample<T> min, Sample<T> max)
        {
            return Min(Max(value, min), max);
        }

        public static Sample<T> Clamp<T>(Sample<T> v, float min, float max)
        {
            if (v.Value < min)
            {
                return new Sample<T>(min);
            }
            else if (v.Value > max)
            {
                return new Sample<T>(max);
            }
            else
            {
                return v;
            }
        }

        public static Sample<T> Clamp01<T>(Sample<T> value)
        {
            if (value < 0)
                return new Sample<T>(0);
            else if (value > 1)
                return new Sample<T>(1);
            else
                return value;
        }

        public static Sample<T> Pow<T>(Sample<T> a, Sample<T> b)
        {
            return new Sample<T>(Mathf.Pow(a.Value, b.Value),
                Mathf.Pow(a.Value, b.Value - 1) * (b.Value * (dynamic)a.Gradient + a.Value * Mathf.Log10(a.Value) * (dynamic)b.Gradient));
        }

        public static Sample<T> Pow<T>(float a, Sample<T> b)
        {
            return new Sample<T>(Mathf.Pow(a, b.Value), Mathf.Log(a) * Mathf.Pow(a, b.Value) * (dynamic)b.Gradient);
        }

        public static Sample<T> Pow<T>(Sample<T> a, float b)
        {
            return new Sample<T>(Mathf.Pow(a.Value, b), Mathf.Pow(a.Value, b - 1) * (b * (dynamic)a.Gradient));
        }

        public static Sample<T> Exp<T>(Sample<T> a)
        {
            return new Sample<T>(Mathf.Exp(a.Value), Mathf.Exp(a.Value) * (dynamic)a.Gradient);
        }

        public static Sample<T> Sqrt<T>(Sample<T> a)
        {
            return new Sample<T>(Mathf.Sqrt(a.Value), (dynamic)a.Gradient / (2 * Mathf.Sqrt(a.Value)));
        }

        public static Sample<T> Sin<T>(Sample<T> a)
        {
            return new Sample<T>(Mathf.Sin(a.Value), (dynamic)a.Gradient * Mathf.Cos(a.Value));
        }

        public static Sample<T> Cos<T>(Sample<T> a)
        {
            return new Sample<T>(Mathf.Cos(a.Value), -(dynamic)a.Gradient * Mathf.Sin(a.Value));
        }

        public static Sample<T> Tan<T>(Sample<T> a)
        {
            return new Sample<T>(Mathf.Tan(a.Value), (dynamic)a.Gradient * Mathf.Pow(1 / Mathf.Cos(a.Value), 2));
        }

        public static Sample<T> Log<T>(Sample<T> a)
        {
            return new Sample<T>(Mathf.Log(a.Value), (dynamic)a.Gradient / a.Value);
        }

        public static Sample<T> Log10<T>(Sample<T> a)
        {
            return new Sample<T>(Mathf.Log10(a.Value), (dynamic)a.Gradient / (Mathf.Log(10) * a.Value));
        }

        public static Sample<T> Log<T>(Sample<T> a, Sample<T> b)
        {
            return new Sample<T>(Mathf.Log(a.Value, b.Value),
                ((dynamic)a.Gradient * Mathf.Log(b.Value) / a.Value - (dynamic)b.Gradient * Mathf.Log(a.Value) / b.Value)
                / Mathf.Pow(Mathf.Log(b.Value), 2));
        }

        public static Sample<T> Log<T>(Sample<T> a, float b)
        {
            return new Sample<T>(Mathf.Log(a.Value, b),
                ((dynamic)a.Gradient * Mathf.Log(b) / a.Value) / Mathf.Pow(Mathf.Log(b), 2));
        }

        public static Sample<T> Abs<T>(Sample<T> a)
        {
            return new Sample<T>(Mathf.Abs(a.Value), a.Value * (dynamic)a.Gradient / Mathf.Abs(a.Value));
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
