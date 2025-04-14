using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileEditorPanel : MonoBehaviour {
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject noContent;

    [SerializeField] private TMP_Dropdown terrain;
    [SerializeField] private TMP_Dropdown building;
    [SerializeField] private TMP_Dropdown unit;
    [SerializeField] private TMP_Dropdown decoration;
    [SerializeField] private Slider decoRotation;

    private Tile currentTile;

    private void Start() {
        List<string> terrainNames = new List<string>();
        foreach (var terrainType in EditorFunction.Instance.terrains) {
            terrainNames.Add(terrainType.terrainName);
        }
        terrain.AddOptions(terrainNames);

        List<string> buildingNames = new List<string>();
        foreach (var building in EditorFunction.Instance.buildings) {
            buildingNames.Add(building.name);
        }
        building.AddOptions(buildingNames);

        List<string> unitNames = new List<string>();
        foreach (var unit in EditorFunction.Instance.units) {
            unitNames.Add(unit.name);
        }
        unit.AddOptions(unitNames);
    }

    public void ShowTile(Tile tile, TileData tileData) {
        content.SetActive(true);
        noContent.SetActive(false);

        currentTile = tile;

        SetDecorationNames();

        terrain.value = EditorFunction.Instance.terrains.IndexOf(tile.terrainType);
        building.value = EditorFunction.Instance.buildings.IndexOf(tileData.buildingHere) + 1;
        unit.value = EditorFunction.Instance.units.IndexOf(tileData.unitHere) + 1;
    }

    private void SetDecorationNames() {
        decoration.ClearOptions();

        List<string> decoNames = new List<string>();
        decoNames.Add("None");
        foreach (var decorations in currentTile.terrainType.decorations) {
            decoNames.Add(decorations.name);
        }
        decoration.AddOptions(decoNames);
        decoration.value = currentTile.decoIndex + 1;
        if (decoration.value != 0) {
            decoRotation.gameObject.SetActive(true);
            decoRotation.value = currentTile.decoRotation.y;
        } else {
            decoRotation.gameObject.SetActive(false);
        }
        
    }

    public void HideTile() {
        content.SetActive(false);
        noContent.SetActive(true);
    }

    public void RotationUpdate(float value) {
        float incValue = Mathf.Round(value / 60) * 60;
        decoRotation.value = incValue;
        Vector3 newRotation = new Vector3(0, incValue, 0);
        currentTile.decoration.transform.rotation = Quaternion.Euler(newRotation);
        EditorFunction.Instance.UpdateTileData(currentTile);
    }

    public void TerrainUpdate(int index) {
        var terrainType = EditorFunction.Instance.terrains[index];
        currentTile.terrainType = terrainType;
        currentTile.SetTerrain();
        EditorFunction.Instance.UpdateTileData(currentTile);
        SetDecorationNames();
    }

    public void DecorationUpdate(int index) {

    }
}
