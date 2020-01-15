using System;
using Unity.Mathematics;

namespace Noise
{
    public static class GeneratorExt
    {
        public static Generator AdjAdd(this Generator a, Generator b)
            => a + b - .5f;

        public static Generator Root(this Generator gen, float n)
        {
            return gen ^ (1 / n);
        }

        public static Generator Clamp(this Generator gen, Generator min, Generator max)
        {
            return new Function(
                (x, f) => Maths.Clamp(gen.Get(x, f), min.Get(x, f), max.Get(x, f)),
                (xy, f) => Maths.Clamp(gen.Get(xy, f), min.Get(xy, f), max.Get(xy, f)),
                (xyz, f) => Maths.Clamp(gen.Get(xyz, f), min.Get(xyz, f), max.Get(xyz, f))
            );
        }

        public static Generator Translate(this Generator gen, float3 translation)
        {
            return new Translation(gen, translation);
        }

        public static Generator Warp(this Generator gen, int levels)
        {
            return new Warp(gen, levels);
        }

        public static Generator Warp(this Generator gen, float strength, int levels)
        {
            return new Warp(gen, strength, levels);
        }

        public static Generator Filter(this Generator gen, params FilterType[] types)
        {
            foreach (var type in types)
            {
                switch (type)
                {
                    case FilterType.Invert:
                        gen = new Function(
                            (x, f) => 1 - gen.Get(x, f),
                            (xy, f) => 1 - gen.Get(xy, f),
                            (xyz, f) => 1 - gen.Get(xyz, f));
                        break;

                    case FilterType.Billow:
                        gen = new Function(
                            (x, f) => Maths.Abs(2 * gen.Get(x, f) - 1),
                            (xy, f) => Maths.Abs(2 * gen.Get(xy, f) - 1),
                            (xyz, f) => Maths.Abs(2 * gen.Get(xyz, f) - 1));
                        break;

                    case FilterType.Ridged:
                        gen = gen.Filter(FilterType.Billow).Filter(FilterType.Invert);
                        break;

                    case FilterType.Sin:
                        gen = new Function(
                            (x, f) => Maths.Sin(2 * math.PI * gen.Get(x, f)) / 2 + .5f,
                            (xy, f) => Maths.Sin(2 * math.PI * gen.Get(xy, f)) / 2 + .5f,
                            (xyz, f) => Maths.Sin(2 * math.PI * gen.Get(xyz, f)) / 2 + .5f);
                        break;

                    case FilterType.Asin:
                        gen = new Function(
                            (x, f) => Maths.Asin(2 * gen.Get(x, f) - 1) / math.PI + .5f,
                            (xy, f) => Maths.Asin(2 * gen.Get(xy, f) - 1) / math.PI + .5f,
                            (xyz, f) => Maths.Asin(2 * gen.Get(xyz, f) - 1) / math.PI + .5f);
                        break;

                    case FilterType.Atan:
                        gen = new Function(
                            (x, f) => 2 * Maths.Atan(2 * gen.Get(x, f) - 1) / math.PI + .5f,
                            (xy, f) => 2 * Maths.Atan(2 * gen.Get(xy, f) - 1) / math.PI + .5f,
                            (xyz, f) => 2 * Maths.Atan(2 * gen.Get(xyz, f) - 1) / math.PI + .5f);
                        break;

                    case FilterType.Tanh:
                        gen = new Function(
                            (x, f) => 2 * Maths.Tanh(2 * gen.Get(x, f) - 1) / 2 + .5f,
                            (xy, f) => 2 * Maths.Tanh(2 * gen.Get(xy, f) - 1) / 2 + .5f,
                            (xyz, f) => 2 * Maths.Tanh(2 * gen.Get(xyz, f) - 1) / 2 + .5f);
                        break;

                    default:
                        throw new ArgumentException($"Invalid filter type: '{type.ToString()}'");
                }
            }

            return gen;
        }
    }

    public enum FilterType
    {
        Invert,
        Billow,
        Ridged,
        Sin,
        Asin,
        Atan,
        Tanh,
    }

    public static class Mathg
    {
        public static Generator Lerp(Generator a, Generator b, Generator t)
        {
            return new Function(
                (x, f) => Maths.Lerp(a.Get(x, f), b.Get(x, f), t.Get(x, f)),
                (xy, f) => Maths.Lerp(a.Get(xy, f), b.Get(xy, f), t.Get(xy, f)),
                (xyz, f) => Maths.Lerp(a.Get(xyz, f), b.Get(xyz, f), t.Get(xyz, f))
            );
        }
    }
}