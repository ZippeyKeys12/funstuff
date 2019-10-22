using UnityEngine;
using Unity.Mathematics;

namespace Noise
{
    class SinFractal : Generator
    {
        protected readonly float lacunarity;
        protected readonly float[] spectralWeights;
        protected readonly Generator[] operands;

        public SinFractal(float lacunarity = 2, float persistance = .5f, params Generator[] operands)
            : this(lacunarity, persistance, .9f, operands) { }

        public SinFractal(float lacunarity = 2f, float persistance = .5f, float spectralExponent = .9f, params Generator[] operands)
        {
            this.lacunarity = lacunarity;

            spectralWeights = new float[operands.Length];
            for (int i = 0; i < operands.Length; i++)
            {
                spectralWeights[i] = (float)System.Math.Pow(persistance, i * spectralExponent);
            }

            this.operands = operands;
        }

        public override Sample<float> Get(float x, float frequency)
        {
            var freq = frequency;
            var noiseHeight = Sample1D.Zero;
            var maxNoiseHeight = 0f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight += Maths.Abs(operands[i].Get(x, freq) * spectralWeights[i]) * 2 - 1;
                maxNoiseHeight += spectralWeights[i];

                freq *= lacunarity;
            }
            return Maths.Sin(x * frequency + noiseHeight);
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            var freq = frequency;
            var noiseHeight = Sample2D.Zero;
            var maxNoiseHeight = 0f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight += Maths.Abs(operands[i].Get(xy, freq) * spectralWeights[i]) * 2 - 1;
                maxNoiseHeight += spectralWeights[i];

                freq *= lacunarity;
            }

            return Maths.Sin(math.dot(xy, frequency) + noiseHeight);
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            var freq = frequency;
            var noiseHeight = Sample3D.Zero;
            var maxNoiseHeight = 0f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight += Maths.Abs(operands[i].Get(xyz, freq) * spectralWeights[i]) * 2 - 1;
                maxNoiseHeight += spectralWeights[i];

                freq *= lacunarity;
            }
            return Maths.Sin(math.dot(xyz, frequency) + noiseHeight);
        }
    }
}