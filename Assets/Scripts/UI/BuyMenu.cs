using UnityEngine;

public class BuyMenu : MonoBehaviour {
    [SerializeField]
    private GameObject buyableUnitEntry;

    [SerializeField]
    private Transform scrollContent;

    public Building building;

    public void Initialize(Building source) {
        building = source;
        for (int i = 0; i < building.availableUnits.Count; i++) {
            GameObject newEntry = Instantiate(buyableUnitEntry);
            newEntry.transform.SetParent(scrollContent, true);

            Vector2 entryPos = new Vector2(0, -90 + i * -175);

            newEntry.GetComponent<BuyableUnitEntry>().Initialize(building.availableUnits[i], this);

            RectTransform rect = newEntry.GetComponent<RectTransform>();
            rect.localPosition = entryPos;
            rect.localScale = new Vector3(1,1,1);
        }
    }

    public void OptionSelected(UnitShopValue unitVals) {
        if (building.BuyUnit(unitVals)) {
            gameObject.SetActive(false);
        }
    }

    public void HideMenu() {
        gameObject.SetActive(false);
    }
}
