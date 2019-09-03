using System.Linq;
using UnityEngine;

[System.Serializable]
public class ValueNoise : INoise {
    private const int hashMask = 255;

    private int[] hash;

    public override void GenerateNoise() {
        var rnd = new System.Random(seed);
        var temp = Enumerable.Range(0, hashMask + 1).OrderBy(x => rnd.Next());
        hash = temp.Concat(temp).ToArray();

        base.GenerateNoise();
    }

    protected override float GeneratePoint(float x, float y) {
        //var ix = Mathf.FloorToInt(x) & hashMask;
        //var iy = Mathf.FloorToInt(y) & hashMask;
        //return hash[(hash[ix] + iy) & hashMask] * (1f / hashMask);
        var ix0 = Mathf.FloorToInt(x);
        var iy0 = Mathf.FloorToInt(y);
        var tx = x - ix0;
        var ty = y - iy0;
        ix0 &= hashMask;
        iy0 &= hashMask;
        var ix1 = ix0 + 1;
        var iy1 = iy0 + 1;

        var h0 = hash[ix0];
        var h1 = hash[ix1];
        var h00 = hash[(h0 + iy0)];
        var h10 = hash[(h1 + iy0)];
        var h01 = hash[(h0 + iy1)];
        var h11 = hash[(h1 + iy1)];

        //tx = Smooth(tx);
        //ty = Smooth(ty);
        return Mathf.Lerp(
            Mathf.Lerp(h00, h10, tx),
            Mathf.Lerp(h01, h11, tx),
            ty) * (1f / hashMask);
    }
}
