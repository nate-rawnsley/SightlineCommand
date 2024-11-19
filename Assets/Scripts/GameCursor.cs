using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCursor : CursorControls {
    private Unit activeUnit;

    protected override void UnitClickBehaviour(Unit unit) {
        activeUnit = unit;
        unit.BeginMove();
    }

    protected override void TileClickBehaviour(Tile tile){
        if (activeUnit != null) {
            activeUnit.EndMove(tile);
            activeUnit = null;
        }
    }
}
