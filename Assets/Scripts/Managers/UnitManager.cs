//***************************************//
// Unit Manager
// La creation, les actions et la destructions des units (Hero & Enemy)
// Baser sur le choix de l'usager dans le ChangeHeroTeam.
// Fait Par: Vincent & Mathieu
//***************************************//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class UnitManager : MonoBehaviour {
    public static UnitManager Instance;

    private List<ScriptableUnit> _alliedUnits, _enemyUnits;
    public BaseUnit SelectedHero;
    public List<BaseUnit> listHero, listEnemy;
    public List<Vector2> positionHero, positionEnemy;
    public int nbHero, nbEnemy, actionTurn;

    private string herochoice, enemychoice;

    void Awake() { 
        Instance = this;
        int randomInt = Mathf.FloorToInt(Random.Range(0, 1));
        if(randomInt == 0)
        {
            enemychoice = "Enemies";
        }
        else if(randomInt == 1)
        {
            enemychoice = "Enemy1";
        }
        herochoice = ChangeHeroTeam.Instance.getHeroChoice();
        if(ChangeHeroTeam.Instance.getHeroChoice() == null)
        {
            herochoice = "Heroes";
        }
        _alliedUnits = Resources.LoadAll<ScriptableUnit>(herochoice).ToList();
        _enemyUnits = Resources.LoadAll<ScriptableUnit>(enemychoice).ToList();
    }

    public List<BaseUnit> getListHero() { return listHero;}
    public List<BaseUnit> getlistEnemy() { return listEnemy;}

    // Création des Units avec leur tuile de Départ + ajout à la listHero
    public void SpawnHeroes() {
        foreach(ScriptableUnit _unit in _alliedUnits.Where(_unit => _unit.Faction == Faction.Hero)) {
            var HeroSpawn = (BaseUnit)_unit.UnitPrefab;
            var spawnedHero = Instantiate(HeroSpawn);
            var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();

            listHero.Add(spawnedHero);
            randomSpawnTile.SetUnit(spawnedHero);
        }

        GameManager.Instance.ChangeState(GameState.SpawnEnemies);
    }

    // Création des Enemies avec leur tuile de Départ + ajout à la listeEnemy
    public void SpawnEnemies() {
        foreach(ScriptableUnit _unit in _enemyUnits.Where(_unit => _unit.Faction == Faction.Enemy)) {
            var EnemySpawn = (BaseUnit)_unit.UnitPrefab;
            var spawnedEnemy = Instantiate(EnemySpawn);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();

            listEnemy.Add(spawnedEnemy);
            randomSpawnTile.SetUnit(spawnedEnemy);
        }

        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }

    // La mort d'un unit
    public void UnitMeurt(BaseUnit unit){
        if (unit.Faction == Faction.Hero){
            listHero.Remove(unit);
            print("Remaining Hero " + listHero.Count());
        }
        else {
            listEnemy.Remove(unit);
            print("Remaining Enemy " + listEnemy.Count());
        }
        CheckEndGame();
    }

    // Verification de Fin de Partie
    public void CheckEndGame() {
        if (listHero.Count() == 0 ){
            print("Lose");
            MenuManager.Instance.ShowFinDePartie(false);
            // Lose
        }
        if (listEnemy.Count() == 0 ){
            print("Win");
            MenuManager.Instance.ShowFinDePartie(true);
            // Win
        }
    }

    // CountDown pour La fin du Tour Hero 
    public void ActionHeroes() {
        actionTurn = actionTurn+1;
        if (actionTurn >= listHero.Count()){
            actionTurn = 0;
            print("Enemies Turn!");
            GameManager.Instance.ChangeState(GameState.EnemiesTurn);
        }
    }

    // CountDown pour la fin du tour Enemy
    public void ActionEnemies() {
        actionTurn = actionTurn+1;
        if (actionTurn >= listEnemy.Count()){
            actionTurn = 0;
            print("Players Turn!");
            GameManager.Instance.ChangeState(GameState.PlayerTurn);
        }
    } 

    // Déroulement du Tour Enemy
    public void EnemiesTurn() {
        UpdatePositionEnemy();UpdatePositionHero();
        foreach (BaseUnit enemy in listEnemy) {
            print("AI : "+enemy.UnitType);
            if (enemy.getCurrentHP() > 0) {
                bool _action = true;
                AIScore.Instance.initDictTargets(); AIScore.Instance.initDictTiles();
                // Liste des attackRange en Format Distance
                List<Vector2> _attackRange = enemy.rangeATK();
                // Liste des attackRange en Format Position
                List<Vector2> _attackRangePos = enemy.AttackRange(enemy.transform.position);
                // Liste des Deplacement en Format Position
                List<Vector2> _deplacement = enemy.Deplacement(enemy.transform.position);
                // Assigner les scores  // Trouvez le target / MinMax Algorithme
                foreach (BaseUnit hero in listHero){
                    //print("Cible : "+hero.UnitType);
                    if(CombatSystem.unitInRange(_attackRangePos,hero.transform.position)){
                        // Attack
                        CombatSystem.CalculCombat(false,true,enemy,hero);
                        double _targetscore = AIScore.Instance.getTargetScore(hero);
                        //print("Score : " + _targetscore);
                        _action = false;
                    }
                    else {
                        // Trouvez la prochaine position / A*
                        Pathfinder.trouverChemin(enemy,hero,_deplacement,_attackRange);
                        Pathfinder.trouverNextMove(enemy,hero,_deplacement,_attackRange);
                        // AStar.FindPath(enemypos,targetpos);
                    }
                    
                }
                // Initie la Position 
                List<Vector2> _attackRangeNewPos = enemy.AttackRange(enemy.transform.position);
                if (_action) {
                    Vector2 _bestTileScore = AIScore.Instance.getBestTile();
                    Tile NextMove = GridManager.Instance.GetTileAtPosition(_bestTileScore);
                    NextMove.SetUnit(enemy);
                    print("PrevieuxTile : " + enemy.LastTile + "BestTile : " + _bestTileScore);
                    _attackRangeNewPos = enemy.AttackRange(enemy.transform.position);
                    foreach(BaseUnit hero in listHero){
                        if (CombatSystem.unitInRange(_attackRangeNewPos,hero.transform.position)){
                            CombatSystem.CalculCombat(false,true,enemy,hero);
                        }
                    }
                }
                BaseUnit _targetUnit = AIScore.Instance.getBestTarget();
                
                if (_targetUnit != null){
                    if (CombatSystem.unitInRange(_attackRangeNewPos,_targetUnit.transform.position)){
                        CombatSystem.CalculCombat(true,false,enemy,_targetUnit);
                    }
                }
            }
            ActionEnemies();
            Invoke("ActionEnemies", 5);
        }
    }

    // Update Position Hero
    public void UpdatePositionHero() { 
        foreach (BaseUnit hero in listHero){
            positionHero.Add(hero.transform.position);
        }
    }

    // Update Position Enemy
    public void UpdatePositionEnemy() {
        foreach (BaseUnit enemy in listEnemy) {
            positionEnemy.Add(enemy.transform.position);
        }
    }
   
    // Setting pour le Selected Hero
    public void SetSelectedHero(BaseUnit hero) { // SelectedHero
        SelectedHero = hero;
        GridManager.Instance.ShowAttackRange(null);
        GridManager.Instance.ShowMovement(null);
        MenuManager.Instance.ShowSelectedHero(hero);
        MenuManager.Instance.ShowActionUi(hero);
    }

    // Setting pour le Selected Enemy
    public void SetSelectedOpponent(BaseUnit enemy) {
        MenuManager.Instance.ShowSelectedOpponent(enemy);
    }

    // Réactivation des Units
    public void ActiveActions(string faction) {
        if (faction == "hero"){
            foreach (BaseUnit hero in listHero) {
                hero.setMoveAble(true);
                hero.setAtkAble(true);
            }
        }
        else {
            foreach (BaseUnit enemy in listEnemy) {
                enemy.setMoveAble(true);
                enemy.setAtkAble(true);
            }
        } 
    }
}
