using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCursor : CursorControls {
    public TileTerrain terrainBrush;

    protected override void TileClickBehaviour(Tile tile) {
        tile.terrainType = terrainBrush;
        tile.GetComponent<Renderer>().material = terrainBrush.material;
    }
}
