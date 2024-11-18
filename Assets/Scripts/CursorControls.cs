using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControls : MonoBehaviour {
    private Unit activeUnit;

    //More efficient ways of doing this can be made later
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Ray cast");
            Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit cursorHit;
            if (Physics.Raycast(cursorRay, out cursorHit)) {
                Debug.Log(cursorHit.collider);
                switch(cursorHit.collider.tag) {
                    case "Unit":
                        activeUnit = cursorHit.collider.GetComponent<Unit>();
                        activeUnit.BeginMove();
                        break;
                    case "Tile":
                        if (activeUnit != null) {
                            activeUnit.EndMove(cursorHit.collider.GetComponent<Tile>());
                            activeUnit = null;
                        }
                        break;
                }
            }
        }
    }
}
