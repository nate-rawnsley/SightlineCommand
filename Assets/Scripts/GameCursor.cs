using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCursor : CursorControls {
    [SerializeField]
    private Unit activeUnit = null;
    private Unit EnemyUnit;
    private bool HasSelection = false;

    private int CurrentTeam = 1;

    private TextMeshPro Values;
    //modes
    public enum UnitMode { None, Attack, Move, Build, End}
    public UnitMode currentMode = UnitMode.None;

    [SerializeField]
    private BuildingPanel buildingPanel;

    public override void UnitClickBehaviour(Unit unit) {
        if (buildingPanel.gameObject.activeSelf) {
            buildingPanel.HidePanel();
        }
        if (HasSelection == false && currentMode != UnitMode.None) {
            activeUnit = unit;
            HasSelection = true;
            Values = unit.GetComponentInChildren<TextMeshPro>();
            

            switch (currentMode) {
                case UnitMode.Attack:
                    if (unit.CurrentAttacks > 0)
                    {
                        Values.text = unit.CurrentAttacks.ToString();
                        unit.CurrentMoveableCol = unit.moveableCol[0];
                        unit.MarkAdjacentTiles(unit.currentTile, 0, unit.AttackRange);
                        
                    }
                    else
                    {
                        CLEARALL();
                    }
                    break;

                case UnitMode.Move:
                    Debug.Log("click");
                    Values.text = unit.CurrentMove.ToString();
                    if (activeUnit.CurrentMove != 0) {
                        unit.BeginMove();
                        Debug.Log("DID MOVE");
                    } else {
                        CLEARALL();

                    }
                    break;

                case UnitMode.Build:
                    if (activeUnit.currentTile.buildingHere == null) {
                        activeUnit.CreateBuilding(0);
                    }
                    CLEARALL();
                    break;
            }
        } else if (currentMode == UnitMode.Attack) {
            //If a unit is clicked in attack mode, deal damage to it as if its tile was clicked on instead.
            TileClickBehaviour(unit.currentTile);
        }
    }

    protected override void TileClickBehaviour(Tile tile) {
        if (buildingPanel.gameObject.activeSelf) {
            buildingPanel.HidePanel();
        }
        bool acted = false;
        if (activeUnit != null) {

            switch (currentMode) {
                case UnitMode.Attack:
                    if (tile.unitHere != activeUnit & tile.unitHere.team != activeUnit.team) {
                        activeUnit.EndTargeting(activeUnit.currentTile, 0, activeUnit.AttackRange);
                        doDamage(tile);
                        acted = true;
                    }
                    break;

                case UnitMode.Move:
                    if (tile.terrainType.walkable)
                    {
                        activeUnit.EndMove(tile); //Clears all highlighted tiles
                        acted = true;
                    }
                    break;

            }
            if (acted) {

                CLEARALL();
            }
        }

    }

    protected override void BuildingClickBehaviour(Building building) {
        if (buildingPanel.gameObject.activeSelf) {
            buildingPanel.HidePanel();
        }
        if (activeUnit == null) {
            buildingPanel.SetBuilding(building);
        } else {
            TileClickBehaviour(building.tile);
        }
        
    }

    protected override void RightClickBehaviour()
    {
        CLEARALL();  //
    }

    protected void doDamage(Tile tile) {
        if (tile.unitHere) {
            EnemyUnit = tile.unitHere;
            EnemyUnit.TakeDamage();
            activeUnit.CurrentAttacks--;
            Debug.Log("DONEDAMAGE2");
        }
    }

    public void Move() {
        if (currentMode == UnitMode.Move) {
            currentMode = UnitMode.None;
        } else {
            currentMode = UnitMode.Move;
        }
    }

    public void Attack() {
        if (currentMode == UnitMode.Attack) {
            currentMode = UnitMode.None;
        } else {
            currentMode = UnitMode.Attack;
        }
    }

    public void Build() {
        if (currentMode == UnitMode.Build) {
            currentMode = UnitMode.None;
        } else {
            currentMode = UnitMode.Build;
        }
    }
    public void EndTurn()
    {
        switch (CurrentTeam) {
            case 1:
                CurrentTeam = 2;

                break;
            case 2:
                CurrentTeam = 1;
                break;
                }
        
    }

    public void CLEARALL()
    {
        activeUnit.EndTargeting(activeUnit.currentTile, 0, activeUnit.AttackRange);
        activeUnit = null;       //Clears all selections                                       
        HasSelection = false;    //
        Values.text = "";
        Values = null;
    }
}