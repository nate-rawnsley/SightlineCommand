using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCamp : Building {
    [Header("Units sold here")]
    public BuildingBuyMenu availableUnits;

    public override bool ActivateBehaviour() {
        GameManager.Instance.gameUI.ShowBuyMenu(this);
        return true;
    }
}
