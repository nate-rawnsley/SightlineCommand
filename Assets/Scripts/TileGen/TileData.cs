using UnityEngine;

/// <summary>
/// Nate
/// The data on each tile in the level editor, saved by LevelSaveEditor into a LevelSave object.
/// </summary>
[System.Serializable]
public class TileData {
    public TileTerrain terrainType;
    public GameObject decoration;
    public Vector3 decorationRotation;
    public GameObject buildingHere;
    public GameObject unitHere;
    public Vector2 coords;

    public TileData(Tile tile) {
        UpdateTerrain(tile);
        coords = tile.coords;
    }

    public void UpdateTerrain(Tile tile) { 
        terrainType = tile.terrainType;
        if (tile.decoIndex == -1) {
            decoration = null;
        } else {
            decoration = terrainType.decorations[tile.decoIndex];
            decorationRotation = tile.decoRotation;
        }
    }
}
