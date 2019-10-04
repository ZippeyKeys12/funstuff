using System;
using UnityEngine;
using Unity.Jobs;

public interface GeneratorSample<T, R> : IJobParallelFor
    where T : unmanaged
{
    T Dimension { get; set; }
    float Frequency { get; set; }

    R Get(T x, float frequency);
}
public abstract class Sample<T>
{
    public float Value { get; }
    public T Gradient { get; }

    protected Sample(float value, T gradient)
    {
        Value = value;
        Gradient = gradient;
    }

    public abstract Sample<T> Of(float a);

    public abstract Sample<T> Apply(Func<Sample<float>, Sample<float>> f);

    public abstract Sample<T> Neg();
    public static Sample<T> operator -(Sample<T> a) => a.Neg();


    public abstract Sample<T> Add(float o);
    public static Sample<T> operator +(Sample<T> a, float b) => a.Add(b);
    public static Sample<T> operator +(float b, Sample<T> a) => a.Add(b);

    public abstract Sample<T> Add(Sample<T> o);
    public static Sample<T> operator +(Sample<T> a, Sample<T> b) => a.Add(b);


    public abstract Sample<T> Sub(float o);
    public static Sample<T> operator -(Sample<T> a, float b) => a.Sub(b);
    public static Sample<T> operator -(float b, Sample<T> a) => -a.Add(b);

    public abstract Sample<T> Sub(Sample<T> o);
    public static Sample<T> operator -(Sample<T> a, Sample<T> b) => a.Sub(b);


    public abstract Sample<T> Mul(float o);
    public static Sample<T> operator *(Sample<T> a, float b) => a.Mul(b);
    public static Sample<T> operator *(float b, Sample<T> a) => a.Mul(b);

    public abstract Sample<T> Mul(Sample<T> o);
    public static Sample<T> operator *(Sample<T> a, Sample<T> b) => a.Mul(b);


    public static Sample<T> operator /(Sample<T> a, float b) => a.Mul(1 / b);

    public abstract Sample<T> Div(Sample<T> o);
    public static Sample<T> operator /(Sample<T> a, Sample<T> b) => a.Div(b);


    public static bool operator >(Sample<T> a, float b) => a.Value > b;
    public static bool operator >(float b, Sample<T> a) => b > a.Value;
    public static bool operator >(Sample<T> a, Sample<T> b) => a.Value > b.Value;


    public static bool operator <(Sample<T> a, float b) => a.Value < b;
    public static bool operator <(float b, Sample<T> a) => b < a.Value;
    public static bool operator <(Sample<T> a, Sample<T> b) => a.Value < b.Value;


    public bool Equals(Sample<T> o)
        => Mathf.Approximately(Value, o.Value);
}

public class Sample1D : Sample<float>
{
    public static readonly Sample<float> Zero = new Sample1D(0);
    public static readonly Sample<float> One = new Sample1D(1);
    public static readonly Sample<float> MinValue = new Sample1D(float.MinValue);
    public static readonly Sample<float> MaxValue = new Sample1D(float.MaxValue);

    public Sample1D(float value)
        : base(value, 0) { }

    public Sample1D(float value, float derivative)
        : base(value, derivative) { }

    public static explicit operator Sample1D(float a)
    {
        return new Sample1D(a);
    }

    public override Sample<float> Of(float a)
    {
        return new Sample1D(a);
    }

    public override Sample<float> Apply(Func<Sample<float>, Sample<float>> f)
        => f(this);

    public override Sample<float> Neg()
        => new Sample1D(-Value, -Gradient);


    public override Sample<float> Add(float o)
        => new Sample1D(Value + o, Gradient);

    public override Sample<float> Add(Sample<float> o)
        => new Sample1D(Value + o.Value, Gradient + o.Gradient);


    public override Sample<float> Sub(float o)
        => new Sample1D(Value - o, Gradient);

    public override Sample<float> Sub(Sample<float> o)
        => new Sample1D(Value - o.Value, Gradient - o.Gradient);


    public override Sample<float> Mul(float o)
        => new Sample1D(Value * o, o * Gradient);

    public override Sample<float> Mul(Sample<float> o)
        => new Sample1D(Value * o.Value, o.Value * Gradient + Value * o.Gradient);

    public override Sample<float> Div(Sample<float> o)
        => new Sample1D(Value * o.Value, (o.Value * Gradient - Value * o.Gradient) / Mathf.Pow(o.Value, 2));
}

