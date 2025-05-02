using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Nate
/// Object that generates the game's grid on start.
/// Can load a save, but if none is selected it will randomly generate a grid with the set parameters.
/// </summary>
public class GridGenerator : MonoBehaviour {
    [SerializeField, Tooltip("Define the different types of terrain in this map.")]
    public List<TileTerrain> terrainTypes = new List<TileTerrain>();

    [SerializeField, Tooltip("Which level to load. Leave blank if you want to randomly generate a new one with below paramters.")]
    private LevelSave levelSave;
    private TileData[,] loadedTiles;

    [SerializeField, Tooltip("How big each tile is."), Range(0.1f, 10)]
    public float scale = 1;

    [SerializeField, Tooltip("How much gap is between each tile."), Range(1, 1.25f)]
    public float gapScale = 1.05f;

    [SerializeField, Tooltip("The number of tiles in the grid."), Range(1,50)]
    public int width = 10, height = 10;

    [SerializeField]
    private Building humanFOB;

    [SerializeField]
    private Building alienFOB;

    //debug bool for testing & creating unit functionality
    [SerializeField]
    private bool testUnits;

    [SerializeField]
    private GameObject Humanunit;
    [SerializeField]
    private GameObject Alienunit;

    [SerializeField]
    private int testSoldierAmount;
    [SerializeField]
    private int testAlienAmount;

    private GameObject gridParent;

    [HideInInspector]
    public bool inEditor = false;

    private void Awake() {
        //Load the parameters and tile data from selected file, if one is selected.
        if (levelSave != null) {
            scale = levelSave.scale;
            gapScale = levelSave.gapScale;
            width = levelSave.width;
            height = levelSave.height;
            loadedTiles = new TileData[width,height];
            foreach (var tile in levelSave.tiles) {
                int x = (int)tile.coords.x;
                int z = (int)tile.coords.y;
                loadedTiles[x, z] = tile;
            }
        }
        gridParent = new GameObject("Grid");
    }
    
    /// <summary>
    /// Creates a grid within the selected/loaded parameters and spawns it in-game.
    /// If a save was loaded, any details from the save (eg. decorations, units) are re-created.
    /// Called at the start of the game (or when level editor is opened).
    /// </summary>
    public void GenerateGrid() {
        GameManager.Instance.SetGridSize(width, height);

        Vector3 position = new Vector3(0, 0, 0);

        float xOffset = (width * scale * gapScale) / 2;
        float zOffset = (height * scale * gapScale) / 2;

        Camera.main.GetComponent<CameraMovement>().SetInitialPosition(scale);

        //Hexagon generation adapted from https://catlikecoding.com/unity/tutorials/hex-map/part-1/
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                position.x = ((x + z * 0.5f - z / 2) * scale * gapScale) - xOffset;
                position.z = (z * 0.866f * scale * gapScale) - zOffset;
                GameObject tilePrefab = (GameObject)Resources.Load("HexTile");
                GameObject gridTile = Instantiate(tilePrefab, gridParent.transform);
                gridTile.transform.localPosition = position;
                gridTile.transform.localScale = new Vector3(scale, scale, scale);

                Tile tileScript = gridTile.AddComponent<Tile>();
                GameManager.Instance.tiles[x,z] = tileScript;
                
                //Re-creates details from save, if applicable. If not, terrain and decoration are randomly picked.
                if (loadedTiles != null) {
                    tileScript.LoadTile(loadedTiles[x, z]);
                } else {
                    tileScript.terrainType = terrainTypes[Random.Range(0, terrainTypes.Count)];
                    tileScript.SetTerrain();
                }
                tileScript.coords = new Vector2(x, z);
            }
        }

        //Finds every tile's adjacent neighbours, once they all have been placed.
        foreach (Tile tile in GameManager.Instance.tiles) {
            int x = (int)tile.coords.x;
            int z = (int)tile.coords.y;
            Vector2 index = new Vector2(x, z);

            if (x > 0) {
                AddAdjacentTiles(tile, GameManager.Instance.tiles[x - 1, z]);
            }
            if (z > 0) {
                AddAdjacentTiles(tile, GameManager.Instance.tiles[x, z - 1]);
            }
            if (z % 2 == 0) {
                if (z > 0 && x > 0) {
                    AddAdjacentTiles(tile, GameManager.Instance.tiles[x - 1, z - 1]);
                }
            } else {
                if (z > 0 && x < width - 1) {
                    AddAdjacentTiles(tile, GameManager.Instance.tiles[x + 1, z - 1]);
                }
            }
        }
        

        //here for testing unit movement
        if (testUnits)
        {
            for (int p = 0; p < testSoldierAmount; p++) //multiple test units done by Dylan
            {
                GameObject FriendObj = Instantiate(Humanunit);
                Humanunit.name = ("Soldier" + p).ToString();
                FriendObj.GetComponent<Unit>().UnitSpawn(GameManager.Instance.tiles[0, p]); //does a for loop for each team to spawn a set amount of units in inspector for teting
            }
            for (int e = 0; e < testAlienAmount; e++) {
                GameObject EnemyObj = Instantiate(Alienunit);
                Alienunit.name = ("Alien" + e).ToString();
                EnemyObj.GetComponent<Unit>().UnitSpawn(GameManager.Instance.tiles[width - 1, e]);
            }
        } else if (!inEditor && levelSave == null) { //If no save is loaded, places FOBs for both players so the game is playable.
            GameManager.Instance.tiles[0, 0].CreateBuilding(humanFOB);
            GameManager.Instance.tiles[width-1, height-1].CreateBuilding(alienFOB);
        }
    }

    /// <summary>
    /// Adds two tiles into each others' lists of adjacent tiles.
    /// </summary>
    private void AddAdjacentTiles(Tile tile1, Tile tile2) {
        tile1.adjacentTiles.Add(tile2);
        tile2.adjacentTiles.Add(tile1);
    }
}
