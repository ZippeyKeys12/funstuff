using System;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;

public interface GeneratorSample<T, R> : IJobParallelFor
    where T : unmanaged
{
    T Dimension { get; set; }
    float Frequency { get; set; }

    R Get(T x);
}
public sealed class Sample<T>
{
    public static Sample<T> Zero
        => new Sample<T>(0);

    public static Sample<T> MinValue
        => new Sample<T>(float.MinValue);

    public static Sample<T> MaxValue
        => new Sample<T>(float.MaxValue);

    public float Value { get; }
    public T Gradient { get; }

    public Sample(float value)
    {
        Value = value;
        Gradient = ZeroGradient();
    }

    public Sample(float value, T gradient)
    {
        Value = value;
        Gradient = gradient;
    }

    public static explicit operator Sample<T>(float a)
    {
        return new Sample<T>(a);
    }

    public static T ZeroGradient()
    {
        var t = typeof(T);

        if (t == typeof(float))
        {
            return (dynamic)0f;
        }
        else if (t == typeof(float2))
        {
            return (dynamic)float2.zero;
        }
        else if (t == typeof(float3))
        {
            return (dynamic)float3.zero;
        }
        else if (t == typeof(float4))
        {
            return (dynamic)float4.zero;
        }

        return (dynamic)null;
    }

    public Sample<T> Apply(Func<Sample<float>, Sample<float>> f)
    {
        var t = typeof(T);

        if (t == typeof(float))
        {
            return f((dynamic)this);
        }
        else
        {
            var g = (dynamic)Gradient;
            if (t == typeof(float2))
            {
                var rx = f(new Sample<float>(Value, g.x));
                var ry = f(new Sample<float>(Value, g.y));

                return (dynamic)new Sample<float2>(rx.Value, new float2(rx.Gradient, ry.Gradient));
            }
            else if (t == typeof(float3))
            {
                var rx = f(new Sample<float>(Value, g.x));
                var ry = f(new Sample<float>(Value, g.y));
                var rz = f(new Sample<float>(Value, g.z));

                return (dynamic)new Sample<float3>(rx.Value, new float3(rx.Gradient, ry.Gradient, rz.Gradient));
            }
            else if (t == typeof(float4))
            {
                var rx = f(new Sample<float>(Value, g.x));
                var ry = f(new Sample<float>(Value, g.y));
                var rz = f(new Sample<float>(Value, g.z));
                var rw = f(new Sample<float>(Value, g.w));

                return (dynamic)new Sample<float4>(rx.Value, new float4(rx.Gradient, ry.Gradient, rz.Gradient, rw.Gradient));
            }
        }

        return (dynamic)null;
    }

    public Sample<T> Neg()
        => new Sample<T>(-Value, -(dynamic)Gradient);
    public static Sample<T> operator -(Sample<T> a) => a.Neg();


    public Sample<T> Add(float o)
        => new Sample<T>(Value + o, Gradient);
    public static Sample<T> operator +(Sample<T> a, float b) => a.Add(b);
    public static Sample<T> operator +(float b, Sample<T> a) => a.Add(b);

    public Sample<T> Add(Sample<T> o)
        => new Sample<T>(Value + o.Value, (dynamic)Gradient + o.Gradient);
    public static Sample<T> operator +(Sample<T> a, Sample<T> b) => a.Add(b);


    public Sample<T> Sub(float o)
        => new Sample<T>(Value - o, Gradient);
    public static Sample<T> operator -(Sample<T> a, float b) => a.Sub(b);
    public static Sample<T> operator -(float b, Sample<T> a) => b + -a;

    public Sample<T> Sub(Sample<T> o)
        => new Sample<T>(Value - o.Value, (dynamic)Gradient - o.Gradient);
    public static Sample<T> operator -(Sample<T> a, Sample<T> b) => a.Sub(b);


    public Sample<T> Mul(float o)
        => new Sample<T>(Value * o, o * (dynamic)Gradient);
    public static Sample<T> operator *(Sample<T> a, float b) => a.Mul(b);
    public static Sample<T> operator *(float b, Sample<T> a) => a.Mul(b);

    public Sample<T> Mul(Sample<T> o)
        => new Sample<T>(Value * o.Value, o.Value * (dynamic)Gradient + Value * (dynamic)o.Gradient);
    public static Sample<T> operator *(Sample<T> a, Sample<T> b) => a.Mul(b);


    public static Sample<T> operator /(Sample<T> a, float b) => a.Mul(1 / b);
    public static Sample<T> operator /(float b, Sample<T> a) => Maths.Pow(a, -1).Mul(b);

    public Sample<T> Div(Sample<T> o)
        => new Sample<T>(Value * o.Value, (o.Value * (dynamic)Gradient - Value * (dynamic)o.Gradient) / math.pow(o.Value, 2));
    public static Sample<T> operator /(Sample<T> a, Sample<T> b) => a.Div(b);


    public static bool operator >(Sample<T> a, float b) => a.Value > b;
    public static bool operator >(float b, Sample<T> a) => b > a.Value;
    public static bool operator >(Sample<T> a, Sample<T> b) => a.Value > b.Value;


    public static bool operator <(Sample<T> a, float b) => a.Value < b;
    public static bool operator <(float b, Sample<T> a) => b < a.Value;
    public static bool operator <(Sample<T> a, Sample<T> b) => a.Value < b.Value;
}