public class Sample2D : Sample<Vector2>
{
    public static readonly Sample<Vector2> Zero = new Sample2D(0);
    public static readonly Sample<Vector2> One = new Sample2D(1);
    public static readonly Sample<Vector2> MinValue = new Sample2D(float.MinValue);
    public static readonly Sample<Vector2> MaxValue = new Sample2D(float.MaxValue);

    public Sample2D(float value)
        : base(value, Vector2.zero) { }

    public Sample2D(float value, Vector2 derivative)
        : base(value, derivative) { }

    public static explicit operator Sample2D(float a)
        => new Sample2D(a);

    public override Sample<Vector2> Of(float a)
        => new Sample2D(a);

    public override Sample<Vector2> Apply(Func<Sample<float>, Sample<float>> f)
    {
        var rx = f(new Sample1D(Value, Gradient.x));
        var ry = f(new Sample1D(Value, Gradient.y));

        return new Sample2D(rx.Value, new Vector2(rx.Gradient, ry.Gradient));
    }

    public override Sample<Vector2> Neg()
        => new Sample2D(-Value, -Gradient);


    public override Sample<Vector2> Add(float o)
        => new Sample2D(Value + o, Gradient);

    public override Sample<Vector2> Add(Sample<Vector2> o)
        => new Sample2D(Value + o.Value, Gradient + o.Gradient);


    public override Sample<Vector2> Sub(float o)
        => new Sample2D(Value - o, Gradient);

    public override Sample<Vector2> Sub(Sample<Vector2> o)
        => new Sample2D(Value - o.Value, Gradient - o.Gradient);


    public override Sample<Vector2> Mul(float o)
        => new Sample2D(Value * o, o * Gradient);

    public override Sample<Vector2> Mul(Sample<Vector2> o)
        => new Sample2D(Value * o.Value, o.Value * Gradient + Value * o.Gradient);


    public override Sample<Vector2> Div(Sample<Vector2> o)
        => new Sample2D(Value * o.Value, (o.Value * Gradient - Value * o.Gradient) / Mathf.Pow(o.Value, 2));
}

public class Sample3D : Sample<Vector3>
{
    public static readonly Sample<Vector3> Zero = new Sample3D(0);
    public static readonly Sample<Vector3> One = new Sample3D(1);
    public static readonly Sample<Vector3> MinValue = new Sample3D(float.MinValue);
    public static readonly Sample<Vector3> MaxValue = new Sample3D(float.MaxValue);

    public Sample3D(float value)
        : base(value, Vector3.zero) { }

    public Sample3D(float value, Vector3 derivative)
        : base(value, derivative) { }

    public static explicit operator Sample3D(float a)
    {
        return new Sample3D(a);
    }

    public override Sample<Vector3> Of(float a)
    {
        return new Sample3D(a);
    }

    public override Sample<Vector3> Apply(Func<Sample<float>, Sample<float>> f)
    {
        var rx = f(new Sample1D(Value, Gradient.x));
        var ry = f(new Sample1D(Value, Gradient.y));
        var rz = f(new Sample1D(Value, Gradient.z));

        return new Sample3D(rx.Value, new Vector3(rx.Gradient, ry.Gradient, rz.Gradient));
    }

    public override Sample<Vector3> Neg()
        => new Sample3D(-Value, -Gradient);


    public override Sample<Vector3> Add(float o)
        => new Sample3D(Value + o, Gradient);

    public override Sample<Vector3> Add(Sample<Vector3> o)
        => new Sample3D(Value + o.Value, Gradient + o.Gradient);


    public override Sample<Vector3> Sub(float o)
        => new Sample3D(Value - o, Gradient);

    public override Sample<Vector3> Sub(Sample<Vector3> o)
        => new Sample3D(Value - o.Value, Gradient - o.Gradient);


    public override Sample<Vector3> Mul(float o)
        => new Sample3D(Value * o, o * Gradient);

    public override Sample<Vector3> Mul(Sample<Vector3> o)
        => new Sample3D(Value * o.Value, o.Value * Gradient + Value * o.Gradient);


    public override Sample<Vector3> Div(Sample<Vector3> o)
        => new Sample3D(Value * o.Value, (o.Value * Gradient - Value * o.Gradient) / Mathf.Pow(o.Value, 2));
}