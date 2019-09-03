using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AverageFilter : INoise {
    public float scale;
    public NoiseContainer[] operands;


    public override void GenerateNoise() {
        foreach (var op in operands) {
            op.GenerateNoise();
        }

        base.GenerateNoise();
    }

    protected override float GeneratePoint(float x, float y) {
        var sum = 0f;

        for (var i = 0; i < operands.Length; i++) {
            sum += operands[i].AtXY(x, y);
        }

        return sum / operands.Length;
    }
}
