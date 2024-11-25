using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TileTerrain {
    public string name;
    public Material material;
    public float travelSpeed = 1;
    public bool walkable = true;
    public int editorIndex;
    public List<GameObject> decorations;
    [Range(0, 1)] public float decorationFrequency;
}
