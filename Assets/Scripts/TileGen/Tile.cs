using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public TileTerrain terrainType;
    public List<Tile> adjacentTiles = new List<Tile>();

    public Unit unitHere;
    public bool IsSelected;
    public Building buildingHere;
    private GameObject decoration;

    private Renderer thisRenderer;
    private bool lerpingColour;
    private float lerpTime;

    public Vector2 coords;

    private void Awake() {
        thisRenderer = GetComponent<Renderer>();
    }

    public void OnDestroy() {
        if (decoration != null) { 
            Destroy(decoration);
        }
    }

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

                int alignment = Random.Range(0, 6);
                Vector3 decoRotation = decoration.transform.rotation.eulerAngles;
                decoRotation.y = alignment * 60;
                decoration.transform.rotation = Quaternion.Euler(decoRotation);
            }
        }
    }

    public void CreateBuilding(Building building) {
        if (buildingHere != null) {
            return;
        }

        if (decoration != null) {
            Destroy(decoration.gameObject);
            decoration = null;
        }

        buildingHere = Instantiate(building.gameObject, transform).GetComponent<Building>();
    }

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
        //will add a proper easing here later
        while (lerpingColour) {
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
}
