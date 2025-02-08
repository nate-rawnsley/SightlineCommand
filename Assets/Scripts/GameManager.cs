using System.Collections.Generic;
using UnityEngine;

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

    public GridParent gridParent;
    public Dictionary<PlayerTeam, PlayerStats> players = new Dictionary<PlayerTeam, PlayerStats>();

    private void Start() {
        StartGame();
    }

    public void RestartGame() {
        gameCursor.CurrentTeam = PlayerTeam.HUMAN;
        gameCursor.CLEARALL();
        gameUI.UpdateModeDisplay();
        gameUI.UpdateTeamDisplay();
        players[PlayerTeam.HUMAN].Destroy();
        players[PlayerTeam.ALIEN].Destroy();
        Destroy(gridParent.gameObject);
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
