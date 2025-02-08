using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTeam {
    HUMAN,
    ALIEN
}

public class GameStats : MonoBehaviour {
    public Tile[,] tiles;
    public Dictionary<PlayerTeam, PlayerStats> players = new Dictionary<PlayerTeam, PlayerStats>();

    public void StartGame() {
        players[PlayerTeam.HUMAN] = new PlayerStats(PlayerTeam.HUMAN);
        players[PlayerTeam.ALIEN] = new PlayerStats(PlayerTeam.ALIEN);
        players[PlayerTeam.HUMAN].otherPlayer = players[PlayerTeam.ALIEN];
        players[PlayerTeam.ALIEN].otherPlayer = players[PlayerTeam.HUMAN];
    }

    public void SetGridSize(int x, int z) {
        tiles = new Tile[x, z];
    }
}
