using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    [SerializeField, Tooltip("The material adjacent tiles are set to when the unit is moving.")]
    private Material moveableMat;

    private Tile currentTile;
    private float scale;

    public void UnitSpawn(Tile tile) {
        tile.unitHere = this;
        currentTile = tile;
        scale = currentTile.transform.localScale.x;
        transform.localScale = transform.localScale * scale * 0.5f;
        MoveToTile();
    }

    private void MoveToTile() {
        Vector3 position = currentTile.transform.position;
        position.y += scale * 0.65f;
        transform.position = position;
    }

    public void BeginMove() {
        foreach (Tile adjacentTile in currentTile.adjacentTiles) {
            adjacentTile.GetComponent<Renderer>().material = moveableMat;
        }
    }

    public void EndMove(Tile targetTile) {
        foreach (Tile adjacentTile in currentTile.adjacentTiles) {
            adjacentTile.GetComponent<Renderer>().material = adjacentTile.terrainType.material;
        }
        if (currentTile.adjacentTiles.Contains(targetTile)) {
            currentTile = targetTile;
            MoveToTile();
        }
    }
}
