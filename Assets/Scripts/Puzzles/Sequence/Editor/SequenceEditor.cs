using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom editor to simplify creating a variety of sequence puzzles
/// </summary>
[CustomEditor(typeof(SequenceConfig))]
public class SequenceEditor : Editor
{
    SerializedProperty floorMask;
    SerializedProperty horizontal;
    SerializedProperty vertical;
    SerializedProperty startPosition;

    private void OnEnable()
    {
        floorMask = serializedObject.FindProperty("floor");
        horizontal = serializedObject.FindProperty("horizontalSize");
        vertical = serializedObject.FindProperty("verticalSize");
        startPosition = serializedObject.FindProperty("startPosition");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(startPosition);

        EditorGUILayout.PropertyField(horizontal);
        EditorGUILayout.PropertyField(vertical);

        if (floorMask.arraySize != horizontal.intValue * vertical.intValue)
        {
            floorMask.arraySize = horizontal.intValue * vertical.intValue;
        }
        
        /// ---FLOOR
        for (int v = 0; v < vertical.intValue; v++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int h = 0; h < horizontal.intValue; h++)
            {
                int pos = (v * horizontal.intValue) + h;
                floorMask.GetArrayElementAtIndex(pos).boolValue =
                    EditorGUILayout.Toggle(floorMask.GetArrayElementAtIndex(pos).boolValue);
            }
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
