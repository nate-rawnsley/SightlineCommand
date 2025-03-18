using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Building : MonoBehaviour {
    [Header("Values")]
    public float price = 10;
    public int capacity = 5;
    public int maxHealth = 5;
    public PlayerTeam team;

    [Header("Panel Text")]
    public string buildingName;
    public string toolTip;
    public string command;

    [Header("Units sold here (if applicable)"), SerializeField]
    public List<UnitShopValue> availableUnits = new List<UnitShopValue>();

    [HideInInspector] public List<Unit> unitsHere;
    [HideInInspector] public Tile tile;

    [HideInInspector] public GameObject unitInCreation;
    [HideInInspector] public int turnsToCreate;
    [HideInInspector] public int health;
    private HealthBar healthBar;
    private SpriteRenderer unitIndicator;

    private void Awake() {
        health = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.DisplaySpecified(maxHealth, maxHealth, team, true);
        healthBar.gameObject.SetActive(false);
        unitIndicator = GetComponentInChildren<SpriteRenderer>();
    }

    //Returns whether to hide the main buiding menu after.
    public virtual bool ActivateBehaviour() { return false; }

    public virtual void DeactivateBehaviour() { }

    protected virtual void DeathBehaviour() {
        List<Unit> unitsToRemove = new List<Unit>(unitsHere); //Made temporary list to avoid errors
        foreach (var unit in unitsToRemove) {
            if (unit != null) {
                OnExitBehaviour(unit); //All units exit when this is destroyed.
            }
        }
        GameManager.instance.players[team].buildings.Remove(this);
        Destroy(gameObject);
    }

    //When a unit enters the tile this building is on, add it to the list, hide it and display a chevron.
    public virtual void OnEnterBehaviour(Unit unitEntered) {
        unitsHere.Add(unitEntered);
        tile.unitHere = null;
        unitEntered.transform.localScale = Vector3.zero;
        Sprite indicatorSprite = Resources.Load<Sprite>("Billboards/Chevron1");
        unitIndicator.sprite = indicatorSprite;
    }

    public virtual void OnExitBehaviour(Unit unitLeaving) {
        Tile exitTile = tile;
        if (tile.unitHere != null) {
            exitTile = tile.FindEmptyTile();
            unitLeaving.MoveToTile(exitTile);
        }
        unitsHere.Remove(unitLeaving);
        exitTile.unitHere = unitLeaving;
        unitLeaving.transform.localScale = unitLeaving.unitScale;
        if (unitsHere.Count == 0) {
            unitIndicator.sprite = null;
        }
    }

    public bool BuyUnit(UnitShopValue unitVals) {
        if (unitInCreation == null && GameManager.instance.UseTokens(team, unitVals.price)) {
            if (GameManager.instance.players[team].troopTokens >= unitVals.price) {
                unitInCreation = unitVals.unitPrefab;
                turnsToCreate = unitVals.createSpeed;
                return true;
            }
        }
        return false;
    }

    public void NewTurn() {
        if (unitInCreation != null) {
            Debug.Log(turnsToCreate);
            turnsToCreate--;
            if (turnsToCreate <= 0) {
                GameObject unitSpawn = Instantiate(unitInCreation);
                unitSpawn.GetComponent<Unit>().UnitSpawn(tile);
                unitInCreation = null;
            }
        }
    }

    public void IndicateHealth(int damage) {
        healthBar.gameObject.SetActive(true);
        healthBar.IndicateDamage(damage);
    }

    public void StopIndicateHealth() {
        healthBar.gameObject.SetActive(false);
        healthBar.StopIndicating();
    }

    public void TakeDamage(int damage) {
        health -= damage;
        StartCoroutine(DelayHealthBarDisplay(damage, false));
        if (health <= 0) {
            DeathBehaviour();
        }
    }

    public void RepairDamage(int heal) {
        int trueHeal = Mathf.Min(heal, maxHealth - health);
        health += trueHeal;
        StartCoroutine(DelayHealthBarDisplay(trueHeal, true));
    }

    private IEnumerator DelayHealthBarDisplay(int value, bool heal) {
        yield return new WaitForEndOfFrame(); //Delays display as it sometimes gets disabled multiple times
        healthBar.gameObject.SetActive(true);
        if (heal) {
            healthBar.Heal(value);
        } else {
            healthBar.Damage(value);
        }
        while (healthBar.active) {
            yield return null;
        }
        healthBar.gameObject.SetActive(false);
    }
}
