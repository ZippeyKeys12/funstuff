//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[System.Serializable]
//public class InvDistNoise : INoise {
//    private Vector2[] points;

//    public InvDistNoise(float w, float h, int nb)
//    {
//        var rnd = new System.Random(seed);

//        points = new Vector2[nb];
//        for (var i = 0; i < nb; i++)
//        {
//            points[i] = new Vector2((float)rnd.NextDouble() * w, (float)rnd.NextDouble() * h);
//        }
//    }

//    public override float Get(float x, float y, float frequency) {
//        float sum_weights = 1,
//              sum_heights = 0;

//        for (var k = 0; k < points.Length; k++) {
//            var sq_dist = Vector2.SqrMagnitude(new Vector2(x, y) - points[k]);
//            sum_weights += points.Length / sq_dist;
//            sum_heights += k / sq_dist;
//        }

//        return sum_heights / sum_weights;
//    }
//}
