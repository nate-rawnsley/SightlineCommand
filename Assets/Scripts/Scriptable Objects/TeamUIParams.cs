using UnityEngine;

/// <summary>
/// Nate
/// The visual differences in the UI when separate teams are active.
/// These parameters are applied by the GameUI to create visual distinction.
/// </summary>
[CreateAssetMenu(menuName = "SightlineCommand/TeamUIParameters")]
public class TeamUIParams : ScriptableObject {
    public string teamName;
    public Sprite background;
    public Color textColor;
    public Color buttonColor;
}
