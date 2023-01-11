//***************************************//
// [Nom du Fichier] BaseUnit (Template Method)
// [Utilisation] Les Etats des Units
// Fait Par: Vincent & Mathieu
//***************************************//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SuperClasse de la création d'un Unit  (Template Method)
public abstract class BaseUnit : MonoBehaviour
{   
    private string _unitName; // Le nom du Unit
    protected string _unitType; // Le type du Unit
    public UnitType UnitType;
    public Tile OccupiedTile; // La position du Unit
    public Tile LastTile; // La position précédente
    public Faction Faction; // Hero ou Enemy
    protected bool _moveAble = true; // Le Unit peut se deplacer
    protected bool _atkAble = true; // Le Unit peut attaquer
    private double _atk; // Atk stats
    private double _def; // Def stats
    private float _critChance; // Critchance (%)
    private double _maxHP; // Hp Max stats
    private double _currentHp; // Current Hp 
    private double _spd; // Spd stats
    private bool playerUnit = false; // Is Player Allied
    private int unitDirection; // La Direction du Unit
    protected SpriteRenderer spriteR;
    protected Sprite _sprite;

    //L'équipement du Unit (HeldObject)

    public BaseUnit(double atk, double def, float critChance, double maxHP, double spd, string unitName)
    {
        this._atk = atk;
        this._def = def;
        this._critChance = critChance;
        this._maxHP = maxHP;
        this._currentHp = maxHP;
        this._spd = spd;
        this._unitName = unitName;
        //this._sprite = Resources.Load<Sprite>(_unitName + "_Unit.png");
        this.setisAllied();
    }

    void Awake() {
        this.spriteR = GetComponent<SpriteRenderer>();
    } 

    // Setter
    public void setCurrentUnitHP(string unitName, double currentHP) {}
    public void takesDamage(double _degat) {this._currentHp = _currentHp - _degat;}

    public void setDeath() {
        UnitManager.Instance.UnitMeurt(this);
        OccupiedTile.UnsetUnit(this);
    }
    public void setMoveAble(bool moveAble) {
        if (moveAble == false) {
            this.spriteR.color = new Color(1f,1f,1f,0.8f);// Rendre l'image plus semi-transparent
        }
        else {
            this.spriteR.color = new Color(1f,1f,1f,1f);// Rendre l'image Original
        }
        this._moveAble = moveAble;
    }
    public void setAtkAble(bool atkAble) {
        if (atkAble == false){
            this.spriteR.color = new Color(1f,1f,1f,0.5f);
        }
        this._atkAble = atkAble;
    }
            
    public void setUnitDirection(int unitDirection) {}

    public void setisAllied() 
    {
        if(this.Faction == Faction.Hero)
        {
            this.playerUnit = true;
        }
        else if(this.Faction == Faction.Enemy)
        {
            this.playerUnit = false;
        }
    }
    


    // Bool
    public bool isAlive() { return (_currentHp > 0);}
    public bool isAllied() { return playerUnit;}
    public bool isMoveAble() { return _moveAble;}
    public bool isAtkAble() { return _atkAble;}

    // Getter
    public double getAtk() { return _atk;}
    public double getDef() { return _def;}
    public double getSpeed() { return _spd;}
    public float getCritChance() { return _critChance;}
    public double getCurrentHP() { return _currentHp;}
    public double getMaxHP() { return _maxHP; }
    public int getUnitDirection() { return unitDirection;}
    public string getUnitType() { return _unitType;}
    public string getUnitName() { return _unitName;}
    // public HeldObject getHeldObject() {}

    public void Undo() {
        if (OccupiedTile != null) OccupiedTile.OccupiedUnit = null;
        transform.position = LastTile.transform.position;
        LastTile.OccupiedUnit = this;
        OccupiedTile = LastTile;
    }

