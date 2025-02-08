using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Tooltip("The name this unit displays in UI.")]
    public string displayName;

    [Tooltip("The colour adjacent tiles are set to when the unit is moving.")]
    public Color[] moveableCol;
    public Color CurrentMoveableCol;

    public bool canBuild = true;
    [Tooltip("Every building this unit can create (if applicable)")]
    public List<Building> createableBuildings;

    public Tile currentTile;

    protected float scale;

    //[SerializeField] 
    //public enum Teams { Team1, Team2 }; replaced with PlayerTeam, from GameStats.cs

    [Header("Troop Settings")]
    public PlayerTeam team;
    [SerializeField]
    public int MaxMovement;
    [SerializeField]
    public int MaxHealth;
    [SerializeField]
    public float Damage;
    [SerializeField]
    public int AttackRange;
    [SerializeField]
    public int MaxAttack;

    [Header("In-game values")]
    public int Health;
    public int CurrentMove;
    public int CurrentAttacks;
    public Vector3 unitScale;
    public HealthBar healthBar;
    public TextMeshPro valuesText;

    public void Start()
    {
        CurrentAttacks = MaxAttack;
        CurrentMoveableCol = moveableCol[0]; //sets up the moveable material
        CurrentMove = MaxMovement;
        Health = MaxHealth;
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.DisplaySpecified(MaxHealth, MaxHealth, team);
        //healthBar.gameObject.SetActive(false);
        valuesText = GetComponentInChildren<TextMeshPro>();
    }
    //Movement///////////////////////////////////////////// Base Movement done by Nate, Limiting Movement Distance and changing movement material Done By Dylan
    public void UnitSpawn(Tile tile)
    {
        tile.unitHere = this;
        currentTile = tile;

        scale = currentTile.transform.localScale.x;
        unitScale = transform.localScale * scale * 0.5f;
        transform.localScale = unitScale;
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
            if (adjacentTile.terrainType.walkable == true)
            {
                adjacentTile.DisplayColour(CurrentMoveableCol);
            }
        }
    }
    public void EndMove(Tile targetTile)
    {
        

        if (CurrentMove > 0)
        {
            targetTile.unitHere = this;
            EndTargeting(currentTile, 1, false);
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

    public void ResetMove()
    {
        CurrentMove = MaxMovement;
    }
    //End Of Movement////////////////////////////////////////

    //Health///////////////////////////////////////////////// Done By Dylan

    public void TakeDamage()
    {
        Health--;
        healthBar.Damage(1);
        if(Health <= 0)
        {
            Destroy(this.gameObject);
        }

    }
    //End of Health//////////////////////////////////////////

    //Damage and Targeting/////////////////////////////////// Done By Dylan & Nate

    //N - Moved the recursive search to Tile for other uses. Still called through here (GetAdjacentGroup)

    public void MarkAdjacentTiles(Tile tileToCheck, int maxLoops, bool dmgIndicate)
    {
        foreach (Tile tile in tileToCheck.GetAdjacentGroup(maxLoops)) {
            tile.DisplayColour(CurrentMoveableCol);
            if (dmgIndicate && tile.unitHere) {
                if (tile.unitHere.team != team) {
                    tile.unitHere.healthBar.IndicateDamage(Damage);
                }
            }
        }
    }
    public void EndTargeting(Tile tileToCheck, int maxLoops, bool dmgIndicate)
    {
        foreach (Tile tile in tileToCheck.GetAdjacentGroup(maxLoops)) {
            tile.ResetMaterial();
            if (dmgIndicate && tile.unitHere) {
                if (tile.unitHere.team != team) {
                    tile.unitHere.healthBar.StopIndicating();
                }
            }
        }
    }

    //End Of Damage and Targeting/////////////////////////////

    //Creating buildings////////////////////////////////////// Done by Nate

    public void CreateBuilding(int index) {
        if (canBuild) {
            currentTile.CreateBuilding(createableBuildings[index]);
            currentTile.buildingHere.tile = currentTile;
            currentTile.buildingHere.OnEnterBehaviour(this);

            GameManager gameStats = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            gameStats.players[team].buildings.Add(currentTile.buildingHere);
        }
    }

    //End of creating buildings//////////////////////////////
    public void ResetUnit()
    {
        CurrentMove = MaxMovement;
        CurrentAttacks = MaxAttack;
    }
}
