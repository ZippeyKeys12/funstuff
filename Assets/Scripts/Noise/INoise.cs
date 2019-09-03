using System;
using UnityEngine;

[Serializable]
public abstract class INoise {
    [Range(1, 1000)]
    public int w = 100, h = 100;

    public int seed;

    [SerializeField]
    protected float[,] actualNoise;

    public virtual void GenerateNoise() {
        actualNoise = new float[w, h];

        for (var x = 0; x < w; x++) {
            for (var y = 0; y < h; y++) {
                actualNoise[x, y] = GeneratePoint(x, y);
            }
        }
    }

    protected abstract float GeneratePoint(float x, float y);

    public virtual float[,] GetNoise(float scale) {
        var noise = new float[w, h];

        for (var x = 0; x < w; x++) {
            for (var y = 0; y < h; y++) {
                noise[x, y] = AtXY(x / scale, y / scale);
            }
        }

        return noise;
    }

    public virtual float AtXY(float x, float y) {
        return GeneratePoint(x, y);
    }
}