    public List<Vector2> rangeMovement(){
        List<Vector2> _distanceDeplacement = new List<Vector2>();
         switch(UnitType){
            case UnitType.Pion: // Sides & Diagonals
                _distanceDeplacement.Add(new Vector2(0,0));
                _distanceDeplacement.Add(new Vector2(0,1));
                _distanceDeplacement.Add(new Vector2(0,1));
                _distanceDeplacement.Add(new Vector2(1,1));
                _distanceDeplacement.Add(new Vector2(1,0));
                _distanceDeplacement.Add(new Vector2(1,-1));
                _distanceDeplacement.Add(new Vector2(-1,1));
                _distanceDeplacement.Add(new Vector2(-1,0));
                _distanceDeplacement.Add(new Vector2(-1,-1));
                break;
            case UnitType.Rook: // Different pour AI
                if (Faction == Faction.Hero){
                    _distanceDeplacement.Add(new Vector2(0,0));
                    _distanceDeplacement.Add(new Vector2(0,1));
                    _distanceDeplacement.Add(new Vector2(0,-1));
                    _distanceDeplacement.Add(new Vector2(1,1));
                    _distanceDeplacement.Add(new Vector2(1,0));
                    _distanceDeplacement.Add(new Vector2(1,-1));
                    _distanceDeplacement.Add(new Vector2(2,0));
                }
                else {
                    _distanceDeplacement.Add(new Vector2(0,0));
                    _distanceDeplacement.Add(new Vector2(0,1));
                    _distanceDeplacement.Add(new Vector2(0,-1));
                    _distanceDeplacement.Add(new Vector2(-1,1));
                    _distanceDeplacement.Add(new Vector2(-1,0));
                    _distanceDeplacement.Add(new Vector2(-1,-1));
                    _distanceDeplacement.Add(new Vector2(-2,0));
                }
                break;
            case UnitType.Caster: // Sides
                _distanceDeplacement.Add(new Vector2(0,0));
                _distanceDeplacement.Add(new Vector2(-1,0));
                _distanceDeplacement.Add(new Vector2(0,1));
                _distanceDeplacement.Add(new Vector2(0,-1));
                _distanceDeplacement.Add(new Vector2(1,0));
                break;
            case UnitType.Lance: // Different pour AI
                if (Faction == Faction.Hero){
                    _distanceDeplacement.Add(new Vector2(0,0));
                    _distanceDeplacement.Add(new Vector2(-1,0));
                    _distanceDeplacement.Add(new Vector2(1,0));
                    _distanceDeplacement.Add(new Vector2(1,1));
                    _distanceDeplacement.Add(new Vector2(1,-1));
                }
                else {
                    _distanceDeplacement.Add(new Vector2(0,0));
                    _distanceDeplacement.Add(new Vector2(-1,0));
                    _distanceDeplacement.Add(new Vector2(1,0));
                    _distanceDeplacement.Add(new Vector2(1,1));
                    _distanceDeplacement.Add(new Vector2(1,-1));
                }
                break;
            case UnitType.Knight: // Losange
                _distanceDeplacement.Add(new Vector2(0,0));
                _distanceDeplacement.Add(new Vector2(3,0));
                _distanceDeplacement.Add(new Vector2(2,1));
                _distanceDeplacement.Add(new Vector2(2,-1));
                _distanceDeplacement.Add(new Vector2(1,2));
                _distanceDeplacement.Add(new Vector2(1,-2));
                _distanceDeplacement.Add(new Vector2(0,3));
                _distanceDeplacement.Add(new Vector2(0,-3));
                _distanceDeplacement.Add(new Vector2(-1,2));
                _distanceDeplacement.Add(new Vector2(-1,-2));
                _distanceDeplacement.Add(new Vector2(-2,1));
                _distanceDeplacement.Add(new Vector2(-2,-1));
                _distanceDeplacement.Add(new Vector2(-3,0));
                break;
            case UnitType.GGeneral: // Different pour AI
                if (Faction == Faction.Hero){
                    _distanceDeplacement.Add(new Vector2(0,0));
                    _distanceDeplacement.Add(new Vector2(-1,0));
                    _distanceDeplacement.Add(new Vector2(1,0));
                    _distanceDeplacement.Add(new Vector2(1,1));
                    _distanceDeplacement.Add(new Vector2(1,-1));
                    _distanceDeplacement.Add(new Vector2(2,0));
                    _distanceDeplacement.Add(new Vector2(2,1));
                    _distanceDeplacement.Add(new Vector2(2,-1));
                }
                else {
                    _distanceDeplacement.Add(new Vector2(0,0));
                    _distanceDeplacement.Add(new Vector2(1,0));
                    _distanceDeplacement.Add(new Vector2(-1,0));
                    _distanceDeplacement.Add(new Vector2(-1,1));
                    _distanceDeplacement.Add(new Vector2(-1,-1));
                    _distanceDeplacement.Add(new Vector2(-2,0));
                    _distanceDeplacement.Add(new Vector2(-2,1));
                    _distanceDeplacement.Add(new Vector2(-2,-1));
                }
                
                break;
            case UnitType.SGeneral: // Sides & Diagonals
                _distanceDeplacement.Add(new Vector2(0,0));
                _distanceDeplacement.Add(new Vector2(0,1));
                _distanceDeplacement.Add(new Vector2(0,-1));
                _distanceDeplacement.Add(new Vector2(1,1));
                _distanceDeplacement.Add(new Vector2(1,0));
                _distanceDeplacement.Add(new Vector2(1,-1));
                _distanceDeplacement.Add(new Vector2(-1,1));
                _distanceDeplacement.Add(new Vector2(-1,0));
                _distanceDeplacement.Add(new Vector2(-1,-1));
                break;
            case UnitType.King: // Sides & Diagonals
                _distanceDeplacement.Add(new Vector2(0,0));
                _distanceDeplacement.Add(new Vector2(0,1));
                _distanceDeplacement.Add(new Vector2(0,-1));
                _distanceDeplacement.Add(new Vector2(1,1));
                _distanceDeplacement.Add(new Vector2(1,0));
                _distanceDeplacement.Add(new Vector2(1,-1));
                _distanceDeplacement.Add(new Vector2(-1,1));
                _distanceDeplacement.Add(new Vector2(-1,0));
                _distanceDeplacement.Add(new Vector2(-1,-1));
                break;
            default:
                _distanceDeplacement.Add(new Vector2(0,0));
                break;
        }
        return _distanceDeplacement;
    }
    public List<Vector2> rangeATK(){
        List<Vector2> _distanceAttack = new List<Vector2>();
        switch (UnitType)
        {
            case UnitType.Pion: // Sides & Diagonals
                _distanceAttack.Add(new Vector2(0,1));
                _distanceAttack.Add(new Vector2(0,-1));
                _distanceAttack.Add(new Vector2(1,1));
                _distanceAttack.Add(new Vector2(1,0));
                _distanceAttack.Add(new Vector2(1,-1));
                _distanceAttack.Add(new Vector2(-1,1));
                _distanceAttack.Add(new Vector2(-1,0));
                _distanceAttack.Add(new Vector2(-1,-1));
                break;
            case UnitType.Rook: // Different pour AI
                if (Faction == Faction.Hero){
                    _distanceAttack.Add(new Vector2(0,1));
                    _distanceAttack.Add(new Vector2(0,-1));
                    _distanceAttack.Add(new Vector2(1,0));
                }
                else {
                    _distanceAttack.Add(new Vector2(0,1));
                    _distanceAttack.Add(new Vector2(0,-1));
                    _distanceAttack.Add(new Vector2(-1,0));
                }
                break;
            case UnitType.Caster: // Geant Losange (Un peu trop gros?)
                _distanceAttack.Add(new Vector2(3,0));
                _distanceAttack.Add(new Vector2(2,1));
                //_distanceAttack.Add(new Vector2(2,0));
                _distanceAttack.Add(new Vector2(2,-1));
                _distanceAttack.Add(new Vector2(1,2));
                _distanceAttack.Add(new Vector2(1,-2));
                _distanceAttack.Add(new Vector2(0,3));
                //_distanceAttack.Add(new Vector2(0,2));
                //_distanceAttack.Add(new Vector2(0,-2));
                _distanceAttack.Add(new Vector2(0,-3));
                _distanceAttack.Add(new Vector2(-1,2));
                _distanceAttack.Add(new Vector2(-1,-2));
                _distanceAttack.Add(new Vector2(-2,1));
                //_distanceAttack.Add(new Vector2(-2,0));
                _distanceAttack.Add(new Vector2(-2,-1));
                _distanceAttack.Add(new Vector2(-3,0));
                break;
            case UnitType.Lance: // Different pour AI
                if (Faction == Faction.Hero){
                    _distanceAttack.Add(new Vector2(1,1));
                    _distanceAttack.Add(new Vector2(1,0));
                    _distanceAttack.Add(new Vector2(1,-1));
                }
                else {
                    _distanceAttack.Add(new Vector2(-1,1));
                    _distanceAttack.Add(new Vector2(-1,0));
                    _distanceAttack.Add(new Vector2(-1,-1));
                }
                
                break;
            case UnitType.Knight: // Different pour AI (Sides except behind)
                if (Faction == Faction.Hero){
                    _distanceAttack.Add(new Vector2(0,1));
                    _distanceAttack.Add(new Vector2(0,-1));
                    _distanceAttack.Add(new Vector2(1,0));
                }
                else {
                    _distanceAttack.Add(new Vector2(0,1));
                    _distanceAttack.Add(new Vector2(0,-1));
                    _distanceAttack.Add(new Vector2(-1,0));
                }
                break;
            case UnitType.GGeneral: // Different pour AI
                if (Faction == Faction.Hero){
                    _distanceAttack.Add(new Vector2(2,1));
                    _distanceAttack.Add(new Vector2(2,0));
                    _distanceAttack.Add(new Vector2(2,-1));
                }
                else {
                    _distanceAttack.Add(new Vector2(-2,1));
                    _distanceAttack.Add(new Vector2(-2,0));
                    _distanceAttack.Add(new Vector2(-2,-1));
                }
                break;
            case UnitType.SGeneral: // Sides & Diagonals
                _distanceAttack.Add(new Vector2(0,1));
                _distanceAttack.Add(new Vector2(0,-1));
                _distanceAttack.Add(new Vector2(1,1));
                _distanceAttack.Add(new Vector2(1,0));
                _distanceAttack.Add(new Vector2(1,-1));
                _distanceAttack.Add(new Vector2(-1,1));
                _distanceAttack.Add(new Vector2(-1,0));
                _distanceAttack.Add(new Vector2(-1,-1));
                break;
            case UnitType.King: // Different pour AI (Additionner les zones d'attaque)
                if (Faction == Faction.Hero){
                    _distanceAttack.Add(new Vector2(1,1));
                    _distanceAttack.Add(new Vector2(1,0));
                    _distanceAttack.Add(new Vector2(1,-1));
                }
                else {
                    _distanceAttack.Add(new Vector2(-1,1));
                    _distanceAttack.Add(new Vector2(-1,0));
                    _distanceAttack.Add(new Vector2(-1,-1));
                }
                
                break;
            default:
                break;
        }
        return _distanceAttack;
    }
    public abstract List<Vector2> Deplacement(Vector2 pos);
    public abstract List<Vector2> AttackRange(Vector2 pos);
}
