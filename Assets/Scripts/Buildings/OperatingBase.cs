using UnityEngine;

public class OperatingBase : UnitCamp {
    protected override void DeathBehaviour() {
        GameManager.Instance.EndGame(team);
    }
}
