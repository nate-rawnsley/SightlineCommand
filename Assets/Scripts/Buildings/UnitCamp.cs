using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCamp : Building {
    [SerializeField] private GameObject buyMenu;
    private GameObject canvas;
    private GameObject menuInstance;

    public override bool ActivateBehaviour() {
        if (menuInstance == null) {
            canvas = GameObject.FindGameObjectWithTag("MainCanvas");
            menuInstance = Instantiate(buyMenu, canvas.transform);
            menuInstance.GetComponent<BuyMenu>().Initialize(this);
        } else {
            menuInstance.SetActive(true);
        }
        return true;
    }
}
