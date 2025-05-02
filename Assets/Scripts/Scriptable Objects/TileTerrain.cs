using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Nate
/// The data for a tile's terrain type.
/// Used by the grid generator and level editor.
/// </summary>
[CreateAssetMenu(menuName = "SightlineCommand/Tile Terrain")]
public class TileTerrain : ScriptableObject {
    public string terrainName;
    public Material material;
    public int travelSpeed = 1; //travel speed added by Dylan
    public bool walkable = true;
    public List<GameObject> decorations;
    [Range(0, 1)] public float decorationFrequency;
}