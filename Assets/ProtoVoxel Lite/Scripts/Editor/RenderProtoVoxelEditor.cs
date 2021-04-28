using UnityEditor;
using UnityEngine;

namespace ProtoVoxelLite
{
    [CustomEditor(typeof(RenderProtoVoxel))]
    [CanEditMultipleObjects]
    public class RenderProtoVoxelEditor:Editor
    {
        SerializedProperty data;
        SerializedProperty color;
        SerializedProperty renderOnChange;
        SerializedProperty modifiers;
        SerializedProperty seed;

        SerializedProperty modScale;
        SerializedProperty modOffsetX;
        SerializedProperty modOffsetY;
        SerializedProperty modBendX;
        SerializedProperty modBendY;
        SerializedProperty modTwist;

        SerializedProperty modScaleMult;
        SerializedProperty modOffsetXMult;
        SerializedProperty modOffsetYMult;
        SerializedProperty modBendXMult;
        SerializedProperty modBendYMult;
        SerializedProperty modTwistMult;

        void OnEnable()
        {
            data = serializedObject.FindProperty("data");
            color = serializedObject.FindProperty("color");
            seed = serializedObject.FindProperty("randomSeed");
            renderOnChange = serializedObject.FindProperty("renderOnChange");
            modifiers = serializedObject.FindProperty("modifiers");

            modScale = serializedObject.FindProperty("modScale");
            modOffsetX = serializedObject.FindProperty("modOffsetX");
            modOffsetY = serializedObject.FindProperty("modOffsetY");
            modBendX = serializedObject.FindProperty("modBendX");
            modBendY = serializedObject.FindProperty("modBendY");
            modTwist = serializedObject.FindProperty("modTwist");

            modScaleMult = serializedObject.FindProperty("modScaleMult");
            modOffsetXMult = serializedObject.FindProperty("modOffsetXMult");
            modOffsetYMult = serializedObject.FindProperty("modOffsetYMult");
            modBendXMult = serializedObject.FindProperty("modBendXMult");
            modBendYMult = serializedObject.FindProperty("modBendYMult");
            modTwistMult = serializedObject.FindProperty("modTwistMult");
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(data, new GUIContent("Data", "Data to be used to create the model;"));
            EditorGUILayout.PropertyField(color, new GUIContent("Color", "A color to multiply the original VoxelData color."));
            EditorGUILayout.PropertyField(renderOnChange, new GUIContent("Render On Change", "Render again the model on each change of this script."));

            if (!renderOnChange.boolValue)
            {
                if(GUILayout.Button("Render Object"))
                {
                    foreach(Object o in serializedObject.targetObjects)
                    {
                        (o as RenderProtoVoxel).RenderVoxel();
                    }
                }
            }

            if (seed.intValue != -1)
            {
                EditorGUILayout.PropertyField(seed, new GUIContent("ProcGen Seed", "Seed used to generate this object."));
                if (GUILayout.Button("Random Seed"))
                {
                    foreach (Object o in serializedObject.targetObjects)
                    {
                        (o as RenderProtoVoxel).NewSeed();
                        //if((o as RenderProtoVoxel).renderOnChange)
                        //    (o as RenderProtoVoxel).RenderVoxel();
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (Object o in serializedObject.targetObjects)
                {
                    if ((o as RenderProtoVoxel).renderOnChange)
                        (o as RenderProtoVoxel).RenderVoxel();
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(modifiers, new GUIContent("Use modifiers", "Calculate the modifiers for this object."));
            
            if (modifiers.boolValue)
            {
                EditorGUI.indentLevel++;


                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Scale", "Scale the model"));
                EditorGUILayout.PropertyField(modScaleMult, new GUIContent(""));
                EditorGUILayout.PropertyField(modScale, new GUIContent(""));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Offset X", "Offset the model in the X axis."));
                EditorGUILayout.PropertyField(modOffsetXMult, new GUIContent(""));
                EditorGUILayout.PropertyField(modOffsetX, new GUIContent("", ""));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Offset Y", "Offset the model in the Y axis."));
                EditorGUILayout.PropertyField(modOffsetYMult, new GUIContent(""));
                EditorGUILayout.PropertyField(modOffsetY, new GUIContent("", ""));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Bend X", "Bend the model in the X axis."));
                EditorGUILayout.PropertyField(modBendXMult, new GUIContent(""));
                EditorGUILayout.PropertyField(modBendX, new GUIContent("", ""));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Bend Y", "Bend the model in the Y axis."));
                EditorGUILayout.PropertyField(modBendYMult, new GUIContent(""));
                EditorGUILayout.PropertyField(modBendY, new GUIContent("", ""));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Twist", "Twist the model by its center on the Y axis."));
                EditorGUILayout.PropertyField(modTwistMult, new GUIContent(""));
                EditorGUILayout.PropertyField(modTwist, new GUIContent("", ""));
                EditorGUILayout.EndHorizontal();


                EditorGUI.indentLevel--;
            }

            

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (Object o in serializedObject.targetObjects)
                {
                    if((o as RenderProtoVoxel).renderOnChange && (o as RenderProtoVoxel).modifiers)
                        (o as RenderProtoVoxel).ApplyMods();
                }
            }

            
        }
    }
}
