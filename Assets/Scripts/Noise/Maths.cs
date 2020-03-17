using Unity.Mathematics;

public static class Maths
{
    public static Sample<T> Sign<T>(Sample<T> a)
    {
        if (a < 0)
        {
            return new Sample<T>(-1, Sample<T>.ZeroGradient());
        }
        else if (a > 0)
        {
            return new Sample<T>(1, Sample<T>.ZeroGradient());
        }

        return new Sample<T>(0, Sample<T>.ZeroGradient());

    }

    public static Sample<T> Lerp<T>(Sample<T> a, Sample<T> b, Sample<T> t)
    {
        return a + ((b - a) * Clamp01(t));
    }

    public static Sample<T> Lerp<T>(Sample<T> a, Sample<T> b, float t)
    {
        return a + (b - a) * math.clamp(t, 0, 1);
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
        return new Sample<T>(math.floor(v.Value), Sample<T>.ZeroGradient());
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
        return new Sample<T>(math.pow(a.Value, b.Value),
            math.pow(a.Value, b.Value - 1) * (b.Value * (dynamic)a.Gradient + a.Value * math.log10(a.Value) * (dynamic)b.Gradient));
    }

    public static Sample<T> Pow<T>(float a, Sample<T> b)
    {
        return new Sample<T>(math.pow(a, b.Value), math.log(a) * math.pow(a, b.Value) * (dynamic)b.Gradient);
    }

    public static Sample<T> Pow<T>(Sample<T> a, float b)
    {
        return new Sample<T>(math.pow(a.Value, b), math.pow(a.Value, b - 1) * b * (dynamic)a.Gradient);
    }

    public static Sample<T> Exp<T>(Sample<T> a)
    {
        return new Sample<T>(math.exp(a.Value), math.exp(a.Value) * (dynamic)a.Gradient);
    }

    public static Sample<T> Sqrt<T>(Sample<T> a)
    {
        return new Sample<T>(math.sqrt(a.Value), (dynamic)a.Gradient / (2 * math.sqrt(a.Value)));
    }

    public static Sample<T> Sin<T>(Sample<T> a)
    {
        return new Sample<T>(math.sin(a.Value), (dynamic)a.Gradient * math.cos(a.Value));
    }

    public static Sample<T> Asin<T>(Sample<T> a)
    {
        return new Sample<T>(math.asin(a.Value), (dynamic)a.Gradient / math.sqrt(1 - math.pow(a.Value, 2)));
    }

    public static Sample<T> Cos<T>(Sample<T> a)
    {
        return new Sample<T>(math.cos(a.Value), -(dynamic)a.Gradient * math.sin(a.Value));
    }

    public static Sample<T> Acos<T>(Sample<T> a)
    {
        return new Sample<T>(math.acos(a.Value), -(dynamic)a.Gradient / math.sqrt(1 - math.pow(a.Value, 2)));
    }

    public static Sample<T> Tan<T>(Sample<T> a)
    {
        return new Sample<T>(math.tan(a.Value), (dynamic)a.Gradient * math.pow(1 / math.cos(a.Value), 2));
    }

    public static Sample<T> Atan<T>(Sample<T> a)
    {
        return new Sample<T>(math.atan(a.Value), (dynamic)a.Gradient / (1 + math.pow(a.Value, 2)));
    }

    public static Sample<T> Tanh<T>(Sample<T> a)
    {
        return new Sample<T>(math.tanh(a.Value), (dynamic)a.Gradient / math.cosh(a.Value));
    }

    public static Sample<T> Acot<T>(Sample<T> a)
    {
        return Atan(1 / a);
    }

    public static Sample<T> Log<T>(Sample<T> a)
    {
        return new Sample<T>(math.log(a.Value), (dynamic)a.Gradient / a.Value);
    }

    public static Sample<T> Log10<T>(Sample<T> a)
    {
        return new Sample<T>(math.log10(a.Value), (dynamic)a.Gradient / (math.log(10) * a.Value));
    }

    public static Sample<T> Log<T>(Sample<T> b, Sample<T> x)
    {
        return new Sample<T>(math.log(x.Value) / math.log(b.Value),
            (((dynamic)x.Gradient * math.log(b.Value) / x.Value)
            - ((dynamic)b.Gradient * math.log(x.Value) / b.Value))
                / math.pow(math.log(b.Value), 2));
    }

    public static Sample<T> Log<T>(float b, Sample<T> x)
    {
        return new Sample<T>(math.log(x.Value) / math.log(b),
            (dynamic)x.Gradient / (x.Value * math.log(b)));
    }

    public static Sample<T> Abs<T>(Sample<T> a)
    {
        if (a.Value == 0)
        {
            return new Sample<T>(0, (dynamic)float.NaN);
        }

        return new Sample<T>(math.abs(a.Value), a.Value * (dynamic)a.Gradient / math.abs(a.Value));
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