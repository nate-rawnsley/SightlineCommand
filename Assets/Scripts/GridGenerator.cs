using UnityEngine;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour {
    [SerializeField, Tooltip("The tile to populate the grid with.")]
    private GameObject tile;

    [SerializeField, Tooltip("Define the different types of terrain in this map.")]
    private List<TileTerrain> terrainTypes = new List<TileTerrain>();

    [SerializeField, Tooltip("How big each tile is."), Range(0.1f, 10)]
    private float scale = 1;

    [SerializeField, Tooltip("How much gap is between each tile."), Range(1, 1.25f)]
    private float gapScale = 1.05f;

    [SerializeField, Tooltip("The number of tiles in the grid."), Range(1,50)]
    private int width = 10, height = 10;

    private void Start() {
        GameObject gridParent = new GameObject("Grid");
        GridParent gridParentScript = gridParent.AddComponent<GridParent>();
        gridParentScript.SetGridSize(width, height);

        Vector3 position = new Vector3(0, 0, 0);

        float xOffset = (width * scale * gapScale) / 2;
        float zOffset = (height * scale * gapScale) / 2;

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                position.x = (x * scale * gapScale) - xOffset;
                position.z = (z * scale * gapScale) - zOffset;
                GameObject gridTile = Instantiate(tile, position, Quaternion.identity, gridParent.transform);
                gridTile.transform.localScale = new Vector3(scale, scale, scale);

                Tile tileScript = gridTile.AddComponent<Tile>();
                gridParentScript.tiles[x,z] = tileScript;

                tileScript.terrainType = terrainTypes[Random.Range(0, terrainTypes.Count)];
                gridTile.GetComponent<Renderer>().material = tileScript.terrainType.material;

                if (x > 0) {
                    tileScript.adjacentTiles.Add(gridParentScript.tiles[x - 1, z]);
                    gridParentScript.tiles[x - 1, z].adjacentTiles.Add(tileScript);
                }
                if (z > 0) {
                    tileScript.adjacentTiles.Add(gridParentScript.tiles[x, z - 1]);
                    gridParentScript.tiles[x, z - 1].adjacentTiles.Add(tileScript);
                }
            }
        }
    }
}
