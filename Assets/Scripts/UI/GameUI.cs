using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI modeDisplay;
    [SerializeField]
    private TextMeshProUGUI teamDisplay;

    [SerializeField]
    private GameCursor gameCursor;

    public void UpdateModeDisplay() {
        modeDisplay.text = $"Current mode: {gameCursor.currentMode}";
    }
    public void UpdateTeamDisplay()
    {
        teamDisplay.text = $"Current Team: {gameCursor.CurrentTeam}";
    }
}
