using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour {
    public float price = 10;
    public int capacity = 5;
    public List<GameObject> unitsHere;
    private GameObject unitIndicator;

    public virtual void ActivateBehaviour() { }

    public virtual void DeactivateBehaviour() { }

    public virtual void OnEnterBehaviour(Unit unitEntered) {
        unitsHere.Add(unitEntered.gameObject);
        unitEntered.transform.localScale = Vector3.zero;
        if (unitIndicator == null) {
            GameObject indicatorObj = Resources.Load<GameObject>("Billboards/Chevron1");
            Vector3 indicatorPos = transform.position;
            //TODO add reference to scale here
            indicatorPos.y += 10;
            unitIndicator = Instantiate(indicatorObj, indicatorPos, Quaternion.identity, transform);
        }
    }
}
