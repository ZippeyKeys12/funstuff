using UnityEngine;

public class PerlinNoise : INoise {
    protected override float GeneratePoint(float x, float y) {
        return Mathf.PerlinNoise(seed + x, seed + y);
    }
}
