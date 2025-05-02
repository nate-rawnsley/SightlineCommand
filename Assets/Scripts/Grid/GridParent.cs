using UnityEngine;

// Nate - unused script that was replaced by GameManager.
public class GridParent : MonoBehaviour {
    public Tile[,] tiles;

    public void SetGridSize(int x, int z) {
        tiles = new Tile[x,z];
    }

    public void ClearTiles() {
        foreach (var tile in tiles) { 
            Destroy(tile.gameObject);
        }
    }
}
