using System.Collections.Generic;
using UnityEngine;
using static GameCursor;

public enum PlayerTeam {
    HUMAN,
    ALIEN
}

/// <summary>
/// This class holds data relating the game, most prominently the stats of both players.
/// It is a MonoBehaviour, a component of the 'Game Manager' object in the scene.
/// </summary>
public class GameManager : MonoBehaviour {
    [SerializeField]
    private GridGenerator gridGenerator;

    [SerializeField]
    private GameCursor gameCursor;

    [SerializeField]
    private GameUI gameUI;

    public Dictionary<PlayerTeam, PlayerStats> players = new Dictionary<PlayerTeam, PlayerStats>();
    public Tile[,] tiles;

    private void Start() {
        StartGame();
    }

    public void SetGridSize(int x, int z) {
        tiles = new Tile[x, z];
    }

    public void ClearTiles() {
        foreach (var tile in tiles) {
            Destroy(tile.gameObject);
        }
    }

    public void RestartGame() {
        gameCursor.CurrentTeam = PlayerTeam.HUMAN;
        gameCursor.currentMode = UnitMode.None;
        gameCursor.CLEARALL();
        gameUI.UpdateModeDisplay();
        gameUI.UpdateTeamDisplay();
        players[PlayerTeam.HUMAN].Destroy();
        players[PlayerTeam.ALIEN].Destroy();
        ClearTiles();
        StartGame();
    }

    public void StartGame() {
        players[PlayerTeam.HUMAN] = new PlayerStats(PlayerTeam.HUMAN);
        players[PlayerTeam.ALIEN] = new PlayerStats(PlayerTeam.ALIEN);
        players[PlayerTeam.HUMAN].otherPlayer = players[PlayerTeam.ALIEN];
        players[PlayerTeam.ALIEN].otherPlayer = players[PlayerTeam.HUMAN];
        gridGenerator.GenerateGrid();
    }

    public void NewTurn(PlayerTeam team) {
        players[team].StartTurn();
    }
}
