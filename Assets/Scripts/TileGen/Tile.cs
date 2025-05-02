using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Nate
/// A data class used for storing the lowest-cost route to reach a tile.
/// </summary>
public class PathTile {
    public Tile tile;
    public int minCost = 99;
    public List<Tile> path;

    public PathTile(Tile tile, int minCost, List<Tile> path) {
        this.tile = tile;
        this.minCost = minCost;
        this.path = path;
    }
}

/// <summary>
/// Nate
/// The behaviour and values of every tile in the grid.
/// Contains functions for setting data, as well as querying about other tiles.
/// In addition, contains the function for spawning buildings.
/// </summary>
public class Tile : MonoBehaviour {
    public TileTerrain terrainType;
    public List<Tile> adjacentTiles = new List<Tile>();

    public Unit unitHere;
    public bool IsSelected;
    public Building buildingHere;
    public GameObject decoration;

    //Allows listeners (currently defence building) to observe when a unit enters, parsing the unit as a parameter.
    public Action<Unit> UnitMovedHere;

    private Renderer thisRenderer;
    public bool lerpingColour;
    public bool highlighted;
    public float lerpTime;

    public Vector2 coords;

    [HideInInspector]
    public int decoIndex;
    [HideInInspector]
    public Vector3 decoRotation;

    private void Awake() {
        thisRenderer = transform.Find("TileMesh").GetComponent<Renderer>();
    }

    public void OnDestroy() {
        if (decoration != null) {
            Destroy(decoration);
        }
    }

    public void ClearTerrain() {
        if (decoration != null) {
            Destroy(decoration);
            decoration = null;
        }
        if (buildingHere != null) {
            Destroy(buildingHere.gameObject);
            buildingHere = null;
        }
        if (unitHere != null) {
            Destroy(unitHere.gameObject);
            unitHere = null;
        }
    }

    /// <summary>
    /// Configures the tile's values and decoration based on its new terrain type.
    /// Called after assigning a terrain type, by the grid generator and editor scripts.
    /// </summary>
    public void SetTerrain() {
        if (decoration != null) {
            Destroy(decoration);
            decoration = null;
        }
        thisRenderer.material = terrainType.material;
        bool hasDecoration = false;
        //The decoration's rotation and index are stored for saving in the level editor.
        if (terrainType.decorations.Count > 0 && buildingHere == null) {
            if (UnityEngine.Random.value <= terrainType.decorationFrequency) {
                decoIndex = UnityEngine.Random.Range(0, terrainType.decorations.Count);
                decoration = Instantiate(terrainType.decorations[decoIndex], transform);

                int alignment = UnityEngine.Random.Range(0, 6);
                decoRotation = decoration.transform.rotation.eulerAngles;
                decoRotation.y = alignment * 60;
                decoration.transform.rotation = Quaternion.Euler(decoRotation);
                hasDecoration = true;
            }
        }
        if (!hasDecoration) {
            decoIndex = -1;
        }
    }

    /// <summary>
    /// Configures the tile's values and decoration based on data loaded from a save.
    /// Replicates its state from the editor, including spawning decorations, buildings, and units.
    /// </summary>
    /// <param name="loadTile">The data from the corresponding tile in the level save.</param>
    public void LoadTile(TileData loadTile) {
        ClearTerrain();
        terrainType = loadTile.terrainType;
        thisRenderer.material = terrainType.material;
        if (loadTile.decoration != null) {
            decoration = Instantiate(loadTile.decoration, transform);
            decoration.transform.rotation = Quaternion.Euler(loadTile.decorationRotation);
        }
        if (loadTile.buildingHere != null) {
            CreateBuilding(loadTile.buildingHere.GetComponent<Building>());
        }
        if (loadTile.unitHere != null) {
            GameObject unitObj = Instantiate(loadTile.unitHere);
            unitObj.GetComponent<Unit>().UnitSpawn(this);
        }
    }

    /// <summary>
    /// Creates a building on top of this tile.
    /// Ensures the building is spawned properly and added to the player's data.
    /// </summary>
    /// <param name="building">The script on the prefab building to create.</param>
    public void CreateBuilding(Building building) {
        if (buildingHere != null || !terrainType.walkable) {
            return;
        }

        if (decoration != null) {
            Destroy(decoration.gameObject);
            decoration = null;
        }

        buildingHere = Instantiate(building.gameObject, transform).GetComponent<Building>();
        buildingHere.tile = this;
        buildingHere.SpawnBehaviour();
        GameManager.Instance.players[building.team].buildings.Add(buildingHere);
    }

    /// <summary>
    /// Highlights the tile in a selected colour.
    /// USed to indicate being in range of moving or attacking.
    /// </summary>
    /// <param name="color"></param>
    public void DisplayColour(Color color) {
        if (lerpingColour) {
            return;
        }
        lerpingColour = true;
        StartCoroutine(LerpColour(color));
    }

