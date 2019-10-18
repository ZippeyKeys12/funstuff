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

    public int mapSize = 100;

    [Range(1, 4)]
    public int mapChunks = 1;

    public float offsetX, offsetY;

    public NoiseType noiseType;

    [Range(.01f, 1f)]
    public float frequency = .5f;

    [Range(1, 10)]
    public int octaves = 1;

    [Range(.0001f, 1f)]
    public float persistance = .5f;

    [Range(.0001f, 5)]
    public float lacunarity = 2;

    [Range(1, 200)]
    public float terrainHeight = 1;

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

        var mapDim = 5 + mapChunks;
        FindObjectOfType<MapRenderer>().DrawTerrain(new float2(transform.position.x, transform.position.z), new float2(offsetX, offsetY), gen, terrainHeight, mapDim, mapSize, mapChunks, frequency);
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