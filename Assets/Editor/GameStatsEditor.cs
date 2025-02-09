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
        if (GUILayout.Button("Hurt Buildings")) {
            foreach (var building in script.players[PlayerTeam.HUMAN].buildings) {
                Debug.Log(building.name);
                building.TakeDamage(1);
            }
        }
    }
}
