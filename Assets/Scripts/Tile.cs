using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public TileTerrain terrainType;
    public List<Tile> adjacentTiles = new List<Tile>();
    public Unit unitHere;
    private GameObject decoration;

    public void SetTerrain() {
        if (decoration != null) {
            Destroy(decoration);
            decoration = null;
        }
        GetComponent<Renderer>().material = terrainType.material;
        if (terrainType.decorations.Count > 0) {
            if (Random.value <= terrainType.decorationFrequency) {
                int index = Random.Range(0, terrainType.decorations.Count);
                decoration = Instantiate(terrainType.decorations[index], transform);

                int alignment = Random.Range(0, 4);
                Vector3 decoRotation = decoration.transform.rotation.eulerAngles;
                decoRotation.y = alignment * 90;
                decoration.transform.rotation = Quaternion.Euler(decoRotation);
            }
        }
    }
}
