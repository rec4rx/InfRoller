using UnityEditor;
using UnityEngine;
using System.Collections;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CanEditMultipleObjects]
[CustomEditor(typeof(Pattern))]

public class PatternEditor : Editor {

    public override void OnInspectorGUI() {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        DrawDefaultInspector ();
        if (GUILayout.Button ("Fill Blocks")) {
            foreach (var myTarget in targets) {
                ((Pattern)myTarget).EditorAutoFillBlocks ();
            }
        }
    }
}