using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour {
    public float price = 10;
    public int capacity = 5;
    public List<GameObject> unitsHere;
    private GameObject unitIndicator;
    public Tile tile;

    public virtual void ActivateBehaviour() { }

    public virtual void DeactivateBehaviour() { }

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
}
