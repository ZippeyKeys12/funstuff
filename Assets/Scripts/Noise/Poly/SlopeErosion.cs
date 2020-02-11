using Unity.Mathematics;
using UnityEngine;

namespace Noise
{
    public class SlopeErosion : Generator
    {
        protected float lacunarity;
        protected float[] spectralWeights;
        protected Generator[] operands;

        public SlopeErosion(float lacunarity = 2, float persistance = .5f, params Generator[] operands)
            : this(lacunarity, persistance, .9f, operands)
        { }

        public SlopeErosion(float lacunarity = 2, float persistance = .5f, float spectralExponent = .9f, params Generator[] operands)
        {
            this.lacunarity = lacunarity;

            spectralWeights = new float[operands.Length];
            for (int i = 0; i < operands.Length; i++)
            {
                spectralWeights[i] = math.pow(persistance, i * spectralExponent);
            }

            this.operands = operands;
        }

        public override Sample<float> Get(float x)
        {
            var freq = 1f;
            var noiseHeight = Sample<float>.Zero;
            var gradient = 0f;

            for (var i = 0; i < operands.Length; i++)
            {
                var val = operands[i].Get(x * freq);

                gradient += val.Gradient;
                noiseHeight += val * spectralWeights[i] / (1 + math.lengthsq(gradient));

                freq *= lacunarity;
            }

            return noiseHeight;
        }

        public override Sample<float2> Get(float2 xy)
        {
            var freq = 1f;
            var noiseHeight = Sample<float2>.Zero;
            var gradient = float2.zero;

            for (var i = 0; i < operands.Length; i++)
            {
                var val = operands[i].Get(xy * freq);

                gradient += val.Gradient;
                noiseHeight += val * spectralWeights[i] / (1 + math.lengthsq(gradient));

                freq *= lacunarity;
            }

            return noiseHeight;
        }

        public override Sample<float3> Get(float3 xyz)
        {
            var freq = 1f;
            var noiseHeight = Sample<float3>.Zero;
            var gradient = float3.zero;

            for (var i = 0; i < operands.Length; i++)
            {
                var val = operands[i].Get(xyz * freq);

                gradient += val.Gradient;
                noiseHeight += val * spectralWeights[i] / (1 + math.lengthsq(gradient));

                freq *= lacunarity;
            }

            return noiseHeight;
        }
    }
}