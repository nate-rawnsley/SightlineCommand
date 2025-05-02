using UnityEngine;

/// <summary>
/// Nate
/// Formerly used in BuildingBuyMenu for a list of troops with theoretically varying costs.
/// Due to the troop token refund on death (and not using varied costs), this was scrapped.
/// Units now contains price and createSpeed as serialized variables.
/// </summary>
[System.Serializable]
public class UnitShopValue {
    public GameObject unitPrefab;
    public int price;
    public int createSpeed;
}
