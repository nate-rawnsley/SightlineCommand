using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Nate
/// Defines building behaviour, which can be created by a Unit (or a level save)
/// </summary>
public class Building : MonoBehaviour {
    [Header("Values")]
    public BuildingCostTree price;
    public int capacity = 5;
    public int maxHealth = 5;
    public PlayerTeam team;
    public int materialYield;

    [Header("Panel Text")]
    public string buildingName;
    public string toolTip;
    public string command;

    [HideInInspector] public bool canActivate;

    [HideInInspector] public List<Unit> unitsHere;
    [HideInInspector] public Tile tile;

    [HideInInspector] public GameObject unitInCreation;
    [HideInInspector] public int turnsToCreate;
    [HideInInspector] public int health;
    private HealthBar healthBar;
    private SpriteRenderer unitIndicator;
    private TextMeshProUGUI createIndicator;

    private void Awake() {
        health = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.DisplaySpecified(maxHealth, maxHealth, team, true);
        healthBar.gameObject.SetActive(false);
        unitIndicator = GetComponentInChildren<SpriteRenderer>();
        createIndicator = transform.Find("Canvas/Creation Indicator").GetComponent<TextMeshProUGUI>();
        createIndicator.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        GameManager.Instance.players[team].buildings.Remove(this);
    }

    //Returns whether to hide the main buiding menu after.
    public virtual bool ActivateBehaviour() { return false; }

    public virtual void SpawnBehaviour() {
        if (price != null) {
            price.numberActive++;
        }
    }

    protected virtual void DeathBehaviour() {
        if (price != null) {
            price.numberActive--;
        }
        List<Unit> unitsToRemove = new List<Unit>(unitsHere); //Made temporary list to avoid errors
        foreach (var unit in unitsToRemove) {
            if (unit != null) {
                OnExitBehaviour(unit); //All units exit when this is destroyed.
            }
        }
        Destroy(gameObject);
    }

    //When a unit enters the tile this building is on, add it to the list, hide it and display a chevron.
    public virtual void OnEnterBehaviour(Unit unitEntered) {
        unitsHere.Add(unitEntered);
        tile.unitHere = null;
        unitEntered.transform.localScale = Vector3.zero;
        string chevronString = "Billboards/Chevron3";
        if (unitsHere.Count < 3) {
            chevronString = $"Billboards/Chevron{unitsHere.Count}";
        }
        Sprite indicatorSprite = Resources.Load<Sprite>(chevronString);
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
        } else {
            string chevronString = "Billboards/Chevron3";
            if (unitsHere.Count < 3) {
                chevronString = $"Billboards/Chevron{unitsHere.Count}";
            }
            Sprite indicatorSprite = Resources.Load<Sprite>(chevronString);
            unitIndicator.sprite = indicatorSprite;
        }
    }

    public bool BuyUnit(Unit newUnit) {
        if (unitInCreation == null && GameManager.Instance.UseTokens(team, newUnit.tokenCost)) {
            unitInCreation = newUnit.gameObject;
            turnsToCreate = newUnit.turnsToCreate;
            createIndicator.gameObject.SetActive(true);
            createIndicator.text = $"Unit Creating: {turnsToCreate} turn(s)";
            return true;
        }
        return false;
    }

    public void EndTurn() { 
        GameManager.Instance.players[team].material += materialYield;
        Debug.Log(GameManager.Instance.players[team].material);
        //GameManager.Instance.players[team].troopTokens++;
    }

    public void NewTurn() {
        if (unitInCreation != null) {
            turnsToCreate--;
            if (turnsToCreate <= 0) {
                GameObject unitSpawn = Instantiate(unitInCreation);
                unitSpawn.GetComponent<Unit>().UnitSpawn(tile);
                unitInCreation = null;
                createIndicator.gameObject.SetActive(false);
            } else {
                createIndicator.text = $"Unit Creating: {turnsToCreate} turn(s)";
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
