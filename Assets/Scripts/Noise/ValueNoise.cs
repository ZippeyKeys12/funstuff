using System.Linq;
using UnityEngine;

[System.Serializable]
public class ValueNoise : INoise {
    private const int hashMask = 255;

    private int[] hash;

    public override void GenerateNoise() {
        var rnd = new System.Random(seed);
        hash = Enumerable.Range(0, hashMask + 1).OrderBy(x => rnd.Next()).ToArray();

        base.GenerateNoise();
    }

    protected override float GeneratePoint(float x, float y) {
        var ix = Mathf.FloorToInt(x) & hashMask;
        var iy = Mathf.FloorToInt(y) & hashMask;
        return hash[(hash[ix] + iy) & hashMask] * (1f / hashMask);
    }
}
