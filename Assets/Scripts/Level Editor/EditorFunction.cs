using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class EditorFunction : MonoBehaviour {
    [SerializeField]
    private GridGenerator gridGeneratorPrefab;

    [SerializeField]
    private GridGenerator genUse;

    [SerializeField]
    private TMP_Dropdown terrainSelect;

    [SerializeField]
    private EditorCursor editorCursor;

    [SerializeField]
    private TMP_InputField nameInput;

    private List<TileTerrain> terrains;
    private GridParent gridParent;

    private void Start() {
        genUse.GenerateGrid();
        terrains = gridGeneratorPrefab.terrainTypes;
        List<string> terrainNames = new List<string>();
        foreach (var terrain in terrains) {
            terrainNames.Add(terrain.name);
        }
        terrainSelect.AddOptions(terrainNames);
        editorCursor.terrainBrush = terrains[0];
        gridParent = FindAnyObjectByType<GridParent>();
    }

    public void DropdownValueChanged(int index) {
        editorCursor.terrainBrush = terrains[index];
    }

    public void Save() {
        Tile[,] tileData = gridParent.tiles;
        string saveString = string.Empty;
        saveString += $"{genUse.scale} {genUse.gapScale} {genUse.width} {genUse.height}\n";
        for (int x = 0; x < tileData.GetLength(0); x++) {
            for (int z = 0; z < tileData.GetLength(1) - 1; z++) {
                saveString += tileData[x, z].terrainType.editorIndex + " ";
            }
            saveString += tileData[x, tileData.GetLength(1) - 1].terrainType.editorIndex + "\n";
        }
        AssetDatabase.Refresh();
        TextAsset levelSave = new TextAsset(saveString);
        AssetDatabase.CreateAsset(levelSave, $"Assets/Level Saves/{nameInput.text}.asset");
    }
}
