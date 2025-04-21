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
    public static GameManager Instance {  get; private set; }

    public GridGenerator gridGenerator;
    public GameCursor gameCursor;
    public GameUI gameUI;

    public Dictionary<PlayerTeam, PlayerStats> players = new Dictionary<PlayerTeam, PlayerStats>();
    public Tile[,] tiles;

    [HideInInspector]
    public bool editorStart = false;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
        }
    }

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
        if (gameUI != null) {
            gameUI.GameStart();
        }
        
        if (!editorStart) {
            gridGenerator.GenerateGrid();
            gameCursor.active = true;
        }

        
    }

    public void NewTurn(PlayerTeam team) {
        players[team].StartTurn();
    }

    public void EndGame(PlayerTeam defeatedTeam) {
        gameCursor.active = false;
        gameUI.DisplayGameOver(defeatedTeam);
    }

    public bool UseMaterial(PlayerTeam team, int material) {
        if (players[team].material >= material) {
            players[team].material -= material;
            gameUI.UpdateStats();
            return true;
        }
        return false;
    }

    public bool UseTokens(PlayerTeam team, int tokens) {
        if (players[team].troopTokens >= tokens) {
            players[team].troopTokens -= tokens;
            gameUI.UpdateStats();
            return true;
        }
        return false;
    }
}
