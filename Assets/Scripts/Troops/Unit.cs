using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Tooltip("The name this unit displays in UI.")]
    public string displayName;

    [Tooltip("The colour adjacent tiles are set to when the unit is moving.")]
    public Color[] moveableCol;
    public Color CurrentMoveableCol;

    public bool canBuild = true;
    [Tooltip("Every building this unit can create (if applicable)")]
    public List<Building> createableBuildings;

    public Tile currentTile;

    protected float scale;

    //[SerializeField] 
    //public enum Teams { Team1, Team2 }; replaced with PlayerTeam, from GameStats.cs

    [Header("Troop Settings")]
    public PlayerTeam team;
    public int MaxMovement;
    public int MaxHealth;
    public int Damage;
    public int AttackRange;
    public int MaxAttack;
    public bool AOEAttack;
    public bool isFlying;

    [SerializeField]
    private AnimatorEventTrigger animateTrigger;

    [SerializeField]
    private ParticleSystem lowHealthParticles;

    [HideInInspector] public int Health;
    [HideInInspector] public int CurrentMove;
    [HideInInspector] public int CurrentAttacks;
    [HideInInspector] public Vector3 unitScale;
    [HideInInspector] public HealthBar healthBar;
    [HideInInspector] public TextMeshPro valuesText;
    [HideInInspector] public List<Unit> enemiesInSight = new List<Unit>();
    [HideInInspector] public List<Building> buildingsInSight = new List<Building>();
    private List<Tile> tilesTargetted = new List<Tile>();
    private Tile tileHighlighted;
    private Unit enemyUnit;
    private Building enemyBuilding;

    private Animator animator;

    private Transform model;

    public void Start()
    {
        CurrentAttacks = MaxAttack;
        CurrentMoveableCol = moveableCol[0]; //sets up the moveable material
        CurrentMove = MaxMovement;
        Health = MaxHealth;
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.DisplaySpecified(MaxHealth, MaxHealth, team);
        //healthBar.gameObject.SetActive(false);
        valuesText = GetComponentInChildren<TextMeshPro>();
        model = transform.Find("Model");
        animator = model.GetComponent<Animator>();
    }
    //Movement///////////////////////////////////////////// Base Movement done by Nate, Limiting Movement Distance and changing movement material Done By Dylan
    public void UnitSpawn(Tile tile)
    {
        tile.unitHere = this;

        GameManager.Instance.players[team].units.Add(this);

        scale = tile.transform.localScale.x * 0.5f;
        unitScale = new Vector3(scale, scale, scale);
        transform.localScale = unitScale;

        MoveToTile(tile);
    }
    public void MoveToTile(Tile tile, bool animate = false)
    {
        currentTile = tile;
        if (animate) {
            StopAllCoroutines();
            StartCoroutine(AnimateToTile());
        } else {
            Vector3 position = currentTile.transform.position;
            position.y += scale * 0.5f;
            transform.position = position;
        }
        if (CurrentMove == 0)
        {
            CurrentMoveableCol = moveableCol[1]; //changes material to the NotMoveable
        }
    }

    private IEnumerator RotateToTarget(Vector3 targetPos) {
        float angle = model.eulerAngles.y;
        model.LookAt(targetPos);
        float endRot = model.eulerAngles.y;
        float currentVelocity = 0;
        while (Mathf.Abs(angle - endRot) > 0.1f) {
            angle = Mathf.SmoothDampAngle(angle, endRot, ref currentVelocity, 0.25f);
            model.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }
    }

    private IEnumerator AnimateToTile() {
        animator.SetBool("Walking", true);
        Vector3 startPosition = transform.position;
        Vector3 targetPos = currentTile.transform.position;
        targetPos.y += scale * 0.5f;
        StartCoroutine(RotateToTarget(targetPos));
        float time = 0;
        while (time < 1) {
            Vector3 pos = Vector3.Lerp(startPosition, targetPos, time);
            transform.position = pos;
            time += Time.deltaTime * 0.5f;
            yield return null;
        }
        animator.SetBool("Walking", false);
    }

    public void BeginMove()
    {
        EndTargeting();
        tilesTargetted = currentTile.GetAdjacentGroup(1);
        foreach (Tile adjacentTile in tilesTargetted)
        {

            if (adjacentTile.terrainType.walkable == true || isFlying)
            {
                adjacentTile.DisplayColour(CurrentMoveableCol);
            }
        }
        
    }
    public void EndMove(Tile targetTile)
    {
        

        if (CurrentMove >= targetTile.terrainType.travelSpeed && tilesTargetted.Contains(targetTile))
        {
            bool moved = false;
            if (!targetTile.unitHere) {
                targetTile.unitHere = this;
                moved = true;
            }

            if (targetTile.buildingHere) {
                if (targetTile.buildingHere.team == team) {
                    targetTile.buildingHere.OnEnterBehaviour(this);
                    moved = true;
                }
                else {
                    moved = false;
                }
            }

            if (moved) {
                CurrentMove -= targetTile.terrainType.travelSpeed;
                currentTile.unitHere = null;              
                Debug.Log(CurrentMove);
                MoveToTile(targetTile, true);
                EndTargeting();
            }
        }
    }

    public void ResetMove()
    {
        CurrentMove = MaxMovement;
    }
    //End Of Movement////////////////////////////////////////

    //Health///////////////////////////////////////////////// Done By Dylan

    public void TakeDamage(int damageDealt)
    {
        Health -= damageDealt;
        healthBar.Damage(damageDealt);
        if (Health <= 1 && lowHealthParticles != null && !lowHealthParticles.isPlaying) {
            lowHealthParticles.Play();
        }
        if (Health <= 0)
        {
            animator.SetTrigger("Defeated");
            healthBar.gameObject.SetActive(false);
            if (animateTrigger != null) {
                animateTrigger.AnimEvent += DestroySelf;
            } else {
                DestroySelf();
            }
        } else {
            animator.SetTrigger("Damaged");
        }

    }
    //End of Health//////////////////////////////////////////

    private void DestroySelf() {
        if (animateTrigger != null) {
            animateTrigger.AnimEvent -= DestroySelf;
        }
        Destroy(this.gameObject);
    }

    public void Heal(int healingDealt) {
        int trueHeal = Mathf.Min(healingDealt, MaxHealth - Health);
        Health += trueHeal;
        healthBar.Heal(trueHeal);
    }

    //Damage and Targeting/////////////////////////////////// Done By Dylan & Nate

    public void MarkAdjacentTiles(Tile tileToCheck, int maxLoops, bool dmgIndicate = false)
    {
        EndTargeting(); //Hopefully doesn't cause issues, but if multiple things are targetted at once it will (Shouldn't happen but might).
        tilesTargetted = tileToCheck.GetAdjacentGroup(maxLoops);
        foreach (Tile tile in tilesTargetted) {
            tile.DisplayColour(CurrentMoveableCol);
            if (dmgIndicate) {
                if (tile.unitHere && tile.unitHere.team != team) {
                    tile.unitHere.healthBar.IndicateDamage(Damage);
                    enemiesInSight.Add(tile.unitHere);
                }
                if (tile.buildingHere && tile.buildingHere.team != team) {
                    tile.buildingHere.IndicateHealth(Damage);
                    buildingsInSight.Add(tile.buildingHere);
                }
            }
        }
    }

    public void EndTargeting() {
        
        foreach (Tile tile in tilesTargetted) {
            tile.ResetMaterial();
        }
        tilesTargetted.Clear();
        foreach (Unit unit in enemiesInSight) {
            unit.healthBar.StopIndicating();
        }
        enemiesInSight.Clear();
        foreach (Building building in buildingsInSight) {
            building.StopIndicateHealth();
        }
        buildingsInSight.Clear();
    }

    // Currently unused code for highlighting the currently hovered tile, with AOE attacks

    //public void HighlightTile(Tile tile) {
    //    if (tile == tileHighlighted) {
    //        return;
    //    }
    //    if (tileHighlighted != null) {
    //        UnHighlightTiles();
    //    }
    //    if (!tilesTargetted.Contains(tile)) {
    //        return;
    //    }
    //    tileHighlighted = tile;
    //    foreach (Tile tileToHighlight in tile.GetAdjacentGroup(AOEAttack ? 2 : 1)) {
    //        tileToHighlight.DisplayColour(moveableCol[1]);
    //    }
    //}

    //public void UnHighlightTiles() {
    //    float startTime = 0;
    //    foreach (Tile markedTile in tilesTargetted) {
    //        if (!markedTile.highlighted) {
    //            startTime = markedTile.lerpTime;
    //            break;
    //        }
    //    }
    //    foreach (Tile oldTile in tileHighlighted.GetAdjacentGroup(AOEAttack ? 2 : 1)) {
    //        oldTile.lerpTime = startTime;
    //        oldTile.DisplayColour(CurrentMoveableCol);
    //    }
    //    tileHighlighted = null;
    //}

    //End Of Damage and Targeting///////////////////////////// Done By Dylan

    public void Attack(Vector3 attackPos) {
        EndTargeting();
        attackPos.y += scale * 0.5f;
        StopCoroutine("RotateToTarget");
        StartCoroutine(RotateToTarget(attackPos));
        animator.SetTrigger("Attacking");
        CurrentAttacks--;
    }

    public void AttackUnit(Tile enemyTile){
        enemyUnit = enemyTile.unitHere;
        Attack(enemyTile.transform.position);
        if (animateTrigger != null) {
            animateTrigger.AnimEvent += DamageEnemy;
        } else {
            DamageEnemy();
        }
    }

    public void AttackBuilding(Tile enemyTile) {
        enemyBuilding = enemyTile.buildingHere;
        Attack(enemyTile.transform.position);
        if (animateTrigger != null) {
            animateTrigger.AnimEvent += DamageBuilding;
        } else {
            DamageBuilding();
        }
    }

    public void DamageEnemy() {
        if (animateTrigger != null) {
            animateTrigger.AnimEvent -= DamageEnemy;
        }
        enemyUnit.TakeDamage(Damage);
    }

    public void DamageBuilding() {
        if (animateTrigger != null) {
            animateTrigger.AnimEvent -= DamageBuilding;
        }
        enemyBuilding.TakeDamage(Damage);
    }

    //Creating buildings////////////////////////////////////// Done by Nate

    public void ShowBuildMenu() {
        if (canBuild && currentTile.buildingHere == null) {
            //GameManager.Instance.gameUI.ShowBuyMenu(this);  //COMMENTED OUT
        }
    }

    public bool CreateBuilding(Building building) {
        if (GameManager.Instance.UseMaterial(team, building.price)) {
            currentTile.CreateBuilding(building);
            currentTile.buildingHere.OnEnterBehaviour(this);
            return true;
        }
        return false;
    }

    //End of creating buildings//////////////////////////////
    public void ResetUnit()
    {
        CurrentMove = MaxMovement;
        CurrentAttacks = MaxAttack;
        CurrentMoveableCol = moveableCol[0];
    }

    private void OnDestroy() {
        GameManager.Instance.players[team].units.Remove(this);
    }
}
