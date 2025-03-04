using TMPro;
using UnityEngine;

public class GameCursor : CursorControls {
    [SerializeField]
    private Unit activeUnit = null;
    private Unit EnemyUnit;

    public PlayerTeam CurrentTeam;

    private TextMeshPro Values;
    //modes
    public enum UnitMode { None, Attack, Move, Build, End}
    public UnitMode currentMode = UnitMode.None;

    [SerializeField]
    private BuildingPanel buildingPanel;

    [SerializeField]
    private GameManager gameStats;

    public override void UnitClickBehaviour(Unit unit) {
        if (buildingPanel.gameObject.activeSelf) {
            buildingPanel.HidePanel();
        }
        if (activeUnit == null && currentMode != UnitMode.None && unit.Health != 0) {
            activeUnit = unit;
            //HasSelection = false  -- N - This was replaced by checking activeUnit == null instead
            Values = unit.valuesText;

            if (activeUnit.team == CurrentTeam)
            {
                switch (currentMode)
                {
                    case UnitMode.Attack:
                        if (unit.CurrentAttacks > 0)
                        {
                            Values.text = unit.CurrentAttacks.ToString();
                            unit.CurrentMoveableCol = unit.moveableCol[0];
                            unit.MarkAdjacentTiles(unit.currentTile, unit.AttackRange, true);
                        }
                        else
                        {
                            CLEARALL();
                        }
                        break;

                    case UnitMode.Move:
                        Debug.Log("click");
                        Values.text = unit.CurrentMove.ToString();
                        if (activeUnit.CurrentMove > 0)
                        {
                            unit.BeginMove();
                            Debug.Log("DID MOVE");
                        }
                        else
                        {
                            CLEARALL();

                        }
                        break;

                    case UnitMode.Build:
                        if (activeUnit.currentTile.buildingHere == null)
                        {
                            activeUnit.CreateBuilding(0);

                        }
                        CLEARALL();
                        break;
                }
            }
            else
            {
                CLEARALL();
            }
        } else if (currentMode == UnitMode.Attack) {
            //If a unit is clicked in attack mode, deal damage to it as if its tile was clicked on instead.
            TileClickBehaviour(unit.currentTile);
        }
    }

    protected override void TileClickBehaviour(Tile tile) {
        if (buildingPanel.gameObject.activeSelf) {
            buildingPanel.HidePanel();
        }
        bool acted = false;
        if (activeUnit != null) {

            switch (currentMode) {
                case UnitMode.Attack:
                    if (tile.unitHere && activeUnit.enemiesInSight.Contains(tile.unitHere)) {
                        activeUnit.Attack(tile.transform.position);
                        doDamage(tile);
                        acted = true;
                    } else if (tile.buildingHere && activeUnit.buildingsInSight.Contains(tile.buildingHere)) {
                        activeUnit.Attack(tile.transform.position);
                        DamageBuilding(tile.buildingHere);
                        acted = true;
                    }
                    break;

                case UnitMode.Move:
                    if (tile.unitHere) {
                        acted = true;
                        break;
                    }
                    if (tile.terrainType.walkable)
                    {
                        activeUnit.EndMove(tile); //Clears all highlighted tiles
                        acted = true;
                    }
                    break;

            }
            if (acted) {

                CLEARALL();
            }
        }

    }

    protected override void BuildingClickBehaviour(Building building) {
        if (currentMode == UnitMode.Attack && activeUnit && activeUnit.team != building.team) {
            activeUnit.Attack(building.tile.transform.position);
            DamageBuilding(building);
            CLEARALL();
            return;
        }
        if (buildingPanel.gameObject.activeSelf) {
            buildingPanel.HidePanel();
        }
        if (activeUnit == null) {
            buildingPanel.SetBuilding(building);
        } else {
            TileClickBehaviour(building.tile);
        }
        
    }

    protected override void RightClickBehaviour()
    {
        CLEARALL();  //
    }

    protected void doDamage(Tile tile) {
        if (tile.unitHere) {            
            EnemyUnit = tile.unitHere;
            EnemyUnit.TakeDamage(activeUnit.Damage);
            activeUnit.CurrentAttacks--;
            Debug.Log("DONEDAMAGE2");
        }
    }

    protected void DamageBuilding(Building building) {
        if (building) {
            building.TakeDamage(activeUnit.Damage);
            activeUnit.CurrentAttacks--;
        }
    }

    //Instead of separate functions for each button, it parses an index and uses the same one.
    public void SetBehaviour(int modeIndex) {
        UnitMode unitMode = (UnitMode)modeIndex;
        if (currentMode == unitMode) {
            currentMode = UnitMode.None;
        } else {
            currentMode = unitMode;
        }
        CLEARALL();
    }

    public void EndTurn()
    {
        CLEARALL();
        CurrentTeam = CurrentTeam == PlayerTeam.HUMAN ? PlayerTeam.ALIEN : PlayerTeam.HUMAN;
        gameStats.NewTurn(CurrentTeam);
    }

    public void CLEARALL()
    {
        if (activeUnit != null) {
            activeUnit.EndTargeting();
            activeUnit = null;       //Clears all selections                                       
            Values.text = "";
            Values = null;
        }
    }
}