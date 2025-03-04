using UnityEngine;

public class OperatingBase : UnitCamp {
    protected override void DeathBehaviour() {
        GameManager gameStats = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameStats.EndGame(team);
    }
}
