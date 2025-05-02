using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nate
/// A scriptable object that contains the selection of units a building can hire.
/// Scriptable object is used so the data can be easily reused for other buildings.
/// </summary>
[CreateAssetMenu(menuName = "SightlineCommand/Building Buy Menu")]
public class BuildingBuyMenu : ScriptableObject {
    public List<Unit> units;
}
