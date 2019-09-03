using UnityEngine;

[System.Serializable]
public class RandomNoise : INoise {
    private System.Random rnd;

    public override void GenerateNoise() {
        rnd = new System.Random(seed);
        base.GenerateNoise();
    }

    protected override float GeneratePoint(float x, float y) {
        return (float)rnd.NextDouble();
    }

    public override float AtXY(float x, float y) {
        int lowX = Mathf.Clamp(Mathf.FloorToInt(x), 0, w - 1),
            highX = Mathf.Clamp(Mathf.CeilToInt(x), 0, w - 1),
            lowY = Mathf.Clamp(Mathf.FloorToInt(y), 0, h - 1),
            highY = Mathf.Clamp(Mathf.CeilToInt(y), 0, h - 1);

        return Mathf.Lerp(
            Mathf.Lerp(actualNoise[lowX, lowY], actualNoise[highX, lowY], x % 1),
            Mathf.Lerp(actualNoise[lowX, highY], actualNoise[highX, highY], x % 1),
            y % 1
        );
    }
}
