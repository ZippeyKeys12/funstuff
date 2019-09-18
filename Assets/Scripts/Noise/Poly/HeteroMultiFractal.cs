//namespace Noise
//{
//    // TODO: Actually do
//    public class HeteroMultiFractal : Generator
//    {
//        private readonly float lacunarity, offset;
//        private readonly float[] spectralWeights;
//        private readonly Generator[] operands;

//        public HeteroMultiFractal(float lacunarity = 2, float persistance = .5f, params Generator[] operands)
//            : this(lacunarity, persistance, 1f, .9f, operands)
//        { }

//        public HeteroMultiFractal(float lacunarity = 2, float persistance = .5f, float offset = 1f, float spectralExponent = .9f, params Generator[] operands)
//        {
//            this.lacunarity = lacunarity;

//            spectralWeights = new float[operands.Length];
//            for (int i = 0; i < operands.Length; i++)
//            {
//                spectralWeights[i] = (float)System.Math.Pow(persistance, i * spectralExponent);
//            }

//            this.offset = offset;
//            this.operands = operands;
//        }

//        public override float Get(float x, float frequency)
//        {
//            var freq = frequency;
//            var noiseHeight = 1f;
//            var maxNoiseHeight = 1f;

//            for (var i = 0; i < operands.Length; i++)
//            {
//                noiseHeight *= operands[i].Get(x, freq) * spectralWeights[i] + offset;
//                maxNoiseHeight *= spectralWeights[i] + offset;

//                freq *= lacunarity;
//            }

//            return noiseHeight / maxNoiseHeight;
//        }

//        public override float Get(float x, float y, float frequency)
//        {
//            var freq = frequency;
//            var noiseHeight = 1f;
//            var maxNoiseHeight = 1f;

//            for (var i = 0; i < operands.Length; i++)
//            {
//                noiseHeight *= operands[i].Get(x, y, freq) * spectralWeights[i] + offset;
//                maxNoiseHeight *= spectralWeights[i] + offset;

//                freq *= lacunarity;
//            }

//            return noiseHeight / maxNoiseHeight;
//        }

//        public override float Get(float x, float y, float z, float frequency)
//        {
//            var freq = frequency;
//            var noiseHeight = 1f;
//            var maxNoiseHeight = 1f;



//            for (var i = 0; i < operands.Length; i++)
//            {
//                noiseHeight *= operands[i].Get(x, y, z, freq) * spectralWeights[i] + offset;
//                maxNoiseHeight *= spectralWeights[i] + offset;

//                freq *= lacunarity;
//            }

//            return noiseHeight / maxNoiseHeight;
//        }
//    }
//}
