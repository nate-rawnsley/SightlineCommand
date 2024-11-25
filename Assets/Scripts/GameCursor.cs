using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCursor : CursorControls {
    private Unit activeUnit = null;
    private Unit EnemyUnit;
    private bool HasSelection = false;
    //modes
    [SerializeField]
    private bool AttackMode;
    [SerializeField]
    private bool MoveMode;




    protected override void UnitClickBehaviour(Unit unit) {
        if (HasSelection == false) {
            activeUnit = unit;
            HasSelection = true;

            if (MoveMode == true)//movement
            {
                if (activeUnit.CurrentMove != 0) {
                    unit.BeginMove();
                    Debug.Log("DIDMOVE");
                }
            }


            if (AttackMode == true) //attacking
            {
                unit.CurrentMoveableMat = unit.moveableMat[0];
                unit.MarkAdjacentTiles(unit.currentTile, 0);
            }

        }
    }

    protected override void TileClickBehaviour(Tile tile) {
        if (activeUnit != null) {

            Debug.Log(tile.unitHere);
            if (AttackMode == true) {
                if (tile.unitHere != activeUnit) {
                    activeUnit.EndTargeting(activeUnit.currentTile, 0);
                    doDamage(tile);
                }

            }
            if (MoveMode == true) {
                activeUnit.EndMove(tile); //Clears all highlited Tiles
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
    public void move() {
        switch (MoveMode) {
            case true:
                MoveMode = false; break;
            case false:
                MoveMode = true;
                if (AttackMode == true) {
                    AttackMode = false;
                }
                break;
        }
    }

    public void attack() {
        switch (AttackMode) {
            case true:
                AttackMode = false;
                break;
            case false:
                AttackMode = true;
                if (MoveMode == true) {
                    MoveMode = false;
                }
                break;
        }
    }

    public void UnitBuild() {
        if (activeUnit != null) {
            activeUnit.CreateBuilding(0);
            if (MoveMode) {
                activeUnit.EndMove(activeUnit.currentTile);
            }
            if (AttackMode) {
                activeUnit.EndTargeting(activeUnit.currentTile, 0);
            }
            activeUnit = null;
            HasSelection = false;
        }
    }
}