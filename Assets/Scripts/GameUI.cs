using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI modeDisplay;

    [SerializeField]
    private GameCursor gameCursor;

    public void UpdateModeDisplay() {
        modeDisplay.text = $"Current mode: {gameCursor.currentMode}";
    }
}
