﻿namespace ZNoise
{
    public class FBM : Generator
    {
        protected readonly float lacunarity;
        protected readonly float[] spectralWeights;
        protected readonly Generator[] operands;

        public FBM(float lacunarity = 2, float persistance = .5f, params Generator[] operands)
            : this(lacunarity, persistance, .9f, operands)
        { }

        public FBM(float lacunarity = 2, float persistance = .5f, float spectralExponent = .9f, params Generator[] operands)
        {
            this.lacunarity = lacunarity;

            spectralWeights = new float[operands.Length];
            for (int i = 0; i < operands.Length; i++)
            {
                spectralWeights[i] = (float)System.Math.Pow(persistance, i * spectralExponent);
            }

            this.operands = operands;
        }

        public override Sample1D Get(float x, float frequency)
        {
            var freq = frequency;
            var noiseHeight = Sample1D.zero;
            var maxNoiseHeight = 0f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight += operands[i].Get(x, freq) * spectralWeights[i];
                maxNoiseHeight += spectralWeights[i];

                freq *= lacunarity;
            }

            return noiseHeight / maxNoiseHeight;
        }

        public override Sample2D Get(float x, float y, float frequency)
        {
            var freq = frequency;
            var noiseHeight = Sample2D.zero;
            var maxNoiseHeight = 0f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight += operands[i].Get(x, y, freq) * spectralWeights[i];
                maxNoiseHeight += spectralWeights[i];

                freq *= lacunarity;
            }

            return noiseHeight / maxNoiseHeight;
        }

        public override Sample3D Get(float x, float y, float z, float frequency)
        {
            var freq = frequency;
            var noiseHeight = Sample3D.zero;
            var maxNoiseHeight = 0f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight += operands[i].Get(x, y, z, freq) * spectralWeights[i];
                maxNoiseHeight += spectralWeights[i];

                freq *= lacunarity;
            }

            return noiseHeight / maxNoiseHeight;
        }
    }
}
