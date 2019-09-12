using System;
using UnityEngine;

public static class Interp
{
    public delegate float TweenType(float x);
    public delegate Sample1D TweenType1D(Sample1D x);
    public delegate Sample2D TweenType2D(Sample2D x);
    public delegate Sample3D TweenType3D(Sample3D x);
    public static class TweenTypes
    {
        public static readonly TweenType Step = x => x < .5f ? 0 : 1;

        public static TweenType1D Step1D()
        {
            return x => Sample1D.Constant(Step(x.Value));
        }

        public static TweenType2D Step2D()
        {
            return x => Sample2D.Constant(Step(x.Value));
        }

        public static TweenType3D Step3D()
        {
            return x => Sample3D.Constant(Step(x.Value));
        }

        public static readonly TweenType Linear = x => x;
        public static readonly TweenType Quadratic = Polynomial(2);
        public static readonly TweenType Cubic = Polynomial(3);
        public static readonly TweenType Exponential = x => Mathf.Exp(10 * (x - 1));
        public static readonly TweenType Logarithmic = x => -Mathf.Log(1 - .9f * x);
        public static readonly TweenType Inverse = x => (x + 1) / (1 - x);
        public static readonly TweenType Sinusoidal = Reverse(x => Mathf.Sin(Mathf.PI / 2 * x));
        public static readonly TweenType Circular = x => -Mathf.Sqrt(1 - Mathf.Pow(x, 2)) + 1;
        public static readonly TweenType SmoothStep = NSmoothStep(1);
        public static readonly TweenType SmootherStep = NSmoothStep(2);
        public static readonly TweenType SmoothestStep = NSmoothStep(3);

        public static TweenType Polynomial(float n)
        {
            return x => Mathf.Pow(x, n);
        }

        public static TweenType NSmoothStep(int N)
        {
            return x =>
            {
                var result = 0f;
                for (var n = 0; n <= N; ++n)
                    result += MathZ.nCr(-N - 1, n) *
                              MathZ.nCr(2 * N + 1, N - n) *
                              Mathf.Pow(x, N + n + 1);
                return result;
            };
        }

        public static TweenType Power(float n)
        {
            return x => Mathf.Pow(n, 10 * (x - 1));
        }

        public static TweenType NRoot(float n)
        {
            if (Mathf.Approximately(n, 2))
            {
                return Circular;
            }

            return x => -Mathf.Pow(1 - Mathf.Pow(x, n), 1 / n) + 1;
        }

        public static TweenType LogN(float n)
        {
            if (Mathf.Approximately(n, 10))
            {
                return x => -Mathf.Log10(1 - .9f * x);
            }

            if (Mathf.Approximately(n, Mathf.Exp(1)))
            {
                return Logarithmic;
            }

            return x => -Mathf.Log(1 - .9f * x, n);
        }
    }

    public delegate TweenType BlendType(TweenType a, TweenType b);
    public static class BlendTypes
    {
        public static readonly BlendType Step = (a, b) => t => t < .5f ? a(t) : b(t);
        public static readonly BlendType Linear = (a, b) => t => Mathf.Lerp(a(t), b(t), t);
        public static readonly BlendType Ease = (a, b) => t => t < .5f ? a(2 * t) / 2 : b(2 * t) / 2;

        public static BlendType Polynomial(float n)
        {
            return (a, b) => t => (1 - Mathf.Pow(t, n)) * a(t) + Mathf.Pow(t, n) * b(t);
        }

        public static TweenType EaseBetween(params TweenType[] tweens)
        {
            return t =>
           {
               var len = tweens.Length;

               for (var i = 0; i < len; i++)
               {
                   if (t < i / len)
                   {
                       return tweens[i](len * t) / len;
                   }
               }

               return tweens[len - 1](len * t) / len;
           };
        }
    }

    public static float Tween(float x, TweenType tween)
    {
        return Tween(x, tween, tween, BlendTypes.Linear);
    }

    public static float Tween(float x, TweenType tween, BlendType blend)
    {
        return Tween(x, tween, tween, blend);
    }

    public static float Tween(float x, TweenType start, TweenType stop)
    {
        return Tween(x, start, stop, BlendTypes.Linear);
    }

    public static float Tween(float x, TweenType start, TweenType stop, BlendType blend)
    {
        if (x <= 0)
        {
            return 0;
        }

        if (x >= 1)
        {
            return 1;
        }

        return blend(start, stop)(x);
    }

    public static Vector2 Tween(Vector2 xy, TweenType tween)
    {
        return new Vector2(Tween(xy.x, tween, BlendTypes.Linear), Tween(xy.y, tween, BlendTypes.Linear));
    }

    public static Vector2 Tween(Vector2 xy, TweenType tween, BlendType blend)
    {
        return new Vector2(Tween(xy.x, tween, blend), Tween(xy.y, tween, blend));
    }

    public static Vector2 Tween(Vector2 xy, TweenType start, TweenType stop)
    {
        return new Vector2(Tween(xy.x, start, stop, BlendTypes.Linear), Tween(xy.y, start, stop, BlendTypes.Linear));
    }

    public static Vector2 Tween(Vector2 xy, TweenType start, TweenType stop, BlendType blend)
    {
        return new Vector2(Tween(xy.x, start, stop, blend), Tween(xy.y, start, stop, blend));
    }

    public static Vector2 Tween(Vector2 xy, TweenType startX, TweenType stopX,
                                            TweenType startY, TweenType stopY,
                                            BlendType blend)
    {
        return new Vector2(Tween(xy.x, startX, stopX, blend), Tween(xy.y, startY, stopY, blend));
    }

    public static Vector2 Tween(Vector2 xy, TweenType startX, TweenType stopX, BlendType blendX,
                                            TweenType startY, TweenType stopY, BlendType blendY)
    {
        return new Vector2(Tween(xy.x, startX, stopX, blendX), Tween(xy.y, startY, stopY, blendY));
    }

    public static TweenType VerticalFlip(TweenType a)
    {
        return x => 1 - a(x);
    }

    public static TweenType HorizontalFlip(TweenType a)
    {
        return x => a(1 - x);
    }

    public static TweenType Reverse(TweenType a)
    {
        return x => 1 - a(1 - x);
    }
}