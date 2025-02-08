using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour {
    public List<GameObject> unitsHere;
    private GameObject unitIndicator;
    public Tile tile;
    public PlayerTeam team;
    public GameObject unitInCreation;
    public int turnsToCreate;

    [Header("Values")]
    public float price = 10;
    public int capacity = 5;

    [Header("Panel Text")]
    public string buildingName;
    public string toolTip;
    public string command;

    [Header("Units sold here (if applicable)"), SerializeField]
    public List<UnitShopValue> availableUnits = new List<UnitShopValue>();

    //Returns whether to hide the main buiding menu after.
    public virtual bool ActivateBehaviour() { return false; }

    public virtual void DeactivateBehaviour() { }

    //When a unit enters the tile this building is on, add it to the list, hide it and display a chevron.
    public virtual void OnEnterBehaviour(Unit unitEntered) {
        unitsHere.Add(unitEntered.gameObject);
        unitEntered.transform.localScale = Vector3.zero;
        if (unitIndicator == null) {
            GameObject indicatorObj = Resources.Load<GameObject>("Billboards/Chevron1");
            unitIndicator = Instantiate(indicatorObj, transform);
        }
    }

    public virtual void OnExitBehaviour(Unit unitLeaving) {
        unitsHere.Remove(unitLeaving.gameObject);
        unitLeaving.transform.localScale = unitLeaving.unitScale;
        if (unitsHere.Count == 0) {
            Destroy(unitIndicator);
            unitIndicator = null;
        }
    }

    public bool BuyUnit(UnitShopValue unitVals) {
        if (unitInCreation == null ) { //TODO && money > cost
            unitInCreation = unitVals.unitPrefab;
            turnsToCreate = unitVals.createSpeed;
            //TODO deduct money
            return true;
        }
        return false;
    }

    public void NewTurn() {
        if (unitInCreation != null) {
            turnsToCreate--;
            if (turnsToCreate <= 0) {
                GameObject unitSpawn = Instantiate(unitInCreation);
                unitSpawn.GetComponent<Unit>().UnitSpawn(tile);
                unitInCreation = null;
            }
        }
    }
}
