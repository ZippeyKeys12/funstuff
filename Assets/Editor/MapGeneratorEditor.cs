using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

using Map.Generation;
using NoiseType = Map.Generation.MapGenerator.NoiseType;
using FilterType = Map.Generation.MapGenerator.FilterType;
using FractalType = Map.Generation.MapGenerator.FractalType;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var mapGen = (MapGenerator)target;

        EditorGUI.BeginChangeCheck();

        #region Drawing
        #region Generator Settings
        EditorGUILayout.LabelField("Generator Settings", EditorStyles.boldLabel);

        mapGen.autoUpdate = EditorGUILayout.Toggle("Auto Update", mapGen.autoUpdate);
        #endregion


        #region Terrain Settings
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Terrain Settings", EditorStyles.boldLabel);

        mapGen.mapSize = EditorGUILayout.DelayedIntField("Map Size", mapGen.mapSize);

        mapGen.resPower = EditorGUILayout.IntSlider("Resolution Power", mapGen.resPower, 5, 8);

        mapGen.terrainHeight = EditorGUILayout.Slider("Terrain Height", mapGen.terrainHeight, 0, 500);

        mapGen.seaLevel = EditorGUILayout.Slider("Sea Level", mapGen.seaLevel, 0, 1);
        #endregion


        #region Noise Settings
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Noise Settings", EditorStyles.boldLabel);

        mapGen.noiseType = (NoiseType)EditorGUILayout.EnumPopup("Type", mapGen.noiseType);

        mapGen.seed = EditorGUILayout.IntSlider("Seed", mapGen.seed, 0, 10000);

        mapGen.offsetX = EditorGUILayout.Slider("Offset X", mapGen.offsetX, 0, 2500);
        mapGen.offsetY = EditorGUILayout.Slider("Offset Y", mapGen.offsetY, 0, 2500);

        mapGen.frequency = EditorGUILayout.Slider("Frequency", mapGen.frequency, .01f, 1);
        #endregion


        #region Filter Settings
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Filter Settings", EditorStyles.boldLabel);

        mapGen.filterType = (FilterType)EditorGUILayout.EnumPopup("Type", mapGen.filterType);

        if (mapGen.filterType == FilterType.Interpolate)
        {
            var resolution = math.pow(2, mapGen.resPower);
            mapGen.interpolationPower = EditorGUILayout.Slider("Unit", mapGen.interpolationPower, 1 / resolution, resolution - 1);
        }
        #endregion


        #region Fractal Settings
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Fractal Settings", EditorStyles.boldLabel);

        mapGen.fractalType = (FractalType)EditorGUILayout.EnumPopup("Type", mapGen.fractalType);

        if (mapGen.fractalType == FractalType.FBM || mapGen.fractalType == FractalType.Multifractal)
        {
            mapGen.octaves = EditorGUILayout.IntSlider("Octaves", mapGen.octaves, 1, 10);

            mapGen.persistance = EditorGUILayout.Slider("Persistance", mapGen.persistance, .0001f, 1f);

            mapGen.lacunarity = EditorGUILayout.Slider("Lacunarity", mapGen.lacunarity, .0001f, 5f);
        }
        #endregion

        if (!mapGen.autoUpdate)
        {
            EditorGUILayout.Separator();
            if (GUILayout.Button("Generate Map"))
            {
                mapGen.GenerateMap();
            }
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