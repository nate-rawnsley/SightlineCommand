using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI modeDisplay;
    [SerializeField]
    private TextMeshProUGUI teamDisplay;
    [SerializeField]
    private Image teamBackground;
    [SerializeField]
    private TextMeshProUGUI winDisplay;

    [HideInInspector] public GameCursor gameCursor;

    [SerializeField]
    private TeamUIParams[] teamUI = new TeamUIParams[2];

    private GameObject turnPanel;
    [HideInInspector] public GameObject buildingPanel;
    private GameObject winPanel;

    private void Awake() {
        turnPanel = transform.Find("Turn Panel").gameObject;
        buildingPanel = transform.Find("Building Panel").gameObject;
        winPanel = transform.Find("Game End Panel").gameObject;
    }

    public void UpdateModeDisplay(int modeIndex) {
        gameCursor.SetBehaviour(modeIndex);
        modeDisplay.text = $"Current mode: {gameCursor.currentMode}";
    }

    public void UpdateTeamDisplay() {
        TeamUIParams team = teamUI[(int)gameCursor.CurrentTeam];
        teamDisplay.text = $"Team: {team.team}";
        teamBackground.sprite = team.background;
        teamDisplay.color = team.textColor;
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
        GameManager.instance.RestartGame();
    }
}
