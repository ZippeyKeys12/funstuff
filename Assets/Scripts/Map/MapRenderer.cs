using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class MapRenderer : MonoBehaviour
{
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

    public void DrawMesh(float[,] map, float meshHeight)
    {
        GetComponent<MeshFilter>().sharedMesh = GenerateMesh(map, meshHeight);
        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GenerateTexture(map);
    }

    public Mesh GenerateMesh(float[,] map, float meshHeight)
    {
        int w = map.GetLength(0),
            h = map.GetLength(1);
        var topLeftX = (w - 1) / -2f;
        var topLeftZ = (h - 1) / 2f;

        var vertices = new float3[w * h];
        var triangles = new int[(w - 1) * (h - 1) * 6];
        var uvs = new float2[w * h];


        var vertexIndex = 0;
        var triangleIndex = 0;

        for (var y = 0; y < h; y++)
        {
            for (var x = 0; x < w; x++)
            {
                vertices[vertexIndex] = new float3(topLeftX + x, map[x, y] * meshHeight, topLeftZ - y);
                uvs[vertexIndex] = new float2(x / (float)w, y / (float)h);

                if (x < w - 1 && y < h - 1)
                {
                    triangles[triangleIndex] = vertexIndex;
                    triangles[triangleIndex + 1] = vertexIndex + w + 1;
                    triangles[triangleIndex + 2] = vertexIndex + w;

                    triangleIndex += 3;

                    triangles[triangleIndex] = vertexIndex + w + 1;
                    triangles[triangleIndex + 1] = vertexIndex;
                    triangles[triangleIndex + 2] = vertexIndex + 1;

                    triangleIndex += 3;
                }

                vertexIndex++;
            }
        }

        var mesh = new Mesh()
        {
            vertices = vertices.Select(a => new Vector3(a.x, a.y, a.z)).ToArray(),
            triangles = triangles,
            uv = uvs.Select(a => new Vector2(a.x, a.y)).ToArray()
        };
        mesh.RecalculateNormals();
        return mesh;
    }

    public void DrawTerrain(float[,] map, float terrainHeight, int resolution)
    {
        var terrainData = GenerateTerrain(map, terrainHeight, resolution);

        var terrain = GetComponent<Terrain>();
        terrain.terrainData = terrainData;
        //terrain.materialTemplate = GenerateTerrainMaterial();

        GetComponent<TerrainCollider>().terrainData = terrainData;
    }

    public TerrainData GenerateTerrain(float[,] map, float terrainHeight, int resolution)
    {
        int w = map.GetLength(0),
            h = map.GetLength(1);

        var terrainData = new TerrainData
        {
            size = new float3(h, terrainHeight, w),
            heightmapResolution = resolution
        };

        terrainData.SetHeights(0, 0, map);

        return terrainData;
    }

    public Material GenerateTerrainMaterial(float[,] map, float terrainHeight, int resolution)
    {
        return null;
    }
}
