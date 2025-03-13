using System.Collections.Generic;
using UnityEngine;

public enum PlayerTeam {
    HUMAN,
    ALIEN
}

/// <summary>
/// This class holds data relating the game, most prominently the stats of both players.
/// It is a MonoBehaviour, a component of the 'Game Manager' object in the scene.
/// It is also a singleton, with one instance that is publically available and static.
/// </summary>
public class GameManager : MonoBehaviour {
    public static GameManager instance {  get; private set; }

    private GridGenerator gridGenerator;
    private GameCursor gameCursor;
    private GameUI gameUI;

    public Dictionary<PlayerTeam, PlayerStats> players = new Dictionary<PlayerTeam, PlayerStats>();
    public Tile[,] tiles;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }
    }

    private void Start() {
        gridGenerator = FindObjectOfType<GridGenerator>();
        gameCursor = FindObjectOfType<GameCursor>();
        gameUI = FindObjectOfType<GameUI>();
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
