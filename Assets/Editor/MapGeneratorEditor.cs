using UnityEditor;
using UnityEngine;
using Map.Generation;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var mapGen = (MapGenerator)target;

        EditorGUI.BeginChangeCheck();

        #region Drawing
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Map"))
        {
            mapGen.GenerateMap();
        }
        #endregion

        else if (EditorGUI.EndChangeCheck())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }
    }
}