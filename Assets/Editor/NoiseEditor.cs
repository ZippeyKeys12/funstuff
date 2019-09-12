//using UnityEditor;
//using UnityEngine;

//[CustomPropertyDrawer(typeof(NoiseContainer))]
//public class NoiseEditor : PropertyDrawer {
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
//        EditorGUI.BeginProperty(position, label, property);
//        bool isChild = property.FindPropertyRelative("isChild").boolValue;

//        #region Drawing
//        EditorGUI.PropertyField(position, property.FindPropertyRelative("seed"));
//        EditorGUILayout.PropertyField(property.FindPropertyRelative("type"), new GUIContent("Operator Type"));

//        switch ((NoiseOperatorType)property.FindPropertyRelative("type").enumValueIndex) {
//            case NoiseOperatorType.Generative:
//                EditorGUILayout.PropertyField(property.FindPropertyRelative("genType"));
//                var genType = (GenerativeNoiseType)property.FindPropertyRelative("genType").enumValueIndex;

//                //if (genType == GenerativeNoiseType.InverseDistance || genType == GenerativeNoiseType.BellShaped) {
//                //    EditorGUILayout.PropertyField(property.FindPropertyRelative("nb"), new GUIContent("# of Points"));
//                //}

//                //if (genType == GenerativeNoiseType.BellShaped)
//                //{
//                //    EditorGUILayout.PropertyField(property.FindPropertyRelative("influence"));
//                //    EditorGUILayout.PropertyField(property.FindPropertyRelative("influenceDecay"));
//                //    EditorGUILayout.PropertyField(property.FindPropertyRelative("verticalShrink"));
//                //}

//                //if (genType == GenerativeNoiseType.DiamondSquare)
//                //{
//                //    EditorGUILayout.PropertyField(property.FindPropertyRelative("steps"));
//                //    EditorGUILayout.PropertyField(property.FindPropertyRelative("multiplier"));
//                //    EditorGUILayout.PropertyField(property.FindPropertyRelative("exponent"));
//                //}

//                break;
//            case NoiseOperatorType.Poly:
//                EditorGUILayout.PropertyField(property.FindPropertyRelative("polyType"), new GUIContent("Noise Type"));
//                var polyType = (PolyNoiseType)property.FindPropertyRelative("polyType").enumValueIndex;

//                if (polyType == PolyNoiseType.Octave) {
//                    EditorGUILayout.PropertyField(property.FindPropertyRelative("persistance"));
//                    EditorGUILayout.PropertyField(property.FindPropertyRelative("lacunarity"));
//                }

//                EditorGUILayout.PropertyField(property.FindPropertyRelative("polyOperands"), new GUIContent("Operands"), true);

//                break;
//        }
//        #endregion

//        EditorGUI.EndProperty();
//    }
//}