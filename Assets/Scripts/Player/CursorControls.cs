using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CursorControls : MonoBehaviour {

    //More efficient ways of doing this can be made later
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit cursorHit;
            if (Physics.Raycast(cursorRay, out cursorHit)) {
                switch(cursorHit.collider.tag) {
                    case "Unit":
                        UnitClickBehaviour(cursorHit.collider.GetComponent<Unit>());
                        break;
                    case "Tile":
                        TileClickBehaviour(cursorHit.collider.GetComponent<Tile>());
                        break;
                    case "Building":
                        BuildingClickBehaviour(cursorHit.collider.GetComponent<Building>());
                        break;
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RightClickBehaviour();
        }
    }

    public virtual void UnitClickBehaviour(Unit unit) { }

    protected virtual void TileClickBehaviour(Tile tile) { }

    protected virtual void BuildingClickBehaviour(Building building) { }

    protected virtual void RightClickBehaviour() { }
}
