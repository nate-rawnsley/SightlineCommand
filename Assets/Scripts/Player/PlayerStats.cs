using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nate
/// The stats for each individual player, created by and stored in GameManager.
/// Also contains functions that affect all units or buildings under a player's control.
/// </summary>
public class PlayerStats {
    public PlayerTeam team;
    public List<Unit> units = new List<Unit>();
    public List<Building> buildings = new List<Building>();
    public PlayerStats otherPlayer;
    public int material = 10;
    public int troopTokens = 0;

    public PlayerStats(PlayerTeam team) {
        this.team = team;
    }
    public void Destroy() {
        foreach (var unit in units) {
            GameObject.Destroy(unit.gameObject);
        }
        foreach (var building in buildings) { 
            GameObject.Destroy(building.gameObject);
        }
    }
    
    public void EndTurn() {
        foreach (var building in buildings) {
            building.EndTurn();
        }
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
