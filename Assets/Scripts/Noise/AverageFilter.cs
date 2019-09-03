using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AverageFilter : INoise {
    public float scale;
    public NoiseContainer[] operands;

    protected override float GeneratePoint(float x, float y) {
        foreach (var op in operands) {
            op.GenerateNoise();
        }

        var sum = 0f;

        for (var i = 0; i < operands.Length; i++) {
            sum += operands[i].AtXY(x, y);
        }

        return sum / operands.Length;
    }
}
