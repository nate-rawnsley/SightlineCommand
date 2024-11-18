using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public TileTerrain terrainType;
    public List<Tile> adjacentTiles = new List<Tile>();
    public Unit unitHere;
}
