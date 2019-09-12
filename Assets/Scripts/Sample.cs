using UnityEngine;

public interface Sample<T>
{
    Sample<T> Zero { get; }
    Sample<T> One { get; }
    Sample<T> MinValue { get; }
    Sample<T> MaxValue { get; }

    float Value { get; }
    T Gradient { get; }
}

public struct Sample1D : Sample<float>
{
    public static readonly Sample1D zero = Constant(0);
    public static readonly Sample1D one = Constant(1);
    public static readonly Sample1D MinValue = Constant(float.MinValue);
    public static readonly Sample1D MaxValue = Constant(float.MaxValue);

    private readonly float value;
    private readonly float derivative;

    public float Value => value;

    public float Gradient => derivative;

    Sample<float> Sample<float>.Zero => zero;

    Sample<float> Sample<float>.One => one;

    Sample<float> Sample<float>.MinValue => MinValue;

    Sample<float> Sample<float>.MaxValue => MaxValue;

    public Sample1D(float value, float derivative)
    {
        this.value = value;
        this.derivative = derivative;
    }

    public static Sample1D Constant(float a)
    {
        return new Sample1D(a, 0);
    }

    public static implicit operator Sample1D(float f)
    {
        return Constant(f);
    }

    public static Sample1D operator -(Sample1D a)
    {
        return new Sample1D(-a.value, -a.derivative);
    }

    public static Sample1D operator +(Sample1D a, Sample1D b)
    {
        return new Sample1D(a.value + b.value, a.derivative + b.derivative);
    }

    public static Sample1D operator -(Sample1D a, Sample1D b)
    {
        return new Sample1D(a.value - b.value, a.derivative - b.derivative);
    }

    public static Sample1D operator *(Sample1D a, Sample1D b)
    {
        return new Sample1D(a.value * b.value, b.value * a.derivative + a.value * b.derivative);
    }

    public static Sample1D operator *(Sample1D a, float b)
    {
        return new Sample1D(a.value * b, a.derivative * b);
    }

    public static Sample1D operator *(float b, Sample1D a)
    {
        return a * b;
    }

    public static Sample1D operator /(Sample1D a, Sample1D b)
    {
        return new Sample1D(a.value * b.value, (b.value * a.derivative - a.value * b.derivative) / Mathf.Pow(b.value, 2));
    }

    public static Sample1D operator /(Sample1D a, float b)
    {
        return new Sample1D(a.value / b, a.derivative / b);
    }

    public static Sample1D operator /(float b, Sample1D a)
    {
        return new Sample1D(b / a.value, -b * a.derivative / Mathf.Pow(a.value, 2));
    }

    public static bool operator <(Sample1D a, Sample1D b)
    {
        return a.value < b.value;
    }

    public static bool operator <(Sample1D a, float b)
    {
        return a.value < b;
    }

    public static bool operator <(float b, Sample1D a)
    {
        return b < a.value;
    }

    public static bool operator >(Sample1D a, Sample1D b)
    {
        return a.value > b.value;
    }

    public static bool operator >(Sample1D a, float b)
    {
        return a.value > b;
    }

    public static bool operator >(float b, Sample1D a)
    {
        return b > a.value;
    }
}

public struct Sample2D : Sample<Vector2>
{
    public static readonly Sample2D zero = Constant(0);
    public static readonly Sample2D one = Constant(1);
    public static readonly Sample2D MinValue = Constant(float.MinValue);
    public static readonly Sample2D MaxValue = Constant(float.MaxValue);

    private readonly float value;
    private readonly Vector2 gradient;

    public float Value => value;

    public Vector2 Gradient => gradient;

    Sample<Vector2> Sample<Vector2>.Zero => zero;

    Sample<Vector2> Sample<Vector2>.One => one;

    Sample<Vector2> Sample<Vector2>.MinValue => MinValue;

    Sample<Vector2> Sample<Vector2>.MaxValue => MaxValue;

    public Sample2D(float value, Vector2 gradient)
    {
        this.value = value;
        this.gradient = gradient;
    }

    public static Sample2D Constant(float a)
    {
        return new Sample2D(a, Vector2.zero);
    }

    public static implicit operator Sample2D(float f)
    {
        return Constant(f);
    }

    public static Sample2D operator -(Sample2D a)
    {
        return new Sample2D(-a.value, -a.gradient);
    }

    public static Sample2D operator +(Sample2D a, Sample2D b)
    {
        return new Sample2D(a.value + b.value, a.gradient + b.gradient);
    }

    public static Sample2D operator +(Sample2D a, float b)
    {
        return new Sample2D(a.value + b, a.gradient);
    }

    public static Sample2D operator +(float b, Sample2D a)
    {
        return a + b;
    }

    public static Sample2D operator -(Sample2D a, Sample2D b)
    {
        return new Sample2D(a.value - b.value, a.gradient - b.gradient);
    }

    public static Sample2D operator -(Sample2D a, float b)
    {
        return new Sample2D(a.value - b, a.gradient);
    }

