using Unity.Mathematics;

namespace Noise
{
    public class FBM : Generator
    {
        protected readonly float lacunarity;
        protected readonly float[] spectralWeights;
        protected readonly Generator[] operands;

        public FBM(float lacunarity = 2, float persistance = .5f, params Generator[] operands)
            : this(lacunarity, persistance, .9f, operands) { }

        public FBM(float lacunarity = 2, float persistance = .5f, float spectralExponent = .9f, params Generator[] operands)
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
            var maxNoiseHeight = 0f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight += operands[i].Get(x * freq) * spectralWeights[i];
                maxNoiseHeight += spectralWeights[i];

                freq *= lacunarity;
            }

            return noiseHeight / maxNoiseHeight;
        }

        public override Sample<float2> Get(float2 xy)
        {
            var freq = 1f;
            var noiseHeight = Sample<float2>.Zero;
            var maxNoiseHeight = 0f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight += operands[i].Get(xy * freq) * spectralWeights[i];
                maxNoiseHeight += spectralWeights[i];

                freq *= lacunarity;
            }

            return noiseHeight / maxNoiseHeight;
        }

        public override Sample<float3> Get(float3 xyz)
        {
            var freq = 1f;
            var noiseHeight = Sample<float3>.Zero;
            var maxNoiseHeight = 0f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight += operands[i].Get(xyz * freq) * spectralWeights[i];
                maxNoiseHeight += spectralWeights[i];

                freq *= lacunarity;
            }

            return noiseHeight / maxNoiseHeight;
        }
    }
}
