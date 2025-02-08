using System.Collections.Generic;

public class PlayerStats {
    public PlayerTeam team;
    public List<Unit> units = new List<Unit>();
    public List<Building> buildings = new List<Building>();
    public PlayerStats otherPlayer;
    public int gold = 0;

    public PlayerStats(PlayerTeam team) {
        this.team = team;
    }

    public void StartTurn() {
        foreach (var unit in units) {
            unit.ResetUnit();
        }
        foreach (var building in buildings) {
            building.NewTurn();
        }
    }
}
