using UnityEngine;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour {

    [SerializeField]
    private GameManager gameManager;

    [SerializeField, Tooltip("The tile to populate the grid with.")]
    private GameObject tilePrefab;

    [SerializeField, Tooltip("Define the different types of terrain in this map.")]
    public List<TileTerrain> terrainTypes = new List<TileTerrain>();

    [SerializeField, Tooltip("Which level to load. Leave blank if you want to randomly generate a new one with below paramters.")]
    private TextAsset levelSave;
    private int[,] loadedTiles;

    [SerializeField, Tooltip("How big each tile is."), Range(0.1f, 10)]
    public float scale = 1;

    [SerializeField, Tooltip("How much gap is between each tile."), Range(1, 1.25f)]
    public float gapScale = 1.05f;

    [SerializeField, Tooltip("The number of tiles in the grid."), Range(1,50)]
    public int width = 10, height = 10;

    //temporarily here for testing & creating unit functionality
    [SerializeField]
    private GameObject Humanunit;
    [SerializeField]
    private GameObject Alienunit;

    [SerializeField]
    private bool testUnits;

    [SerializeField]
    private int testSoldierAmount;
    [SerializeField]
    private int testAlienAmount;

    GameObject gridParent;


    private void Awake() {
        if (levelSave != null) {
            string[] rawLines = levelSave.text.Split('\n');
            string[] rawParamters = rawLines[0].Split(' ');
            scale = float.Parse(rawParamters[0]);
            gapScale = float.Parse(rawParamters[1]);
            width = int.Parse(rawParamters[2]);
            height = int.Parse(rawParamters[3]);
            loadedTiles = new int[width, height];
            for (int i = 0; i < rawLines.Length - 2; i++) {
                string[] rawValues = rawLines[i + 1].Split(' ');
                for (int j = 0; j < rawValues.Length; j++) {
                    loadedTiles[i,j] = int.Parse(rawValues[j]);
                }
                
            }
        }
        gridParent = new GameObject("Grid");
    }
    

    public void GenerateGrid() {
        gameManager.SetGridSize(width, height);

        Vector3 position = new Vector3(0, 0, 0);

        float xOffset = (width * scale * gapScale) / 2;
        float zOffset = (height * scale * gapScale) / 2;

        //Hexagon generation adapted from https://catlikecoding.com/unity/tutorials/hex-map/part-1/
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                position.x = ((x + z * 0.5f - z / 2) * scale * gapScale) - xOffset;
                position.z = (z * 0.866f * scale * gapScale) - zOffset;
                GameObject gridTile = Instantiate(tilePrefab, gridParent.transform);
                gridTile.transform.localPosition = position;
                gridTile.transform.localScale = new Vector3(scale, scale, scale);

                Tile tileScript = gridTile.AddComponent<Tile>();
                gameManager.tiles[x,z] = tileScript;

                if (loadedTiles != null) {
                    tileScript.terrainType = terrainTypes[loadedTiles[x, z]];
                } else {
                    tileScript.terrainType = terrainTypes[Random.Range(0, terrainTypes.Count)];
                }
                tileScript.SetTerrain();

                tileScript.coords = new Vector2(x, z);

            }
        }
        foreach (Tile tile in gameManager.tiles) {
            int x = (int)tile.coords.x;
            int z = (int)tile.coords.y;
            Vector2 index = new Vector2(x, z);

            if (x > 0) {
                AddAdjacentTiles(tile, gameManager.tiles[x - 1, z]);
            }
            if (z > 0) {
                AddAdjacentTiles(tile, gameManager.tiles[x, z - 1]);
            }
            if (z % 2 == 0) {
                if (z > 0 && x > 0) {
                    AddAdjacentTiles(tile, gameManager.tiles[x - 1, z - 1]);
                }
            } else {
                if (z > 0 && x < width - 1) {
                    AddAdjacentTiles(tile, gameManager.tiles[x + 1, z - 1]);
                }
            }
        }
        Camera.main.GetComponent<CameraMovement>().SetInitialPosition(scale);
        //here for testing unit movement

        if (testUnits)
        {
            for (int p = 0; p < testSoldierAmount; p++) //multiple test units done by Dylan
            {
                GameObject FriendObj = Instantiate(Humanunit);
                Humanunit.name = ("Soldier" + p).ToString();
                FriendObj.GetComponent<Unit>().UnitSpawn(gameManager.tiles[0, p]);
                gameManager.players[PlayerTeam.HUMAN].units.Add(FriendObj.GetComponent<Unit>());

            }
            for (int e = 0; e < testAlienAmount; e++) {
                GameObject EnemyObj = Instantiate(Alienunit);
                Alienunit.name = ("Alien" + e).ToString();
                EnemyObj.GetComponent<Unit>().UnitSpawn(gameManager.tiles[width - 1, e]);
                gameManager.players[PlayerTeam.ALIEN].units.Add(EnemyObj.GetComponent<Unit>());

            }
        }
    }

    private void AddAdjacentTiles(Tile tile1, Tile tile2) {
        tile1.adjacentTiles.Add(tile2);
        tile2.adjacentTiles.Add(tile1);
    }
}
