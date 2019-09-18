using System;

namespace Noise
{
    [Serializable]
    public class BillowNoise : Map
    {
        public BillowNoise(int seed)
            : base(new PerlinNoise(seed), x => 2 * Maths.Abs(x) - 1, x => 2 * Maths.Abs(x) - 1, x => 2 * Maths.Abs(x) - 1)
        { }
    }
}