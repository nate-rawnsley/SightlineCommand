using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCursor : CursorControls {
    private Unit activeUnit;

    protected override void UnitClickBehaviour(Unit unit) {
        if (activeUnit == null || activeUnit != unit) {
            activeUnit = unit;
            unit.BeginMove();
        } 
        //below is debug way of testing buildings can be created
        else {
            unit.MakeBuilding(0);
            activeUnit.EndMove(activeUnit.currentTile);
            activeUnit = null;
        }
    }

    protected override void TileClickBehaviour(Tile tile){
        if (activeUnit != null) {
            activeUnit.EndMove(tile);
            activeUnit = null;
        }
    }
}
