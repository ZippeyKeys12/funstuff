using UnityEngine;

[System.Serializable]
public class OctaveFilter : INoise {
    public float persistance, lacunarity;

    private const float maxNoiseHeight = float.MinValue,
                        minNoiseHeight = float.MaxValue;

    public NoiseContainer[] operands;

    public override void GenerateNoise() {
        foreach (var op in operands) {
            op.GenerateNoise();
        }

        var maxNoiseHeight = float.MinValue;
        var minNoiseHeight = float.MaxValue;

        for (var x = 0; x < w; x++) {
            for (var y = 0; y < h; y++) {
                var ampl = 0f;
                var freq = 1f;
                var noiseHeight = 0f;

                for (var i = 0; i < operands.Length; i++) {
                    noiseHeight += operands[i].AtXY(x * freq, y * freq) * ampl;

                    ampl *= persistance;
                    freq *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }

                actualNoise[x, y] = noiseHeight;
            }
        }

        for (var x = 0; x < w; x++) {
            for (var y = 0; y < h; y++) {
                actualNoise[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, actualNoise[x, y]);
            }
        }
    }

    protected override float GeneratePoint(float x, float y) {
        throw new System.NotImplementedException();
    }
}
