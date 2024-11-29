using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Tooltip("The colour adjacent tiles are set to when the unit is moving.")]
    public Color[] moveableCol;
    public Color CurrentMoveableCol;

    public bool canBuild = true;
    [Tooltip("Every building this unit can create (if applicable)")]
    public List<Building> createableBuildings;

    public Tile currentTile;

    protected float scale;

    [Header("Troop Settings")]
    [SerializeField]
    protected int MaxMovement;
    [SerializeField]
    protected int Health;
    [SerializeField]
    protected int Damage;
    [SerializeField]
    public int AttackRange;


    public int CurrentMove;

    public void Start()
    {
        CurrentMoveableCol = moveableCol[0]; //sets up the moveable material
        CurrentMove = MaxMovement;
    }
    //Movement///////////////////////////////////////////// Base Movement done by Nate, Limiting Movement Distance and changing movement material Done By Dylan
    public void UnitSpawn(Tile tile)
    {
        tile.unitHere = this;
        currentTile = tile;

        scale = currentTile.transform.localScale.x;
        transform.localScale = transform.localScale * scale * 0.5f;
        MoveToTile();
    }
    protected void MoveToTile()
    {        
        Vector3 position = currentTile.transform.position;
        position.y += scale * 0.65f;
        transform.position = position;
        if (CurrentMove == 0)
        {
            CurrentMoveableCol = moveableCol[1]; //changes material to the NotMoveable
        }
    }
    public void BeginMove()
    {
        foreach (Tile adjacentTile in currentTile.adjacentTiles)
        {
            adjacentTile.DisplayColour(CurrentMoveableCol);
        }
    }
    public void EndMove(Tile targetTile)
    {
        

        if (CurrentMove > 0)
        {
            targetTile.unitHere = this;
            EndTargeting(currentTile, 0);
            foreach (Tile adjacentTile in currentTile.adjacentTiles)
            {
                adjacentTile.ResetMaterial();
            }
            if (currentTile.adjacentTiles.Contains(targetTile))
            {
                currentTile = targetTile;
                CurrentMove--;
                Debug.Log(CurrentMove);
                MoveToTile();
                
            }

            if (currentTile.buildingHere != null) {
                currentTile.buildingHere.OnEnterBehaviour(this);
            }
        }
        
    }
    //End Of Movement////////////////////////////////////////

    //Health///////////////////////////////////////////////// Done By Dylan

    public void TakeDamage()
    {
        Health--;
        if(Health <= 0)
        {
            Destroy(this.gameObject);
        }

    }
    //End of Health//////////////////////////////////////////

    //Damage and Targeting/////////////////////////////////// Done By Dylan

    public void MarkAdjacentTiles(Tile tileToCheck, int loopNo)
    {
        loopNo++;
        foreach (Tile adjacentTile in tileToCheck.adjacentTiles)
        {
            adjacentTile.DisplayColour(CurrentMoveableCol);
            if (loopNo < AttackRange)
            {
                MarkAdjacentTiles(adjacentTile, loopNo);
            }
        }
    }
    public void EndTargeting(Tile tileToCheck, int loopNo)
    {
        loopNo++;
        foreach (Tile adjacentTile in tileToCheck.adjacentTiles)
        {
            adjacentTile.ResetMaterial();
            if (loopNo < AttackRange)
            {
                EndTargeting(adjacentTile, loopNo);
            }
        }
    }

    //End Of Damage and Targeting/////////////////////////////

    //Colour lerping and creating buildings/////////////////// Done by Nate

    public void CreateBuilding(int index) {
        if (canBuild) {
            currentTile.CreateBuilding(createableBuildings[index]);
        }
    }

    //End of colour and buildings////////////////////////////
}
