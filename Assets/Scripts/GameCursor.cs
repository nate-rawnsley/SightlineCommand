using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCursor : CursorControls
{
    private Unit activeUnit = null;
    private Unit EnemyUnit;
    private bool HasSelection = false;

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
