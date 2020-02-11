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
                x => Maths.Clamp(gen.Get(x), min.Get(x), max.Get(x)),
                xy => Maths.Clamp(gen.Get(xy), min.Get(xy), max.Get(xy)),
                xyz => Maths.Clamp(gen.Get(xyz), min.Get(xyz), max.Get(xyz))
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
                var tempGen = gen;
                switch (type)
                {
                    case FilterType.Invert:
                        gen = new Function(
                            x => 1 - tempGen.Get(x),
                            xy => 1 - tempGen.Get(xy),
                            xyz => 1 - tempGen.Get(xyz));
                        break;

                    case FilterType.Billow:
                        gen = new Function(
                            x => Maths.Abs((2 * tempGen.Get(x)) - 1),
                            xy => Maths.Abs((2 * tempGen.Get(xy)) - 1),
                            xyz => Maths.Abs((2 * tempGen.Get(xyz)) - 1));
                        break;

                    case FilterType.Ridged:
                        gen = gen.Filter(FilterType.Billow, FilterType.Invert);
                        break;

                    case FilterType.Sin:
                        gen = new Function(
                            x => (Maths.Sin(2 * math.PI * tempGen.Get(x)) / 2) + .5f,
                            xy => (Maths.Sin(2 * math.PI * tempGen.Get(xy)) / 2) + .5f,
                            xyz => (Maths.Sin(2 * math.PI * tempGen.Get(xyz)) / 2) + .5f);
                        break;

                    case FilterType.Asin:
                        gen = new Function(
                            x => (Maths.Asin((2 * tempGen.Get(x)) - 1) / math.PI) + .5f,
                            xy => (Maths.Asin((2 * tempGen.Get(xy)) - 1) / math.PI) + .5f,
                            xyz => (Maths.Asin((2 * tempGen.Get(xyz)) - 1) / math.PI) + .5f);
                        break;

                    case FilterType.Atan:
                        gen = new Function(
                            x => (2 * Maths.Atan((2 * tempGen.Get(x)) - 1) / math.PI) + .5f,
                            xy => (2 * Maths.Atan((2 * tempGen.Get(xy)) - 1) / math.PI) + .5f,
                            xyz => (2 * Maths.Atan((2 * tempGen.Get(xyz)) - 1) / math.PI) + .5f);
                        break;

                    case FilterType.Tanh:
                        gen = new Function(
                            x => 2 * Maths.Tanh(2 * tempGen.Get(x) - 1) / 2 + .5f,
                            xy => 2 * Maths.Tanh(2 * tempGen.Get(xy) - 1) / 2 + .5f,
                            xyz => 2 * Maths.Tanh(2 * tempGen.Get(xyz) - 1) / 2 + .5f);
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
                x => Maths.Lerp(a.Get(x), b.Get(x), t.Get(x)),
                xy => Maths.Lerp(a.Get(xy), b.Get(xy), t.Get(xy)),
                xyz => Maths.Lerp(a.Get(xyz), b.Get(xyz), t.Get(xyz))
            );
        }
    }
}