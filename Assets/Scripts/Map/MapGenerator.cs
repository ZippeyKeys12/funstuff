using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public MapDrawMode drawMode;

    public NoiseContainer noiseType;

    [Range(1f, 100f)]
    public float scale = 1;

    public bool autoUpdate;

    public void GenerateMap() {
        if (noiseType == null) {
            noiseType = new NoiseContainer();
        }

        float[,] map = { { 0f, 1f }, { .2f, .4f } };

        noiseType.UpdateInst();
        noiseType.GenerateNoise();
        map = noiseType.GetNoise(scale);

        var renderer = FindObjectOfType<MapRenderer>();
        switch (drawMode) {
            case MapDrawMode.Texture:
                renderer.DrawTexture(map);
                break;
            case MapDrawMode.Mesh:
                renderer.DrawTexture(map);
                break;
        }
    }
}

public enum MapDrawMode {
    Texture,
    Mesh
}

public static class NoiseTypeImpl {
    public static INoise ToGen(this GenerativeNoiseType noiseGen, int nb, int w, int h, int seed) {
        INoise gen = null;

        switch (noiseGen) {
            case GenerativeNoiseType.Random:
                gen = new RandomNoise();
                break;
            case GenerativeNoiseType.Value:
                gen = new ValueNoise();
                break;
            case GenerativeNoiseType.InverseDistance:
                gen = new InvDistNoise();
                break;
            case GenerativeNoiseType.Perlin:
                gen = new PerlinNoise();
                break;
        }
        return gen;
    }
}