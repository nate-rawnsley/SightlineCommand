using System.Collections.Generic;
using UnityEngine;

public class BuyMenu : MonoBehaviour {
    [SerializeField]
    private GameObject buyableUnitEntry;

    [SerializeField]
    private Transform scrollContent;

    private List<GameObject> entries = new List<GameObject>();

    public UnitCamp building;

    public void Initialize(UnitCamp source) {
        building = source;
        for (int i = 0; i < building.availableUnits.units.Count; i++) { 
            GameObject newEntry = Instantiate(buyableUnitEntry);
            newEntry.transform.SetParent(scrollContent, true);

            Vector2 entryPos = new Vector2(0, -90 + i * -175);

            newEntry.GetComponent<BuyableUnitEntry>().Initialize(building.availableUnits.units[i], this);

            RectTransform rect = newEntry.GetComponent<RectTransform>();
            rect.localPosition = entryPos;
            rect.localScale = new Vector3(1,1,1);
            entries.Add(newEntry);
        }
    }

    public void OptionSelected(UnitShopValue unitVals) {
        building.BuyUnit(unitVals);
            HideMenu();
        //add need more tokens indicator
    }

    public void HideMenu() {
        foreach (var entry in entries) {
            Destroy(entry);
        }
        entries.Clear();
        gameObject.SetActive(false);
    }
}
