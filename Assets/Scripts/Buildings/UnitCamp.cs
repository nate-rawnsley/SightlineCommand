using UnityEngine;

public class UnitCamp : Building {
    [Header("Units sold here")]
    public BuildingBuyMenu availableUnits;

    private void Start() {
        canActivate = true;
    }

    public override bool ActivateBehaviour() {
        GameManager.Instance.gameUI.ShowUnitBuyMenu(this);
        return true;
    }
}
