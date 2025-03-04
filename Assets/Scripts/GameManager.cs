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
    private GridGenerator gridGenerator;
    private GameCursor gameCursor;
    private GameUI gameUI;

    public Dictionary<PlayerTeam, PlayerStats> players = new Dictionary<PlayerTeam, PlayerStats>();
    public Tile[,] tiles;

    private void Start() {
        gridGenerator = FindObjectOfType<GridGenerator>();
        gridGenerator.gameManager = this;
        gameCursor = FindObjectOfType<GameCursor>();
        gameCursor.gameManager = this;
        gameUI = FindObjectOfType<GameUI>();
        gameUI.gameManager = this;
        gameUI.gameCursor = gameCursor;
        gameCursor.buildingPanel = gameUI.buildingPanel.GetComponent<BuildingPanel>();
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
        gameCursor.CLEARALL();
        gameUI.UpdateModeDisplay(0);
        gameUI.UpdateTeamDisplay();
        players[PlayerTeam.HUMAN].Destroy();
        players[PlayerTeam.ALIEN].Destroy();
        ClearTiles();
        StartGame();
    }

    public void StartGame() {
        PlayerStats humanStats = new PlayerStats(PlayerTeam.HUMAN);
        PlayerStats alienStats = new PlayerStats(PlayerTeam.ALIEN);
        humanStats.otherPlayer = alienStats;
        alienStats.otherPlayer = humanStats;
        players[PlayerTeam.HUMAN] = humanStats;
        players[PlayerTeam.ALIEN] = alienStats;
        gameUI.GameStart();
        gridGenerator.GenerateGrid();
    }

    public void NewTurn(PlayerTeam team) {
        players[team].StartTurn();
    }

    public void EndGame(PlayerTeam defeatedTeam) {
        gameUI.DisplayGameOver(defeatedTeam);
    }
}
