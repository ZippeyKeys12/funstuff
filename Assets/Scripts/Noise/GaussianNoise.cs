using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GaussianNoise : INoise {
    private System.Random rnd;

    public override void GenerateNoise() {
        rnd = new System.Random(seed);
        base.GenerateNoise();
    }

    protected override float GeneratePoint(float x, float y) {
        return (float)rnd.NextDouble();
    }

    public override float AtXY(float x, float y) {
        return actualNoise[Mathf.RoundToInt(x), Mathf.RoundToInt(y)];
    }
}

