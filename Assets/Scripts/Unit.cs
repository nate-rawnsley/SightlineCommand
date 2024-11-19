using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    [SerializeField, Tooltip("The material adjacent tiles are set to when the unit is moving.")]
    protected Material[] moveableMat;
    protected Material CurrentMoveableMat;

    protected Tile currentTile;
    protected float scale;
    protected int MaxMove;
    protected int CurrentMove;

    public void Start()
    {
        CurrentMoveableMat = moveableMat[0];
        CurrentMove = MaxMove;
    }
    public void UnitSpawn(Tile tile) {
        tile.unitHere = this;
        currentTile = tile;
        scale = currentTile.transform.localScale.x;
        transform.localScale = transform.localScale * scale * 0.5f;
        MoveToTile();
    }

    protected void MoveToTile() {
        Vector3 position = currentTile.transform.position;
        position.y += scale * 0.65f;
        transform.position = position;
        if (CurrentMove == 0)
        {
            CurrentMoveableMat = moveableMat[1];
        }
    }

    public void BeginMove() {
        foreach (Tile adjacentTile in currentTile.adjacentTiles)
        {
            adjacentTile.GetComponent<Renderer>().material = CurrentMoveableMat;

        }
        
    }


    public void EndMove(Tile targetTile) {
        

        if (CurrentMove > 0)
        {
            foreach (Tile adjacentTile in currentTile.adjacentTiles) {
                adjacentTile.GetComponent<Renderer>().material = adjacentTile.terrainType.material;
            }
            if (currentTile.adjacentTiles.Contains(targetTile)) {
                currentTile = targetTile;
                CurrentMove--;
                Debug.Log(CurrentMove);
                MoveToTile();


            

            }
        }
    }
}
