using Leap;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class HandCasting : MonoBehaviour
{ //Code made by Nate/Edited into Hand Tracking System By Dylan
    public GameObject Fingertip;
    private Vector3 OriginFinger;

    public bool Check;
    public bool active;
    
    private void Update()
    {
        if (!active)
        {
            return;
        }
        Vector3 OriginFinger = Fingertip.transform.position;
        RaycastHit FingerHit;
        Physics.Raycast(OriginFinger, Fingertip.transform.right, out FingerHit, 500f);
        if (Check == true || Input.GetKeyDown(KeyCode.Space))
        {
            
            Debug.Log(FingerHit.collider.tag);
            switch (FingerHit.collider.tag)
            {
                case "Unit":
                    UnitClickBehaviour(FingerHit.collider.GetComponentInParent<Unit>());
                    break;
                case "Tile":
                    TileClickBehaviour(FingerHit.collider.GetComponentInParent<Tile>());
                    break;
                case "Building":
                    BuildingClickBehaviour(FingerHit.collider.GetComponent<Building>());
                    break;
            }
        }
        }
    public void Selections()
    {
        Check = true;
        Debug.Log("True");
    }
    public void SelectEnd()
    {
        Check = false;
        Debug.Log("False");
    }
    public virtual void UnitClickBehaviour(Unit unit) { }

    protected virtual void TileClickBehaviour(Tile tile) { }

    protected virtual void BuildingClickBehaviour(Building building) { }

    protected virtual void TileHoverBehaviour(Tile tile) { }

    protected virtual void RightClickBehaviour() { }
}
