using UnityEngine;
using TMPro;

/// <summary>
/// Nate
/// An entry used to display a unit that can be hired in the buy menu.
/// Contains a button that allows the unit to be selected and hired.
/// </summary>
public class BuyableUnitEntry : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private TextMeshProUGUI unitStats;
    [SerializeField] private TextMeshProUGUI buyStats;

    private Unit unit;
    private BuyMenu buyMenu;

    public void Initialize(Unit newUnit, BuyMenu source) {
        unit = newUnit;

        unitName.text = unit.displayName;
        unitStats.text = $"Attack: {unit.Damage}    Range: {unit.AttackRange}\n" +
            $"Health: {unit.MaxHealth}    Moves: {unit.MaxMovement}";
        buyStats.text = $"Cost:     {newUnit.tokenCost} tokens\nSpeed:  {newUnit.turnsToCreate} turn(s)";

        buyMenu = source;
    }

    public void OptionSelected() {
        buyMenu.UnitSelected(unit);
    }
}
