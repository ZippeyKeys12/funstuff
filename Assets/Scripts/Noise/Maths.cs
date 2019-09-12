using UnityEngine;

namespace ZNoise
{
    public static class Maths
    {
        public static Sample1D Pow(Sample1D a, Sample1D b)
        {
            return new Sample1D(Mathf.Pow(a.Value, b.Value),
                Mathf.Pow(a.Value, b.Value - 1) * (b.Value * a.Gradient + a.Value * Mathf.Log10(a.Value) * b.Gradient));
        }

        public static Sample2D Pow(Sample2D a, Sample2D b)
        {
            return new Sample2D(Mathf.Pow(a.Value, b.Value),
                Mathf.Pow(a.Value, b.Value - 1) * (b.Value * a.Gradient + a.Value * Mathf.Log10(a.Value) * b.Gradient));
        }

        public static Sample3D Pow(Sample3D a, Sample3D b)
        {
            return new Sample3D(Mathf.Pow(a.Value, b.Value),
                Mathf.Pow(a.Value, b.Value - 1) * (b.Value * a.Gradient + a.Value * Mathf.Log10(a.Value) * b.Gradient));
        }

        public static Sample3D Sqrt(Sample3D a)
        {
            return new Sample3D(Mathf.Sqrt(a.Value), a.Gradient / (2 * Mathf.Sqrt(a.Value)));
        }

        public static Sample1D Sin(Sample1D a)
        {
            return new Sample1D(Mathf.Sin(a.Value), a.Gradient * Mathf.Cos(a.Value));
        }

        public static Sample2D Sin(Sample2D a)
        {
            return new Sample2D(Mathf.Sin(a.Value), a.Gradient * Mathf.Cos(a.Value));
        }

        public static Sample3D Sin(Sample3D a)
        {
            return new Sample3D(Mathf.Sin(a.Value), a.Gradient * Mathf.Cos(a.Value));
        }

        public static Sample1D Cos(Sample1D a)
        {
            return new Sample1D(Mathf.Cos(a.Value), -a.Gradient * Mathf.Sin(a.Value));
        }

        public static Sample2D Cos(Sample2D a)
        {
            return new Sample2D(Mathf.Cos(a.Value), -a.Gradient * Mathf.Sin(a.Value));
        }

        public static Sample3D Cos(Sample3D a)
        {
            return new Sample3D(Mathf.Cos(a.Value), -a.Gradient * Mathf.Sin(a.Value));
        }

        public static Sample1D Tan(Sample1D a)
        {
            return new Sample1D(Mathf.Tan(a.Value), a.Gradient * Mathf.Pow(1 / Mathf.Cos(a.Value), 2));
        }

        public static Sample2D Tan(Sample2D a)
        {
            return new Sample2D(Mathf.Tan(a.Value), a.Gradient * Mathf.Pow(1 / Mathf.Cos(a.Value), 2));
        }

        public static Sample3D Tan(Sample3D a)
        {
            return new Sample3D(Mathf.Tan(a.Value), a.Gradient * Mathf.Pow(1 / Mathf.Cos(a.Value), 2));
        }

        public static Sample1D Log(Sample1D a)
        {
            return new Sample1D(Mathf.Log(a.Value), a.Gradient / a.Value);
        }

        public static Sample1D Log(Sample1D a, Sample1D b)
        {
            return new Sample1D(Mathf.Log(a.Value, b.Value),
                (a.Gradient * Mathf.Log(b.Value) / a.Value - b.Gradient * Mathf.Log(a.Value) / b.Value)
                / Mathf.Pow(Mathf.Log(b.Value), 2));
        }

        public static Sample2D Log(Sample2D a)
        {
            return new Sample2D(Mathf.Log(a.Value), a.Gradient / a.Value);
        }

        public static Sample2D Log(Sample2D a, Sample2D b)
        {
            return new Sample2D(Mathf.Log(a.Value, b.Value),
                (a.Gradient * Mathf.Log(b.Value) / a.Value - b.Gradient * Mathf.Log(a.Value) / b.Value)
                / Mathf.Pow(Mathf.Log(b.Value), 2));
        }

        public static Sample3D Log(Sample3D a)
        {
            return new Sample3D(Mathf.Log(a.Value), a.Gradient / a.Value);
        }

        public static Sample3D Log(Sample3D a, Sample3D b)
        {
            return new Sample3D(Mathf.Log(a.Value, b.Value),
                (a.Gradient * Mathf.Log(b.Value) / a.Value - b.Gradient * Mathf.Log(a.Value) / b.Value)
                / Mathf.Pow(Mathf.Log(b.Value), 2));
        }

        public static Sample1D Abs(Sample1D a)
        {
            return new Sample1D(Mathf.Abs(a.Value), a.Value * a.Gradient / Mathf.Abs(a.Value));
        }

        public static Sample2D Abs(Sample2D a)
        {
            return new Sample2D(Mathf.Abs(a.Value), a.Value * a.Gradient / Mathf.Abs(a.Value));
        }

        public static Sample3D Abs(Sample3D a)
        {
            return new Sample3D(Mathf.Abs(a.Value), a.Value * a.Gradient / Mathf.Abs(a.Value));
        }

        public static Sample1D Max(params Sample1D[] samples)
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

        public static Sample2D Max(params Sample2D[] samples)
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

        public static Sample3D Max(params Sample3D[] samples)
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

        public static Sample1D Min(params Sample1D[] samples)
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

        public static Sample2D Min(params Sample2D[] samples)
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

        public static Sample3D Min(params Sample3D[] samples)
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
