using System;
using UnityEngine;

namespace ZNoise
{
    [Serializable]
    public class RidgedNoise : Map
    {
        public RidgedNoise(int seed)
            : base(new BillowNoise(seed), x => -x, x => -x, x => -x)
        { }
    }
}