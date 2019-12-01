using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Noise;

namespace Map
{
    public class MapRenderer : MonoBehaviour
    {
        protected GameObject[,] terrains;
        int count;

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
            count = 0;
            foreach (var terrain in GetComponentsInChildren(typeof(Terrain)))
            {
                GameObject.DestroyImmediate(terrain.gameObject);
            }
            terrains = new GameObject[2 * chunkDim + 1, 2 * chunkDim + 1];

            for (var x = -chunkDim; x <= chunkDim; x++)
            {
                for (var y = -chunkDim; y <= chunkDim; y++)
                {
                    var centered = new int2(x, y);
                    var manhattan = math.abs(centered);
                    var resolution = (int)math.pow(2, resPower /* - math.max(manhattan.x, manhattan.y) */) + 1;

                    DrawSubTerrain(centered + chunkDim, centered * size + pos, centered * size + offset, gen, terrainHeight, resolution, size, frequency);
                }
            }

            for (var x = 0; x <= 2 * chunkDim; x++)
            {
                for (var y = 0; y <= 2 * chunkDim; y++)
                {
                    var terrain = terrains[x, y].GetComponent<Terrain>();
                    terrain.SetNeighbors(
                        (x > 0) ? terrains[x - 1, y].GetComponent<Terrain>() : null,
                        (y < 2 * chunkDim) ? terrains[x, y + 1].GetComponent<Terrain>() : null,
                        (x < 2 * chunkDim) ? terrains[x + 1, y].GetComponent<Terrain>() : null,
                        (y > 0) ? terrains[x, y - 1].GetComponent<Terrain>() : null
                    );
                }
            }
        }

        protected float[,] GenerateHeightMap(int resolution, Generator gen, float2 offset, float frequency, int size)
        {
            var map = new float[resolution, resolution];
            for (var x = 0; x < resolution; x++)
            {
                for (var y = 0; y < resolution; y++)
                {
                    map[x, y] = gen.Get(size * math.unlerp(0, resolution - 1, new float2(x, y)) + offset, frequency).Value;
                }
            }
            return map;
        }

        protected void DrawSubTerrain(int2 index, float2 pos, float2 offset, Generator gen, float terrainHeight, int resolution, int size, float frequency)
        {
            var map = GenerateHeightMap(resolution, gen, offset, frequency, size);

            var elevation = new float[resolution, resolution];
            for (var i = 0; i < resolution; i++)
            {
                for (var j = 0; j < resolution; j++)
                {
                    elevation[i, j] = math.unlerp(-1, 1, map[i, j]);
                }
            }

            var terrainData = GenerateTerrain(elevation, terrainHeight, resolution, size);

            var terrainObject = new GameObject($"Terrain{count++}");
            terrainObject.transform.parent = this.transform;
            terrainObject.transform.position = new Vector3(pos.x, 0, pos.y);

            var terrain = terrainObject.AddComponent<Terrain>();
            terrain.terrainData = terrainData;
            terrain.basemapDistance = 10000;
            terrain.heightmapPixelError = 5;

            terrain.materialTemplate = new Material(Shader.Find("Standard"));
            terrain.materialTemplate.mainTexture = GenerateTexture(map);
            // terrain.materialTemplate.enableInstancing = true;
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
}