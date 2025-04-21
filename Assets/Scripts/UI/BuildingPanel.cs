using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingPanel : MonoBehaviour {

    [SerializeField]
    private GameObject buildingUnitEntry;

    [SerializeField]
    private Transform scrollContent;

    [SerializeField]
    private HealthBar healthBar;

    private GameHandsUI gameUI;

    private Building building;
    private List<GameObject> entries = new List<GameObject>();

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI tipText;
    private TextMeshProUGUI commandText;

    private void Awake() {
        gameUI = GetComponentInParent<GameHandsUI>();
        nameText = transform.Find("BuildingName").GetComponent<TextMeshProUGUI>();
        tipText = transform.Find("ToolTip").GetComponent<TextMeshProUGUI>();
        commandText = transform.Find("ActiveButton/Action").GetComponent<TextMeshProUGUI>();
    }

    public void SetBuilding(Building selectedBuilding) {
        gameUI.ShowBuildingPanel();
        building = selectedBuilding;
        nameText.text = building.buildingName;
        tipText.text = building.toolTip;
        if (building.unitInCreation != null) {
            tipText.text += $"\nHiring: {building.unitInCreation.GetComponent<Unit>().displayName}, complete in {building.turnsToCreate} turn(s)";
        }
        commandText.text = building.command;
        for (int i = 0; i < building.unitsHere.Count; i++) {
            GameObject newEntry = Instantiate(buildingUnitEntry);
            newEntry.transform.SetParent(scrollContent, true);

            Vector2 entryPos = new Vector2(181.381424f, -75 + i * -125);

            Unit unit = building.unitsHere[i].GetComponent<Unit>();
            newEntry.GetComponent<BuildingUnitEntry>().Initialize(unit, building, this, entryPos);
            entries.Add(newEntry);
        }
        healthBar.DisplaySpecified(building.maxHealth, building.health, building.team, true);
    }
    
    public void HidePanel() {
        foreach (GameObject entry in entries) {
            Destroy(entry);
        }
        entries.Clear();
        gameUI.HideBuildingPanel();
    }

    public void ActivateBuilding()
    {
        if (building.ActivateBehaviour()) {
            //HidePanel();
        }
    }

}
