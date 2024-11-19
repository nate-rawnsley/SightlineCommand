using System.Collections.Generic;
using UnityEngine;

public class UnitScout : Unit {
    private void Awake()
    {
        MaxMove = 4;
        Debug.Log(MaxMove);
    }
}
