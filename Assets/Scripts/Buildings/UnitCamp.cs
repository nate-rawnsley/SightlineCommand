using UnityEngine;

public class UnitCamp : Building {
    [Header("Units sold here")]
    public BuildingBuyMenu availableUnits;

    private void Start() {
        canActivate = true;
    }

    public override void SpawnBehaviour() {
        if (GameManager.Instance != null) {
            GameManager.Instance.AddTokens(team, 5);
        }
    }

    public override bool ActivateBehaviour() {
        GameManager.Instance.gameUI.ShowUnitBuyMenu(this);
        return true;
    }
}
