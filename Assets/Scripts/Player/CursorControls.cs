using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CursorControls : MonoBehaviour {

    //More efficient ways of doing this can be made later
    protected virtual void Update() {
        if (Input.GetMouseButtonDown(1)) {
            RightClickBehaviour();
        }
        Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit cursorHit;
        if (Physics.Raycast(cursorRay, out cursorHit)) {
            switch (cursorHit.collider.tag) {
                case "Tile":
                    //TileHoverBehaviour(cursorHit.collider.GetComponentInParent<Tile>());
                    break;
            }
            if (Input.GetMouseButtonDown(0)) {
                switch (cursorHit.collider.tag) {
                    case "Unit":
                        UnitClickBehaviour(cursorHit.collider.GetComponentInParent<Unit>());
                        break;
                    case "Tile":
                        TileClickBehaviour(cursorHit.collider.GetComponentInParent<Tile>());
                        break;
                    case "Building":
                        BuildingClickBehaviour(cursorHit.collider.GetComponent<Building>());
                        break;
                }
            }
        }
    }

    public virtual void UnitClickBehaviour(Unit unit) { }

    protected virtual void TileClickBehaviour(Tile tile) { }

    protected virtual void BuildingClickBehaviour(Building building) { }

    protected virtual void TileHoverBehaviour(Tile tile) { }

    protected virtual void RightClickBehaviour() { }
}
