using UnityEngine;
using TMPro;

public class BuildingUnitEntry : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI unitType;

    [SerializeField]
    private TextMeshProUGUI currentHP;

    private Unit unit;
    private Building building;

    private GameCursor gameCursor;
    private BuildingPanel buildPanel;

    private void Awake() {
        gameCursor = Camera.main.GetComponent<GameCursor>();
    }

    public void Initialize(Unit thisUnit, Building thisBuilding, BuildingPanel bp) { 
        unit = thisUnit;
        unitType.text = unit.gameObject.name;
        currentHP.text = $"HP: {unit.Health}/{unit.MaxHealth}";
        building = thisBuilding;
        buildPanel = bp;
        Debug.Log(GetComponent<RectTransform>().localPosition);
    }

    public void SelectUnit() {
        building.OnExitBehaviour(unit);
        gameCursor.UnitClickBehaviour(unit);
        buildPanel.HidePanel();
    }
}
