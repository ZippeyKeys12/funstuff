using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NoiseContainer))]
public class NoiseEditor : PropertyDrawer {
    public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label) {
        EditorGUI.BeginProperty(rect, label, prop);

        #region Drawing
        EditorGUI.PropertyField(rect, prop.FindPropertyRelative("mapWidth"));
        EditorGUILayout.PropertyField(prop.FindPropertyRelative("mapHeight"));
        EditorGUILayout.PropertyField(prop.FindPropertyRelative("seed"));

        EditorGUILayout.PropertyField(prop.FindPropertyRelative("type"), new GUIContent("Operator Type"));
        switch ((NoiseOperatorType)prop.FindPropertyRelative("type").enumValueIndex) {
            case NoiseOperatorType.Generative:
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("genType"));
                var genType = (GenerativeNoiseType)prop.FindPropertyRelative("genType").enumValueIndex;

                if (genType == GenerativeNoiseType.InverseDistance || genType == GenerativeNoiseType.BellShaped) {
                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("nb"), new GUIContent("# of Points"));
                }

                break;
            case NoiseOperatorType.Poly:
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("polyType"), new GUIContent("Noise Type"));
                var polyType = (PolyNoiseType)prop.FindPropertyRelative("polyType").enumValueIndex;

                if (polyType == PolyNoiseType.Octave) {
                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("persistance"));
                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("lacunarity"));
                }

                EditorGUILayout.PropertyField(prop.FindPropertyRelative("polyOperands"), true);

                break;
        }
        #endregion

        EditorGUI.EndProperty();
    }
}