    public static Sample2D operator -(float b, Sample2D a)
    {
        return b + -a;
    }

    public static Sample2D operator *(Sample2D a, Sample2D b)
    {
        return new Sample2D(a.value * b.value, b.value * a.gradient + a.value * b.gradient);
    }

    public static Sample2D operator *(Sample2D a, float b)
    {
        return new Sample2D(a.value * b, a.gradient * b);
    }

    public static Sample2D operator *(float b, Sample2D a)
    {
        return a * b;
    }

    public static Sample2D operator /(Sample2D a, Sample2D b)
    {
        return new Sample2D(a.value * b.value, (b.value * a.gradient - a.value * b.gradient) / Mathf.Pow(b.value, 2));
    }

    public static Sample2D operator /(Sample2D a, float b)
    {
        return new Sample2D(a.value / b, a.gradient / b);
    }

    public static Sample2D operator /(float b, Sample2D a)
    {
        return new Sample2D(b / a.value, -b * a.gradient / Mathf.Pow(a.value, 2));
    }

    public static bool operator <(Sample2D a, Sample2D b)
    {
        return a.value < b.value;
    }

    public static bool operator <(Sample2D a, float b)
    {
        return a.value < b;
    }

    public static bool operator <(float b, Sample2D a)
    {
        return b < a.value;
    }

    public static bool operator >(Sample2D a, Sample2D b)
    {
        return a.value > b.value;
    }

    public static bool operator >(Sample2D a, float b)
    {
        return a.value > b;
    }

    public static bool operator >(float b, Sample2D a)
    {
        return b > a.value;
    }
}

public struct Sample3D : Sample<Vector3>
{
    public static readonly Sample3D zero = Constant(0);
    public static readonly Sample3D one = Constant(1);
    public static readonly Sample3D MinValue = Constant(float.MinValue);
    public static readonly Sample3D MaxValue = Constant(float.MaxValue);

    private readonly float value;
    private readonly Vector3 gradient;

    public float Value => value;

    public Vector3 Gradient => gradient;

    Sample<Vector3> Sample<Vector3>.Zero => zero;

    Sample<Vector3> Sample<Vector3>.One => one;

    Sample<Vector3> Sample<Vector3>.MinValue => MinValue;

    Sample<Vector3> Sample<Vector3>.MaxValue => MaxValue;

    public Sample3D(float value, Vector3 gradient)
    {
        this.value = value;
        this.gradient = gradient;
    }

    public static Sample3D Constant(float a)
    {
        return new Sample3D(a, Vector3.zero);
    }

    public static implicit operator Sample3D(float f)
    {
        return Constant(f);
    }

    public static Sample3D operator -(Sample3D a)
    {
        return new Sample3D(-a.value, -a.gradient);
    }

    public static Sample3D operator +(Sample3D a, Sample3D b)
    {
        return new Sample3D(a.value + b.value, a.gradient + b.gradient);
    }

    public static Sample3D operator +(Sample3D a, float b)
    {
        return new Sample3D(a.value + b, a.gradient);
    }

    public static Sample3D operator +(float b, Sample3D a)
    {
        return a + b;
    }

    public static Sample3D operator -(Sample3D a, Sample3D b)
    {
        return new Sample3D(a.value - b.value, a.gradient - b.gradient);
    }

    public static Sample3D operator -(Sample3D a, float b)
    {
        return new Sample3D(a.value - b, a.gradient);
    }

    public static Sample3D operator -(float b, Sample3D a)
    {
        return b + -a;
    }

    public static Sample3D operator *(Sample3D a, Sample3D b)
    {
        return new Sample3D(a.value * b.value, b.value * a.gradient + a.value * b.gradient);
    }

    public static Sample3D operator *(Sample3D a, float b)
    {
        return new Sample3D(a.value * b, a.gradient * b);
    }

    public static Sample3D operator *(float b, Sample3D a)
    {
        return a * b;
    }

    public static Sample3D operator /(Sample3D a, Sample3D b)
    {
        return new Sample3D(a.value * b.value, (b.value * a.gradient - a.value * b.gradient) / Mathf.Pow(b.value, 2));
    }

    public static Sample3D operator /(Sample3D a, float b)
    {
        return new Sample3D(a.value / b, a.gradient / b);
    }

    public static Sample3D operator /(float b, Sample3D a)
    {
        return new Sample3D(b / a.value, -b * a.gradient / Mathf.Pow(a.value, 2));
    }

    public static bool operator <(Sample3D a, Sample3D b)
    {
        return a.value < b.value;
    }

    public static bool operator <(Sample3D a, float b)
    {
        return a.value < b;
    }

    public static bool operator <(float b, Sample3D a)
    {
        return b < a.value;
    }

    public static bool operator >(Sample3D a, Sample3D b)
    {
        return a.value > b.value;
    }

    public static bool operator >(Sample3D a, float b)
    {
        return a.value > b;
    }

    public static bool operator >(float b, Sample3D a)
    {
        return b > a.value;
    }
}