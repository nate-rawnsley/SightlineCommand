using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UIElements;

public class BuildingUnitEntry : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI unitType;

    [SerializeField]
    private TextMeshProUGUI currentHP;

    private Unit unit;
    private Building building;

    private GameCursor gameCursor;
    private BuildingPanel buildPanel;
    private RectTransform rect;
    private Vector2 test;

    private void Awake() {
        gameCursor = Camera.main.GetComponent<GameCursor>();
        rect = GetComponent<RectTransform>();
    }

    public void Initialize(Unit thisUnit, Building thisBuilding, BuildingPanel bp, Vector2 position) { 
        unit = thisUnit;
        unitType.text = unit.gameObject.name;
        currentHP.text = $"HP: {unit.Health}/{unit.MaxHealth}";
        building = thisBuilding;
        buildPanel = bp;
        
        test = position;
        StartCoroutine(DelayPosition(position));
    }

    private IEnumerator DelayPosition(Vector2 pos)
    {
        yield return null;
        rect.localPosition = pos;
    }

    private void LateUpdate()
    {
        Debug.Log($"{rect.localPosition}, {test}");
    }

    public void SelectUnit() {
        building.OnExitBehaviour(unit);
        gameCursor.UnitClickBehaviour(unit);
        buildPanel.HidePanel();
    }
}
