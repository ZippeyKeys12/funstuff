using UnityEngine;
using Unity.Mathematics;

namespace Noise
{
    public class MultiFractal : Generator
    {
        private readonly float lacunarity, offset;
        private readonly float[] spectralWeights;
        private readonly Generator[] operands;

        public MultiFractal(float lacunarity = 2, float persistance = .5f, params Generator[] operands)
            : this(lacunarity, persistance, 1f, .9f, operands) { }

        public MultiFractal(float lacunarity = 2, float persistance = .5f, float offset = 1f, float spectralExponent = .9f, params Generator[] operands)
        {
            this.lacunarity = lacunarity;

            spectralWeights = new float[operands.Length];
            for (int i = 0; i < operands.Length; i++)
            {
                spectralWeights[i] = Mathf.Pow(persistance, i * spectralExponent);
            }

            this.offset = offset;
            this.operands = operands;
        }

        public override Sample<float> Get(float x, float frequency)
        {
            var freq = frequency;
            var noiseHeight = Sample1D.One;
            var maxNoiseHeight = 1f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight *= operands[i].Get(x, freq) * spectralWeights[i] + offset;
                maxNoiseHeight *= spectralWeights[i] + offset;

                freq *= lacunarity;
            }

            return noiseHeight / maxNoiseHeight;
        }

        public override Sample<float2> Get(float x, float y, float frequency)
        {
            var freq = frequency;
            var noiseHeight = Sample2D.One;
            var maxNoiseHeight = 1f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight *= operands[i].Get(x, y, freq) * spectralWeights[i] + offset;
                maxNoiseHeight *= spectralWeights[i] + offset;

                freq *= lacunarity;
            }

            return noiseHeight / maxNoiseHeight;
        }

        public override Sample<float3> Get(float x, float y, float z, float frequency)
        {
            var freq = frequency;
            var noiseHeight = Sample3D.One;
            var maxNoiseHeight = 1f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight *= operands[i].Get(x, y, z, freq) * spectralWeights[i] + offset;
                maxNoiseHeight *= spectralWeights[i] + offset;

                freq *= lacunarity;
            }

            return noiseHeight / maxNoiseHeight;
        }
    }
}
