using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        var mapGen = (MapGenerator)target;

        EditorGUI.BeginChangeCheck();

        #region Drawing
        DrawDefaultInspector();

        if (mapGen.drawMode == MapDrawMode.Mesh) {
            mapGen.meshHeightMult = EditorGUILayout.Slider("Mesh Height", mapGen.meshHeightMult, 1, 200);
        }

        if (GUILayout.Button("Generate Map")) {
            mapGen.GenerateMap();
        }
        #endregion

        if (EditorGUI.EndChangeCheck()) {
            if (mapGen.autoUpdate) {
                mapGen.GenerateMap();
            }
        }
    }
}