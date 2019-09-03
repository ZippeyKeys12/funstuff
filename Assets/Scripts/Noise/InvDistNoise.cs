using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InvDistNoise : INoise {
    public int nb;

    private Vector2[] points;

    public override void GenerateNoise() {
        var rnd = new System.Random(seed);

        points = new Vector2[nb];
        for (var i = 0; i < nb; i++) {
            points[i] = new Vector2((float)rnd.NextDouble() * w, (float)rnd.NextDouble() * h);
        }

        base.GenerateNoise();
    }

    protected override float GeneratePoint(float x, float y) {
        float sum_weights = 1,
              sum_heights = 0;

        for (var k = 0; k < points.Length; k++) {
            var sq_dist = Vector2.SqrMagnitude(new Vector2(x, y) - points[k]);
            sum_weights += points.Length / sq_dist;
            sum_heights += k / sq_dist;
        }

        return sum_heights / sum_weights;
    }
}
