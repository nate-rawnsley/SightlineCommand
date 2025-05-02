using UnityEngine;

/// <summary>
/// Nate
/// Defines the cost of a building, which scales depending on how many are active.
/// When creating a building, the list of prices is indexed by the number active to create this scale.
/// Being a scriptable object means the numberActive is shared between buildings of the same type.
/// </summary>
[CreateAssetMenu(menuName = "SightlineCommand/Building Cost Tree")]
public class BuildingCostTree : ScriptableObject {
    public int[] costs;
    public int numberActive;

    private void OnEnable() {
        numberActive = 0;
    }
}