    public void ResetMaterial()
    {
        lerpingColour = false;
        lerpTime = 0;
        thisRenderer.material = terrainType.material;
    }

    private IEnumerator LerpColour(Color color) {
        while (lerpingColour) {
            //Linear easing for the lerping that reverses halfway through.
            float perc = lerpTime < 0.5f ? lerpTime * 1.75f : 1 - (lerpTime - 0.5f) * 1.75f;
            Color newColour = Color.Lerp(terrainType.material.color, color, perc);

            thisRenderer.material.color = newColour;

            lerpTime += Time.deltaTime;
            if (lerpTime >= 1) {
                lerpTime = 0;
            }
            yield return null;
        }
    }

    /// <summary>
    /// Finds a list of all adjacent tiles a unit can reach with their maximum movement.
    /// Since tiles have separate travel speeds (eg. sand is twice as slow as others), a weighted search is performed.
    /// </summary>
    /// <param name="maxMovement">The unit's MaxMovement stat. Provides the limit for the loop.</param>
    /// <param name="flying">The unit's isFlying stat. Allows them to ignore tile cost and walkability.</param>
    /// <returns>A list of all tiles in range, as well as the most efficient route to reach them (unfinished).</returns>
    public List<PathTile> GetWalkableGroup(int maxMovement, bool flying = false) {
        List<PathTile> tilePaths = new List<PathTile>(); 
        tilePaths = WeightedSearch(new PathTile(this, 0, new List<Tile>()), 0, maxMovement, tilePaths, flying);
        return tilePaths;
    }

    public List<PathTile> WeightedSearch(PathTile tileToCheck, int currentCost, int maxMovement, List<PathTile> pathList, bool flying) {

        foreach (Tile adjacentTile in tileToCheck.tile.adjacentTiles){
            if (!adjacentTile.terrainType.walkable && !flying) { //checks if the unit is a flying one to allow it to ignore terrain affordability and inaccessable tiles. (Dylan)
                continue;
            }
            int nextTravelSpeed = flying ? 1 : adjacentTile.terrainType.travelSpeed;
            int newCost = currentCost + nextTravelSpeed;

            PathTile adjacentPathTile = new PathTile(this, 0, new List<Tile>());

            bool inList = false;
            ///Recording the most efficient path to a tile is not properly implemented yet.
            ///For the sake of simpicity, the list simply contains the end destination.
            foreach (PathTile path in pathList) { 
                if (adjacentTile == path.tile) {
                    adjacentPathTile = path;
                    inList = true;
                    if (path.minCost > newCost) {
                        path.minCost = newCost;
                        //List<Tile> newPath = tileToCheck.path;
                        //newPath.Add(adjacentTile);
                        //path.path = newPath;
                    }
                    break;
                }
            }
            if (!inList && newCost <= maxMovement) {
                List<Tile> newPath = new List<Tile>(); //tileToCheck.path;
                newPath.Add(adjacentTile);
                adjacentPathTile = new PathTile(adjacentTile, currentCost, newPath);
                pathList.Add(adjacentPathTile);
            }
            if (newCost < maxMovement) {
                pathList = WeightedSearch(adjacentPathTile, newCost, maxMovement, pathList, flying);
            }
        }
        return pathList;
    }

    /// <summary>
    /// Performs a recursive search for adjacent tiles.
    /// Used for attacking and for defence buildings.
    /// </summary>
    /// <param name="maxLoops">Layers to search (1 = neighbours, 2 = neighbours of neighbours etc.)</param>
    /// <returns>A list of all relevant adjacent tiles.</returns>
    public List<Tile> GetAdjacentGroup(int maxLoops) {
        List<Tile> tileList = new List<Tile>();
        tileList = AdjacentSearch(this, 0, maxLoops, tileList);
        return tileList;
    }

    public List<Tile> AdjacentSearch(Tile tileToCheck, int loopNo, int maxLoops, List<Tile> tileList) {
        loopNo++;
        foreach (Tile adjacentTile in tileToCheck.adjacentTiles) {
            if (!tileList.Contains(adjacentTile)) { 
                tileList.Add(adjacentTile);
            }
            if (loopNo < maxLoops) {
                tileList = AdjacentSearch(adjacentTile, loopNo, maxLoops, tileList);
            }
        }
        return tileList;
    }

    /// <summary>
    /// Attempts to find tiles that do not have a unit in immediate neighbours.
    /// Used for units leaving buildings, to prevent stacking.
    /// </summary>
    /// <returns>An empty tile.</returns>
    public Tile FindEmptyTile() {
        if (!unitHere) {
            return this;
        } 
        foreach (Tile tile in adjacentTiles) {
            if (!tile.unitHere && tile.terrainType.walkable) {
                return tile;
            }
        }
        return this; //currently returns self as a fallback. might add additional check layer later
    }
}
