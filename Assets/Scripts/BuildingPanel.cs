using System.Collections.Generic;
using UnityEngine;

public class BuildingPanel : MonoBehaviour {

    [SerializeField]
    private GameObject buildingUnitEntry;

    [SerializeField]
    private Transform scrollContent;

    private Building building;
    private List<GameObject> entries = new List<GameObject>();

    public void SetBuilding(Building selectedBuilding) {
        Debug.Log(building);
        gameObject.SetActive(true);
        building = selectedBuilding;
        for (int i = 0; i < building.unitsHere.Count; i++) {
            GameObject newEntry = Instantiate(buildingUnitEntry);
            newEntry.transform.SetParent(scrollContent, true);

            RectTransform trans = newEntry.GetComponent<RectTransform>();
            trans.localPosition = new Vector2(200, -75 + i * -125);
            trans.localScale = new Vector2(1, 1);

            Unit unit = building.unitsHere[i].GetComponent<Unit>();
            newEntry.GetComponent<BuildingUnitEntry>().Initialize(unit, building, this);
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

}
