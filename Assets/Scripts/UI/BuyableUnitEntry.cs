using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyableUnitEntry : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private TextMeshProUGUI unitStats;
    [SerializeField] private TextMeshProUGUI buyStats;

    private UnitShopValue shopValue;
    private BuyMenu buyMenu;

    public void Initialize(UnitShopValue newShopValue, BuyMenu source) {
        shopValue = newShopValue;

        unitName.text = shopValue.unitPrefab.name;

        Unit unit = shopValue.unitPrefab.GetComponent<Unit>();
        unitStats.text = $"Attack: {unit.Damage}    Range: {unit.AttackRange}\n" +
            $"Health: {unit.MaxHealth}    Moves: {unit.MaxMovement}";

        buyStats.text = $"Cost:     {newShopValue.price} gold\nSpeed:  {newShopValue.createSpeed} turn(s)";

        buyMenu = source;
    }

    public void OptionSelected() {
        buyMenu.OptionSelected(shopValue);
    }
}
