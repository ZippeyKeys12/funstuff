using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Noise;

public class MapRenderer : MonoBehaviour
{
    protected GameObject[,] terrains;

    public Texture2D GenerateTexture(float[,] map)
    {
        int w = map.GetLength(0),
            h = map.GetLength(1);

        var texture = new Texture2D(w, h);

        var colorMap = new Color[w * h];
        for (var x = 0; x < w; x++)
        {
            for (var y = 0; y < h; y++)
            {
                colorMap[y * w + x] = Color.Lerp(Color.black, Color.white, math.unlerp(-1, 1, map[x, y]));
            }
        }

        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }

    public void DrawTerrain(float2 pos, float2 offset, Generator gen, float terrainHeight, int resPower, int size, int chunkDim, float frequency)
    {
        if (terrains != null)
        {
            foreach (var terrain in terrains)
            {
                GameObject.DestroyImmediate(terrain);
            }
        }
        terrains = new GameObject[2 * chunkDim + 1, 2 * chunkDim + 1];

        for (var x = 0; x <= 2 * chunkDim; x++)
        {
            for (var y = 0; y <= 2 * chunkDim; y++)
            {
                var centered = new float2(x, y) - chunkDim;
                var manhattan = math.abs(centered);

                DrawSubTerrain(new int2(x, y), (new float2(x, y) - chunkDim) * size, new float2(y, x) * size + offset, gen, terrainHeight,
                    (int)math.pow(2, resPower - math.max(manhattan.x, manhattan.y)) + 1, size, frequency);
            }
        }

        for (var x = 0; x <= 2 * chunkDim; x++)
        {
            for (var y = 0; y <= 2 * chunkDim; y++)
            {
                var terrain = terrains[x, y].GetComponent<Terrain>();
                terrain.SetNeighbors(
                    (x > 1) ? terrains[x - 1, y].GetComponent<Terrain>() : null,
                    (y < 2 * chunkDim - 1) ? terrains[x, y + 1].GetComponent<Terrain>() : null,
                    (x < 2 * chunkDim - 1) ? terrains[x + 1, y].GetComponent<Terrain>() : null,
                    (y > 1) ? terrains[x, y - 1].GetComponent<Terrain>() : null
                );
            }
        }
    }

    protected float[,] GenerateHeightMap(int resolution, int size, Generator gen, float2 offset, float frequency)
    {
        // var map = new Sample<float2>[resolution, resolution];
        var elevation = new float[resolution, resolution];
        var xi = 0;
        // TODO: Determine necessity of double over float
        for (var x = 0d; xi < resolution; x += size / (double)resolution)
        {
            var yi = 0;
            for (var y = 0d; yi < resolution; y += size / (double)resolution)
            {
                // map[x, y] = gen.Get(x + offset.x, y + offset.y, frequency);
                // Debug.Log($"Size: {size}, {size}");
                // Debug.Log($"Resolution: {resolution}, {resolution}");
                // Debug.Log($"Pos: {x}, {y}");
                // Debug.Log($"Pos: {xi}, {yi}");
                elevation[xi, yi] = gen.Get((float)x + offset.x, (float)y + offset.y, frequency).Value / 2 + .5f;
                yi++;
            }
            xi++;
        }

        // elevation[0, 0] = gen.Get(offset.x, offset.y, frequency).Value / 2 + .5f;
        elevation[resolution - 1, resolution - 1] = gen.Get(size + offset.x, size + offset.y, frequency).Value / 2 + .5f;

        return elevation;
    }

    protected void DrawSubTerrain(int2 index, float2 pos, float2 offset, Generator gen, float terrainHeight, int resolution, int size, float frequency)
    {
        var map = GenerateHeightMap(resolution, size, gen, offset, frequency);
        var terrainData = GenerateTerrain(map, terrainHeight, resolution, size);

        var terrainObject = new GameObject("Terrain");
        terrainObject.transform.parent = this.transform;
        terrainObject.transform.position = new Vector3(pos.x, 0, pos.y);

        var terrain = terrainObject.AddComponent<Terrain>();
        terrain.terrainData = terrainData;
        terrain.basemapDistance = 10000;
        terrain.heightmapPixelError = 5;

        terrain.materialTemplate = new Material(Shader.Find("Standard"));
        terrain.materialTemplate.mainTexture = GenerateTexture(map);
        terrain.materialTemplate.enableInstancing = true;
        // terrain.materialTemplate = GenerateTerrainMaterial(map, terrainHeight, resolution);

        var terrainCollider = terrainObject.AddComponent<TerrainCollider>();
        terrainCollider.terrainData = terrainData;

        terrains[index.x, index.y] = terrainObject;
    }

    public TerrainData GenerateTerrain(float[,] map, float terrainHeight, int resolution, int size)
    {
        var terrainData = new TerrainData
        {
            heightmapResolution = resolution,
            size = new float3(size, terrainHeight, size)
        };
        terrainData.SetDetailResolution(resolution, 16);
        terrainData.alphamapResolution = resolution;
        terrainData.baseMapResolution = resolution;

        terrainData.SetHeights(0, 0, map);

        return terrainData;
    }

    public Material GenerateTerrainMaterial(float[,] map, float terrainHeight, int resolution)
    {
        return null;
    }
}
