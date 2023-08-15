using Unity.Mathematics;

namespace Noise.Poly
{
    public class Multifractal : Generator
    {
        private readonly float lacunarity, offset;
        private readonly float[] spectralWeights;
        private readonly Generator[] operands;

        public Multifractal(float lacunarity = 2, float persistance = .5f, params Generator[] operands)
            : this(lacunarity, persistance, 1f, .9f, operands) { }

        public Multifractal(float lacunarity = 2, float persistance = .5f, float offset = 1f, float spectralExponent = .9f, params Generator[] operands)
        {
            this.lacunarity = lacunarity;

            spectralWeights = new float[operands.Length];
            for (int i = 0; i < operands.Length; i++)
            {
                spectralWeights[i] = math.pow(persistance, i * spectralExponent);
            }

            this.offset = offset;
            this.operands = operands;
        }

        public override Sample<float> Get(float x)
        {
            var freq = 1f;
            var noiseHeight = new Sample<float>(1, 1);
            var maxNoiseHeight = 1f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight *= (operands[i].Get(x * freq) * spectralWeights[i]) + offset;
                maxNoiseHeight *= spectralWeights[i] + offset;

                freq *= lacunarity;
            }

            return noiseHeight / maxNoiseHeight;
        }

        public override Sample<float2> Get(float2 xy)
        {
            var freq = 1f;
            var noiseHeight = new Sample<float2>(1, new float2(1, 1));
            var maxNoiseHeight = 1f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight *= (operands[i].Get(xy * freq) * spectralWeights[i]) + offset;
                maxNoiseHeight *= spectralWeights[i] + offset;

                freq *= lacunarity;
            }

            return noiseHeight / maxNoiseHeight;
        }

        public override Sample<float3> Get(float3 xyz)
        {
            var freq = 1f;
            var noiseHeight = new Sample<float3>(1, new float3(1, 1, 1));
            var maxNoiseHeight = 1f;

            for (var i = 0; i < operands.Length; i++)
            {
                noiseHeight *= (operands[i].Get(xyz * freq) * spectralWeights[i]) + offset;
                maxNoiseHeight *= spectralWeights[i] + offset;

                freq *= lacunarity;
            }

            return noiseHeight / maxNoiseHeight;
        }
    }
}
