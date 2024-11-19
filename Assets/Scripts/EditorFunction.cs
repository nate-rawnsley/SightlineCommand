using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditorFunction : MonoBehaviour {
    [SerializeField]
    private GridGenerator gridGeneratorPrefab;

    [SerializeField]
    private TMP_Dropdown terrainSelect;

    [SerializeField]
    private EditorCursor editorCursor;

    private List<TileTerrain> terrains;

    private void Awake() {
        terrains = gridGeneratorPrefab.terrainTypes;
        List<string> terrainNames = new List<string>();
        foreach (var terrain in terrains) {
            terrainNames.Add(terrain.name);
        }
        terrainSelect.AddOptions(terrainNames);
        //seditorCursor.terrainType = terrains.First();
    }
}
