using UnityEngine;

[System.Serializable]
public class TileTerrain {
    public string name;
    public Material material;
    public float travelSpeed = 1;
    public bool walkable = true;
    public int editorIndex;
}
