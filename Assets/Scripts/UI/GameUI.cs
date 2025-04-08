using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameUI : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI modeDisplay;
    [SerializeField]
    private TextMeshProUGUI teamDisplay;
    [SerializeField]
    private TextMeshProUGUI statsDisplay;
    [SerializeField]
    private Image[] teamBackgrounds;
    [SerializeField]
    private TextMeshProUGUI winDisplay;
    

    private GameCursor gameCursor;

    [SerializeField]
    private TeamUIParams[] teamUI = new TeamUIParams[2];

    private GameObject turnPanel;
    public GameObject buildingPanel;
    private GameObject winPanel;
    [SerializeField]
    private BuyMenu buyMenu;

    private void Start() {
        turnPanel = transform.Find("Turn Panel").gameObject;
        winPanel = transform.Find("Game End Panel").gameObject;
        gameCursor = GameManager.Instance.gameCursor;
    }

    public void UpdateModeDisplay(int modeIndex) {
        gameCursor.SetBehaviour(modeIndex);
        modeDisplay.text = $"Current mode: {gameCursor.currentMode}";
    }

    public void UpdateTeamDisplay() {
        TeamUIParams team = teamUI[(int)gameCursor.CurrentTeam];
        teamDisplay.text = $"Team: {team.team}";
        foreach (var background in teamBackgrounds) {
            background.sprite = team.background;
        }
        teamDisplay.color = team.textColor;
        statsDisplay.color = team.textColor;
        UpdateStats();
    }

    public void UpdateStats() {
        int material = GameManager.Instance.players[gameCursor.CurrentTeam].material;
        int tokens = GameManager.Instance.players[gameCursor.CurrentTeam].troopTokens;
        statsDisplay.text = $"Material: {material}\nUnit Tokens: {tokens}";
    }

    public void EndTurn() {
        gameCursor.EndTurn();
        UpdateTeamDisplay();
    }

    public void ShowBuildingPanel() {
        buildingPanel.SetActive(true);
        turnPanel.SetActive(false);
    }

    public void HideBuildingPanel() {
        buildingPanel.SetActive(false);
        turnPanel.SetActive(true);
    }

    public void GameStart() {
        turnPanel.SetActive(true);
        buildingPanel.SetActive(false);
        winPanel.SetActive(false);
        UpdateTeamDisplay();
    }

    public void ShowBuyMenu(UnitCamp source) {
        buyMenu.gameObject.SetActive(true);
        buyMenu.Initialize(source);
        buildingPanel.SetActive(false);
    }

    public void DisplayGameOver(PlayerTeam defeatedTeam) {
        turnPanel.SetActive(false);
        winPanel.SetActive(true);
        switch (defeatedTeam) {
            case PlayerTeam.HUMAN:
                winDisplay.text = "Aliens Win!";
                break;
            case PlayerTeam.ALIEN:
                winDisplay.text = "Humans Win!";
                break;
        }
    }

    public void RestartGame() {
        GameManager.Instance.RestartGame();
    }
}
