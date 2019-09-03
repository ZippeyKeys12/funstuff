using UnityEngine;

public class MapRenderer : MonoBehaviour {
    public Renderer textureRenderer;

    public void DrawTexture(float[,] map) {
        int w = map.GetLength(0),
            h = map.GetLength(1);

        var texture = new Texture2D(w, h);

        var colorMap = new Color[w * h];
        for (var x = 0; x < w; x++) {
            for (var y = 0; y < h; y++) {
                colorMap[y * w + x] = Color.Lerp(Color.black, Color.white, map[x, y]);
            }
        }

        texture.SetPixels(colorMap);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(w, 1, h);
    }

    public void DrawMesh(float[,] map) {
        int w = map.GetLength(0),
            h = map.GetLength(1);
        float topLeftX = (w - 1) / -2f,
              topLeftZ = (h - 1) /  2f;

        int vertexIndex = 0;
    }
}
