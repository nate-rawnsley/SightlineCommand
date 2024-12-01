using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCursor : CursorControls {
    private Unit activeUnit = null;
    private Unit EnemyUnit;
    private bool HasSelection = false;
    //modes
    public enum UnitMode { None, Attack, Move, Build }
    public UnitMode currentMode = UnitMode.None;

    protected override void UnitClickBehaviour(Unit unit) {
        if (HasSelection == false) {
            activeUnit = unit;
            HasSelection = true;
            Debug.Log(currentMode);

            switch (currentMode) {
                case UnitMode.Attack:
                    unit.CurrentMoveableCol = unit.moveableCol[0];
                    unit.MarkAdjacentTiles(unit.currentTile, 0, unit.AttackRange);
                    break;

                case UnitMode.Move:
                    if (activeUnit.CurrentMove != 0) {
                        unit.BeginMove();
                        Debug.Log("DIDMOVE");
                    }
                    break;

                case UnitMode.Build:
                    activeUnit.CreateBuilding(0);
                    activeUnit = null;
                    HasSelection = false;
                    break;
            }
        } else if (currentMode == UnitMode.Attack) {
            //If a unit is clicked in attack mode, deal damage to it as if its tile was clicked on instead.
            TileClickBehaviour(unit.currentTile);
        }
    }

    protected override void TileClickBehaviour(Tile tile) {
        if (activeUnit != null) {

            Debug.Log(tile.unitHere);
            switch (currentMode) {
                case UnitMode.Attack:
                    if (tile.unitHere != activeUnit) {
                        activeUnit.EndTargeting(activeUnit.currentTile, 0, activeUnit.AttackRange);
                        doDamage(tile);
                    }
                    break;

                case UnitMode.Move:
                    activeUnit.EndMove(tile); //Clears all highlighted tiles
                    break;

            }
            activeUnit = null;       //Clears all selections                                       
            HasSelection = false;    //
        }

    }
    protected void doDamage(Tile tile) {
        if (tile.unitHere) {
            EnemyUnit = tile.unitHere;
            EnemyUnit.TakeDamage();
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
}