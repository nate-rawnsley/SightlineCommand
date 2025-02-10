using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EditorFunction))]
public class LevelSaveEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        EditorFunction script = (EditorFunction)target;

        if (GUILayout.Button("Save Level", GUILayout.Height(40))) {
            AssetDatabase.Refresh();
            TextAsset levelSave = new TextAsset(script.SaveString());
            AssetDatabase.CreateAsset(levelSave, $"Assets/Level Saves/{script.nameInput.text}.asset");
        }
    }
}
