using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingPanel : MonoBehaviour {

    [SerializeField]
    private GameObject buildingUnitEntry;

    [SerializeField]
    private Transform scrollContent;

    private Building building;
    private List<GameObject> entries = new List<GameObject>();

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI tipText;
    private TextMeshProUGUI commandText;

    private void Awake() {
        nameText = transform.Find("BuildingName").GetComponent<TextMeshProUGUI>();
        tipText = transform.Find("ToolTip").GetComponent<TextMeshProUGUI>();
        commandText = transform.Find("ActiveButton/Action").GetComponent<TextMeshProUGUI>();
    }

    public void SetBuilding(Building selectedBuilding) {
        gameObject.SetActive(true);
        building = selectedBuilding;
        nameText.text = building.buildingName;
        tipText.text = building.toolTip;
        if (building.unitInCreation != null) {
            tipText.text += $"\nHiring: {building.unitInCreation}, complete in {building.turnsToCreate} turn(s)";
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
    }
    
    public void HidePanel() {
        foreach (GameObject entry in entries) {
            Destroy(entry);
        }
        entries.Clear();
        gameObject.SetActive(false);
    }

    public void ActivateBuilding()
    {
        if (building.ActivateBehaviour()) {
            HidePanel();
        }
    }

}
