using System;
using System.Linq;
using UnityEngine;
using ZNoise;
using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    [Min(1)]
    public int mapWidth = 100, mapHeight = 100;

    [Range(0, 10000)]
    public int seed;

    public MapDrawMode drawMode;

    public NoiseType noiseType;

    [Range(.01f, 1f)]
    public float frequency = .5f;

    [Range(1, 10)]
    public int octaves = 1;

    [Range(.0001f, 1f)]
    public float persistance = .5f;

    [Range(.0001f, 5)]
    public float lacunarity = 2;

    [HideInInspector]
    public float meshHeightMult;

    public bool autoUpdate;

    public void GenerateMap()
    {
        Func<int, int> getSeed = x => (int)(seed + new Random(seed * x).NextDouble());

        Generator gen = null;
        switch (noiseType)
        {
            case NoiseType.Value:
                gen = new ValueNoise(seed);
                break;
            case NoiseType.Perlin:
                gen = new FBM(lacunarity, persistance, Enumerable.Range(0, octaves).Select(x => new PerlinNoise(getSeed(x))).ToArray());
                break;
            case NoiseType.Billow:
                gen = new FBM(lacunarity, persistance, Enumerable.Range(0, octaves).Select(x => new BillowNoise(getSeed(x))).ToArray());
                break;
            case NoiseType.Ridged:
                gen = new FBM(lacunarity, persistance, Enumerable.Range(0, octaves).Select(x => new RidgedNoise(getSeed(x))).ToArray());
                break;
        }

        var map = new float[mapWidth, mapHeight];
        for (var x = 0; x < mapWidth; x++)
        {
            for (var y = 0; y < mapHeight; y++)
            {
                //map[x, y] = -1;
                map[x, y] = gen.Get(x, y, frequency);
            }
        }

        var mapRenderer = FindObjectOfType<MapRenderer>();
        switch (drawMode)
        {
            case MapDrawMode.Texture:
                mapRenderer.DrawTexture(map);
                break;
            case MapDrawMode.Mesh:
                mapRenderer.DrawMesh(map, meshHeightMult);
                break;
        }
    }
}

public enum MapDrawMode
{
    Texture,
    Mesh
}

public enum NoiseType
{
    Value,
    Perlin,
    Billow,
    Ridged,
}