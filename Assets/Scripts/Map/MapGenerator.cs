using System;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Noise;
using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    [Range(0, 10000)]
    public int seed;

    public int height = 100;
    public int width = 100;

    public MapDrawMode drawMode;

    public NoiseType noiseType;

    [Range(.01f, 1f)]
    public float frequency = .5f;

    public float offsetX, offsetY;

    [Range(1, 10)]
    public int octaves = 1;

    [Range(.0001f, 1f)]
    public float persistance = .5f;

    [Range(.0001f, 5)]
    public float lacunarity = 2;

    [HideInInspector]
    public float heightMult;

    public bool autoUpdate;

    public void GenerateMap()
    {
        Func<int, int> getSeed = x => (int)(seed + new Random(seed * x).NextDouble());

        Generator gen = null;
        switch (noiseType)
        {
            case NoiseType.Value:
                gen = new FBM(lacunarity, persistance, Enumerable.Range(0, octaves).Select(x => new ValueNoise(getSeed(x))).ToArray());
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

        var mapDim = 129;
        var map = new Sample<float2>[mapDim, mapDim];
        var elevation = new float[mapDim, mapDim];
        for (var x = 0; x < mapDim; x++)
        {
            for (var y = 0; y < mapDim; y++)
            {
                map[x, y] = gen.Get(x + offsetX, y + offsetY, frequency);
                elevation[x, y] = map[x, y].Value / 2 + .5f;
            }
        }

        var mapRenderer = FindObjectOfType<MapRenderer>();
        switch (drawMode)
        {
            case MapDrawMode.Mesh:
                mapRenderer.DrawMesh(elevation, heightMult);
                break;
            case MapDrawMode.Terrain:
                mapRenderer.DrawTerrain(elevation, heightMult, mapDim, height, width);
                break;
        }
    }
}

public enum MapDrawMode
{
    Mesh,
    Terrain
}

public enum NoiseType
{
    Value,
    Perlin,
    Billow,
    Ridged,
}