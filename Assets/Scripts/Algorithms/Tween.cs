﻿using UnityEngine;
using Noise;

public static class Interp {
    public delegate Sample<float> TweenType(Sample<float> x);

    public static class TweenTypes {
        public static readonly TweenType Step =
            x => new Sample1D(x < .5f ? 0 : 1);

        public static readonly TweenType Linear =
            x => new Sample1D(x.Value, x.Gradient);

        public static TweenType Polynomial(float n)
            => x => Maths.Pow(Maths.Clamp01(x), n);

        public static TweenType NSmoothStep(int N)
            => x => {
                var result = 0f;
                var deriv = 0f;
                for(var n = 0; n <= N; ++n) {
                    var inc = MathZ.nCr(-N - 1, n) *
                              MathZ.nCr(2 * N + 1, N - n) *
                              Mathf.Pow(x.Value, N + n + 1);
                    result += inc;
                    deriv += N + n * inc / x.Value;
                }
                return new Sample1D(result, deriv * x.Gradient);
            };

        public static readonly TweenType Exponential =
            x => Maths.Exp(10 * (x - 1));

        public static readonly TweenType Logarithmic =
            x => -Maths.Log(1 - .9f * x);

        public static readonly TweenType Inverse =
            x => (x + 1) / (1 - x);

        public static readonly TweenType Sinusoidal =
            Reverse(x => Maths.Sin(Mathf.PI / 2 * x));

        public static readonly TweenType Circular =
            x => -Maths.Sqrt(1 - Maths.Pow(x, 2)) + 1;

        public static readonly TweenType SmoothStep = NSmoothStep(1);
        public static readonly TweenType SmootherStep = NSmoothStep(2);
        public static readonly TweenType SmoothestStep = NSmoothStep(3);

        public static TweenType Power(float n) {
            return x => Maths.Pow(n, 10 * (x - 1));
        }

        public static TweenType NRoot(float n) {
            if(Mathf.Approximately(n, 2)) {
                return Circular;
            }

            return x => -Maths.Pow(1 - Maths.Pow(x, n), 1 / n) + 1;
        }

        public static TweenType LogN(float n) {
            if(Mathf.Approximately(n, 10)) {
                return x => -Maths.Log10(1 - .9f * x);
            }

            if(Mathf.Approximately(n, Mathf.Exp(1))) {
                return Logarithmic;
            }

            return x => -Maths.Log(1 - .9f * x, n);
        }
    }

    public delegate TweenType BlendType(TweenType a, TweenType b);
    public static class BlendTypes {
        public static readonly BlendType Step = (a, b) => t => t < .5f ? a(t) : b(t);
        public static readonly BlendType Linear = (a, b) => t => Maths.Lerp(a(t), b(t), t);
        public static readonly BlendType Ease = (a, b) => t => t < .5f ? a(2 * t) / 2 : b(2 * t) / 2;

        public static BlendType Polynomial(float n) {
            return (a, b) => t => (1 - Maths.Pow(t, n)) * a(t) + Maths.Pow(t, n) * b(t);
        }

        public static TweenType EaseBetween(params TweenType[] tweens) {
            return t => {
                var len = tweens.Length;

                for(var i = 0; i < len; i++) {
                    if(t < i / len) {
                        return tweens[i](len * t) / len;
                    }
                }

                return tweens[len - 1](len * t) / len;
            };
        }
    }

    public static Sample<float> Tween(Sample<float> x, TweenType tween) {
        return Tween(x, tween, tween, BlendTypes.Linear);
    }

    public static Sample<float> Tween(Sample<float> x, TweenType tween, BlendType blend) {
        return Tween(x, tween, tween, blend);
    }

    public static Sample<float> Tween(Sample<float> x, TweenType start, TweenType stop) {
        return Tween(x, start, stop, BlendTypes.Linear);
    }

    public static Sample<float> Tween(Sample<float> x, TweenType start, TweenType stop, BlendType blend) {
        if(x < 0) {
            return x.Of(0);
        }

        if(x > 1) {
            return x.Of(1);
        }

        return blend(start, stop)(x);
    }

    public static Vector2 Tween(Vector2 xy, TweenType tween) {
        return new Vector2(Tween((Sample1D)xy.x, tween, BlendTypes.Linear).Value,
                           Tween((Sample1D)xy.y, tween, BlendTypes.Linear).Value);
    }

    public static Vector2 Tween(Vector2 xy, TweenType tween, BlendType blend) {
        return new Vector2(Tween((Sample1D)xy.x, tween, blend).Value,
                           Tween((Sample1D)xy.y, tween, blend).Value);
    }

    public static Vector2 Tween(Vector2 xy, TweenType start, TweenType stop) {
        return new Vector2(Tween((Sample1D)xy.x, start, stop, BlendTypes.Linear).Value,
                           Tween((Sample1D)xy.y, start, stop, BlendTypes.Linear).Value);
    }

    public static Vector2 Tween(Vector2 xy, TweenType start, TweenType stop, BlendType blend) {
        return new Vector2(Tween((Sample1D)xy.x, start, stop, blend).Value,
                           Tween((Sample1D)xy.y, start, stop, blend).Value);
    }

    public static Vector2 Tween(Vector2 xy, TweenType startX, TweenType stopX,
                                            TweenType startY, TweenType stopY,
                                            BlendType blend) {
        return new Vector2(Tween((Sample1D)xy.x, startX, stopX, blend).Value,
                           Tween((Sample1D)xy.y, startY, stopY, blend).Value);
    }

    public static Vector2 Tween(Vector2 xy, TweenType startX, TweenType stopX, BlendType blendX,
                                            TweenType startY, TweenType stopY, BlendType blendY) {
        return new Vector2(Tween((Sample1D)xy.x, startX, stopX, blendX).Value,
                           Tween((Sample1D)xy.y, startY, stopY, blendY).Value);
    }

    public static TweenType VerticalFlip(TweenType a) {
        return x => 1 - a(x);
    }

    public static TweenType HorizontalFlip(TweenType a) {
        return x => a(1 - x);
    }

    public static TweenType Reverse(TweenType a) {
        return x => 1 - a(1 - x);
    }
}