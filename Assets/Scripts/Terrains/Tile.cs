//***************************************//
// [Nom du Fichier] Tile (Template Method)
// [Utilisation] La vérification des Mouvement du joueur,l'activation des UI d'attaqueRange & Déplacement 
// La classe parent de toute les Tuiles & l'utilisation de OccupiedUnit comme variable Importante au jeu.
// Fait Par: Vincent
//***************************************//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _attackRange;
    [SerializeField] private GameObject _deplacement;
    [SerializeField] public string TileName;
    //[SerializeField] private Script UnitType;
    public BaseUnit OccupiedUnit;
    public bool Walkable => !_IsWall && OccupiedUnit == null;

    protected bool _IsWall;
    // Private Variables
    private string _Name;
    private int _DPS;
    private int _MvmPenalty;
    private bool _CanWalk;
    private bool _CanJump;
    private string _TerrainType;
    private float _TerrainBuff;

    private int x,y;


    // AI Stuff (Pas Utilisé)
    // Public Variables
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;
    public List<Tile> Neighbors { get; protected set; }
    public Tile Connection { get; private set; }

    public void SetConnection(Tile tile) {Connection = tile;}

    public virtual void init(int x, int y) {
        this.x = x;
        this.y = y;
        //Pos = transform.positon;
    }
    
    // Creator
    public void CreateTile(string name, int MvmPenalty, bool CanWalk, bool CanJump, bool IsWall, string TerrainType, float TerrainBuff)
    {
        this._Name = name;
        this._MvmPenalty = MvmPenalty;
        this._CanWalk = CanWalk;
        this._CanJump = CanJump;
        this._IsWall = IsWall;
        this._TerrainType = TerrainType;
        this._TerrainBuff = TerrainBuff;
    }

    // Getter
    public string getName() { return _Name; }
    public bool getCanWalk() { return _CanWalk; }
    public int getDPS() { return _DPS; }
    public int getMvmPenalty() { return _MvmPenalty; }
    public bool getCanJump() { return _CanJump; }
    public bool getIsWall() { return _IsWall; }
    public string getTerrainType() { return _TerrainType; }
    public float getTerrainBuff() { return _TerrainBuff; }
    public float getG() { return G;}

    // Setter
    public void setIsWall(bool isWall){this._IsWall = isWall;}
    public void SetG(float g) {this.G = g;}
    public void SetH(float h) {this.H = h;}

    // MouseOver
    void OnMouseEnter() {
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
        if (this._deplacement.active == true || OccupiedUnit == UnitManager.Instance.SelectedHero){
            if (UnitManager.Instance.SelectedHero != null){
                GridManager.Instance.ShowAttackRange(null);
                List<Vector2> _attackRange = UnitManager.Instance.SelectedHero.AttackRange(transform.position);
                GridManager.Instance.ShowAttackRange(_attackRange);
            }
        }
        if (OccupiedUnit != null){ // Il y a un Unit sur la Case
            List<Vector2> _deplacement = OccupiedUnit.Deplacement(OccupiedUnit.transform.position);
            if (OccupiedUnit.isMoveAble()){
                GridManager.Instance.ShowMovement(_deplacement);
            }
            if (OccupiedUnit.Faction == Faction.Enemy || (OccupiedUnit.isAtkAble() && OccupiedUnit.isMoveAble() == false)){
                List<Vector2> _attackRange = OccupiedUnit.AttackRange(OccupiedUnit.transform.position);
                GridManager.Instance.ShowAttackRange(_attackRange);
                if (OccupiedUnit.Faction == Faction.Enemy || OccupiedUnit.isAtkAble() == false) {
                    foreach (Vector2 movePos in _deplacement){
                        List<Vector2> _atkRange = OccupiedUnit.AttackRange(movePos);
                        GridManager.Instance.ShowAttackRange(_atkRange);
                    }
                }
            }
        }
    }

    void OnMouseExit() {
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null); 
        if (UnitManager.Instance.SelectedHero == null){ // Reset UI
            GridManager.Instance.ShowAttackRange(null);
            GridManager.Instance.ShowMovement(null);
        }
        if (OccupiedUnit != null && OccupiedUnit != UnitManager.Instance.SelectedHero && UnitManager.Instance.SelectedHero != null){
            GridManager.Instance.ShowMovement(null);
            List<Vector2> _deplacement = UnitManager.Instance.SelectedHero.Deplacement(UnitManager.Instance.SelectedHero.transform.position);// Vas cherchez le Deplacement du UnitType
            GridManager.Instance.ShowMovement(_deplacement);
        }
    }

    // Click
    void OnMouseDown() {
        if (GameManager.Instance.GameState != GameState.PlayerTurn) {
            return;
        }

        if(Time.timeScale > 0.0f)
        {
            if (OccupiedUnit != null)
            { // Click sur une Case Occupee
                if (OccupiedUnit.Faction == Faction.Hero)
                { // Selectionne un Hero Playable
                    if (OccupiedUnit == UnitManager.Instance.SelectedHero)
                    { // Selected Hero
                        GridManager.Instance.ShowMovement(null);
                        List<Vector2> _attackRange = OccupiedUnit.AttackRange(OccupiedUnit.transform.position);
                        GridManager.Instance.ShowAttackRange(_attackRange);
                        if (UnitManager.Instance.SelectedHero.isMoveAble())
                        {
                            UnitManager.Instance.SelectedHero.setMoveAble(false); // Utiliser le Movement du tour
                        }
                        else if (UnitManager.Instance.SelectedHero.isAtkAble())
                        {
                            UnitManager.Instance.SelectedHero.setAtkAble(false);
                            UnitManager.Instance.ActionHeroes();
                        }

                    }
                    UnitManager.Instance.SetSelectedHero(OccupiedUnit);
                }
                else if (OccupiedUnit.Faction == Faction.Enemy)
                { // Selectionne un Ennemy
                    if (UnitManager.Instance.SelectedHero != null)
                    {
                        List<Vector2> _attackRange = UnitManager.Instance.SelectedHero.AttackRange(UnitManager.Instance.SelectedHero.transform.position);
                        UnitManager.Instance.SetSelectedOpponent(OccupiedUnit);
                        CombatSystem.CalculCombat(false, false, UnitManager.Instance.SelectedHero, OccupiedUnit);
                        if (CombatSystem.unitInRange(_attackRange, OccupiedUnit.transform.position))
                        {
                            CombatSystem.CalculCombat(true, false, UnitManager.Instance.SelectedHero, OccupiedUnit);
                            UnitManager.Instance.SelectedHero.setMoveAble(false);
                            UnitManager.Instance.SelectedHero.setAtkAble(false);
                            UnitManager.Instance.ActionHeroes();
                        }
                        // var enemy = (BaseEnemy)OccupiedUnit;
                        // Destroy(enemy.GameObject);
                    }
                }
            }
            else
            { // Click sur une Case Vide
                if (UnitManager.Instance.SelectedHero != null)
                { // Si un hero est Selectionner => Mouvement
                    GridManager.Instance.ShowAttackRange(null);
                    if (this._deplacement.active == true)
                    {
                        if (this.Walkable)
                        {
                            if (UnitManager.Instance.SelectedHero.isMoveAble() == true)
                            { // Deplacement Possible
                                SetUnit(UnitManager.Instance.SelectedHero);
                                List<Vector2> _attackRange = OccupiedUnit.AttackRange(OccupiedUnit.transform.position);
                                GridManager.Instance.ShowAttackRange(_attackRange);
                                UnitManager.Instance.SelectedHero.setMoveAble(false); // Utiliser l'action du tour
                            }
                        }
                    }

                    UnitManager.Instance.SetSelectedHero(null);
                    UnitManager.Instance.SetSelectedOpponent(null);
                    GridManager.Instance.ShowMovement(null);
                    //GridManager.Instance.ShowAttackRange(null);

                }
            }
        }
    }

    // A chaque fois qu'un unit change de position
    public void SetUnit(BaseUnit unit) {
        unit.LastTile = unit.OccupiedTile;
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }
    // A chaque fois qu'un unit meurt -> Hors de la Caméra!
    public void UnsetUnit(BaseUnit unit) {
        unit.transform.position = new Vector2(100,100);
        OccupiedUnit = null;
    }

    // Retour a la position précedante 
    public void Undo(BaseUnit unit) {
        if(unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = unit.LastTile.transform.position;
        unit.LastTile.OccupiedUnit = unit;
        unit.OccupiedTile = unit.LastTile;
    }

    // Activer les UI Movevement
    public void ActivateMovement(bool active) {
        if (active == false){
            _deplacement.SetActive(false);
            return;
        }
        _deplacement.SetActive(true);
    }
    // Activer les UI AttackRange
    public void ActivateAttackRange(bool active) {
        if (active == false){
            _attackRange.SetActive(false);
            return;
        }
        _attackRange.SetActive(true);
    }

    // AI Fonction (Tarodev A* findPath()) (50% complêté)
    public void FindNeighbors(List<Vector2> Tiles ) {
        Neighbors = new List<Tile>();

        //foreach (var tile in Tiles.Select(t => GridManager.Instance.GetTileAtPosition(t)).Where(t => t != null)){
        //    Neighbors.Add(tile);
        //}
    }

    // GetDistance d'une tuile à l'autre. Pathdinder trouverNextMove()
    public double GetDistance(Vector2 target) {
        Vector2 presentTile = transform.position;
        //print("x " + presentTile.x + " y " + presentTile.y + " targetx " + target.x + " targety " + target.y);
        var dist = new Vector2Int(System.Math.Abs((int)presentTile.x - (int)target.x),System.Math.Abs((int)presentTile.y - (int)target.y));
        //print("Distance " + dist);

        var lowest = System.Math.Min(dist.x,dist.y);
        var highest = System.Math.Max(dist.x,dist.y);

        var horizontalMovesRequired = highest - lowest;

        return lowest * 14 + horizontalMovesRequired * 10;
    }
}
