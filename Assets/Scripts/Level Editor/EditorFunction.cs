using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditorFunction : MonoBehaviour {
    public GridGenerator gridGeneratorPrefab;

    public GridGenerator genUse;

    public TMP_Dropdown terrainSelect;

    public EditorCursor editorCursor;

    public TMP_InputField nameInput;

    public List<TileTerrain> terrains;

    private void Start() {
        terrains = gridGeneratorPrefab.terrainTypes;
        List<string> terrainNames = new List<string>();
        foreach (var terrain in terrains) {
            terrainNames.Add(terrain.name);
        }
        terrainSelect.AddOptions(terrainNames);
        editorCursor.terrainBrush = terrains[0];
        //gridParent = FindAnyObjectByType<GridParent>();
    }

    public void DropdownValueChanged(int index) {
        editorCursor.terrainBrush = terrains[index];
    }

    public string SaveString() {
        Tile[,] tileData = GameManager.Instance.tiles;
        string saveString = string.Empty;
        saveString += $"{genUse.scale} {genUse.gapScale} {genUse.width} {genUse.height}\n";
        for (int x = 0; x < tileData.GetLength(0); x++) {
            for (int z = 0; z < tileData.GetLength(1) - 1; z++) {
                saveString += tileData[x, z].terrainType.editorIndex + " ";
            }
            saveString += tileData[x, tileData.GetLength(1) - 1].terrainType.editorIndex + "\n";
        }
        return saveString;
    }
}
