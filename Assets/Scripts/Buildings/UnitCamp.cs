using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCamp : Building {
    [SerializeField] private GameObject buyMenu;

    [Header("Units sold here"), SerializeField]
    public List<UnitShopValue> availableUnits = new List<UnitShopValue>();

    public override bool ActivateBehaviour() {
        GameManager.Instance.gameUI.ShowBuyMenu(this);
        return true;
    }
}
