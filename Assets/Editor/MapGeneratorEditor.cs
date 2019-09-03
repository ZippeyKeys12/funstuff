using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        var mapGen = (MapGenerator)target;

        EditorGUI.BeginChangeCheck();

        #region Drawing
        DrawDefaultInspector();

        //if (mapGen.noiseGenerator == NoiseType.InverseDistance || mapGen.noiseGenerator == NoiseType.BellShaped) {
        //    mapGen.nb = EditorGUILayout.IntSlider("# of Points", mapGen.nb, 0, 200);
        //}

        if (GUILayout.Button("Generate Map")) {
            mapGen.GenerateMap();
        }
        #endregion

        if (EditorGUI.EndChangeCheck()) {
            if (mapGen.autoUpdate) {
                mapGen.GenerateMap();
                mapGen.noiseType.UpdateInst();
            }
        }
    }
}