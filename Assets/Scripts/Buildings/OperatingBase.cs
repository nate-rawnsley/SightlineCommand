using UnityEngine;

public class OperatingBase : UnitCamp {
    protected override void DeathBehaviour() {
        GameManager.instance.EndGame(team);
    }
}
