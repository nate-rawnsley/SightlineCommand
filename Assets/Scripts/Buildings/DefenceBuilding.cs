using UnityEngine;
using System.Collections.Generic;

public class DefenceBuilding : Building {
    [SerializeField]
    private int range = 1, damage = 1;

    private List<Tile> tilesInRange = new List<Tile>();

    public override void SpawnBehaviour() {
        tilesInRange = tile.GetAdjacentGroup(range);
        foreach (var rangeTile in tilesInRange) {
            rangeTile.UnitMovedHere += UnitEnterRange;
        }
    }

    protected override void DeathBehaviour() {
        foreach (var rangeTile in tilesInRange) {
            rangeTile.UnitMovedHere -= UnitEnterRange;
        }
        base.DeathBehaviour();
    }

    public void UnitEnterRange(Unit unit) {
        if (unit.team != team) {
            unit.TakeDamage(damage);
        }
    }
}
