using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameStatsEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        GameManager script = (GameManager)target;
        if (GUILayout.Button("Restart Game")) {
            script.RestartGame();
        }
    }
}
