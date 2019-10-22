//using System;
//using UnityEngine;
//using Random = System.Random;

//[Serializable]
//public class BellShapedNoise : INoise {
//    public int influence;

//    public float influenceDecay, verticalShrink;

//    private readonly float2[] points;

//    public BellShapedNoise(int nb)
//    {
//        var rnd = new Random(seed);

//        points = new float2[nb];
//        for (var i = 0; i < nb; i++)
//        {
//            points[i] = new float2((float)rnd.NextDouble(), (float)rnd.NextDouble());
//        }
//    }

//    public void GenerateNoise() {
//        for (var k = 0; k < points.Length; k++) {
//            var g = points[k];
//            var z = 1 + influenceDecay * k;
//            var f1 = influence * z;
//            var f2 = verticalShrink * z;

//            var x = -g.x;
//            for (var i = 0; i < w; i++, x += 1f/w)
//            {
//                var x2 = x * x;
//                var y = -g.y;
//                for (var j = 0; j < h; j++, y += 1f / h)
//                {
//                    actualNoise[i, j] += math.exp(-f1 * (x2 + y * y)) / f2;
//                }
//            }
//        }
//    }

//    public override float Get(float x, float y, float frequency)
//    {
//        return actualNoise[Mathf.RoundToInt(x), Mathf.RoundToInt(y)];
//    }
//}
