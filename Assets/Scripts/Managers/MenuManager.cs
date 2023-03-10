//***************************************//
// Menu Manager
// L'activation des UI pour le joueur
// Fait Par: Vincent
//***************************************//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    // Reference GameObject
    [SerializeField] private GameObject _selectedHeroObject, _selectedOpponentObject, _tileObject, _tileUnitObject;
    // UI Stats
    [SerializeField] private GameObject _selectedDamage, _selectedCurrentHp, _selectedComingHp;
    [SerializeField] private GameObject _opponentDamage, _opponentCurrentHp, _opponentComingHp;
    // UI Combat
    [SerializeField] private GameObject _playerTurn, _enemyTurn;
    [SerializeField] private GameObject _combatUI, _moveUndoButton, _attackButton;
    private bool Move_Undo = true;
    // UI Menu
    [SerializeField] private GameObject _FinDePartie, _RetourMenu;

    private bool _cUIActif, Atk_Move = false;
    // Pause Menu
    private float _time = 0.0f;
    public float _interPeriod = 0.1f;

    public Text selectedDamage, selectedCurrentHp, selectedComingHp, opponentDamage, opponentCurrentHp, opponentComingHp;

    void Awake() {
        Instance = this;
        InvokeRepeating("AlterneAtkMove", 1.0f, 3.0f);
        selectedDamage = _selectedDamage.GetComponent<Text>();
        selectedCurrentHp = _selectedCurrentHp.GetComponent<Text>();
        selectedComingHp = _selectedComingHp.GetComponent<Text>();
        opponentDamage = _opponentDamage.GetComponent<Text>();
        opponentCurrentHp = _opponentCurrentHp.GetComponent<Text>();   
        opponentComingHp = _opponentComingHp.GetComponent<Text>();     
    }

    void Start() {
        Button btnAtk = _attackButton.GetComponent<Button>();
        Button btnMove = _moveUndoButton.GetComponent<Button>();
        btnAtk.onClick.AddListener(AttackOnClick);
        btnMove.onClick.AddListener(MoveOnClick);
    }

    void Update() {
        if (_cUIActif) {

            if (Atk_Move) {
                AttackOnClick();
            }
            else {
                MoveOnClick();
            }
            
        }
        else {
            CancelInvoke();
        }
        //_time += _time.deltaTime;


        //StartCoroutine( AlterneAtkMove());    
    }

    void AlterneAtkMove() {
        Atk_Move = !Atk_Move;
    }

    void AttackOnClick() {
        //GridManager.Instance.ShowMovement(null);
        //MonoBehaviour.print("AttackOnClick");
        BaseUnit unit = UnitManager.Instance.SelectedHero;
        if (unit != null){
            List<Vector2> _attackRange = unit.AttackRange(unit.transform.position);
            GridManager.Instance.ShowAttackRange(_attackRange);
        } 
    }

    void MoveOnClick() {
        //GridManager.Instance.ShowAttackRange(null);
        //MonoBehaviour.print("MoveOnClick");
        BaseUnit unit = UnitManager.Instance.SelectedHero;
        if (unit != null){
            List<Vector2> _deplacement = unit.Deplacement(unit.transform.position);
            GridManager.Instance.ShowMovement(_deplacement);
        }
    }

    public void ShowTileInfo(Tile tile) {
        if (tile == null) {
            _tileObject.SetActive(false);
            _tileUnitObject.SetActive(false);
            GameUIScript.Instance.setAllDisable();
            return;
        }

        _tileObject.GetComponentInChildren<Text>().text = tile.TileName;
        _tileObject.SetActive(true);

        if (tile.OccupiedUnit) {
            _tileUnitObject.GetComponentInChildren<Text>().text = tile.OccupiedUnit.getUnitName();
            //_tileUnitObject.SetActive(true);
            GameUIScript.Instance.setUnit(tile.OccupiedUnit, true);
        }
    }
    
    public void ShowSelectedHero(BaseUnit hero) {
        if (hero == null) {
            _selectedHeroObject.SetActive(false);
            GameUIScript.Instance.setUnit(hero, false);
            return;
        }

        _selectedHeroObject.GetComponentInChildren<Text>().text = hero.getUnitName();
        selectedDamage.text = "Damage " +  hero.getAtk().ToString();
        selectedCurrentHp.text = "CurrentHp " + hero.getCurrentHP().ToString();
        GameUIScript.Instance.setUnit(hero, true);
        _selectedHeroObject.SetActive(true);
    }

    public void ShowSelectedOpponent(BaseUnit enemy){
        if (enemy == null) {
            _selectedOpponentObject.SetActive(false);
            return;
        }

        _selectedOpponentObject.GetComponentInChildren<Text>().text = enemy.getUnitName();
        opponentCurrentHp.text =  "CurrentHp" + enemy.getCurrentHP().ToString();
        _selectedOpponentObject.SetActive(true);
    }

    public void ShowCombatUI(double _degat, double _degatSubi, double _remainingHpDef, double _remainingHpAtk) {
        if (_degat == null || _degatSubi == null){
            selectedDamage.text = "";
            selectedComingHp.text = "";
            return;
        }
        selectedDamage.text = "Damage " +_degat.ToString();
        opponentDamage.text = "Damage " + _degatSubi.ToString();
        selectedComingHp.text = "ComingHp " + _remainingHpAtk.ToString();
        opponentComingHp.text = "ComingHp " +_remainingHpDef.ToString();

    }

    public void ShowActionUi(BaseUnit hero) {
        if (hero == null){
            _cUIActif = false;
            _combatUI.SetActive(false);
            return;
        }
        if (hero.isMoveAble()) {
            Move_Undo = true;
            _moveUndoButton.GetComponentInChildren<Image>().color = new Color32(91,191,176,255);
            _moveUndoButton.GetComponentInChildren<Text>().text = "Move";

        }
        else {
            Move_Undo = false;
            _moveUndoButton.GetComponentInChildren<Text>().text = "Undo";
            _moveUndoButton.GetComponentInChildren<Image>().color = new Color(1f,1f,1f,0.5f);
        }
        if (hero.isAtkAble()) {
            _attackButton.GetComponentInChildren<Image>().color = new Color32(191, 86, 100, 255);
        }
        else {
            _attackButton.GetComponentInChildren<Image>().color = new Color32(191, 86, 100, 175);

        }
        _cUIActif = true;
        _combatUI.SetActive(true);
    }

    public void ShowFinDePartie(bool Win) {
        _FinDePartie.SetActive(true);
        _RetourMenu.SetActive(true);
        if (Win) {
            _FinDePartie.GetComponentInChildren<Text>().text = "Victory!";
        }
        else {
            _FinDePartie.GetComponentInChildren<Text>().text = "Defeat!";
        }
    }

    public async void  ShowTurn(string turn) {
        if (turn == "player") {
            await Task.Delay(2000);
            _playerTurn.SetActive(true);
            await Task.Delay(2000);
            _playerTurn.SetActive(false);
        }
        else {
            _enemyTurn.SetActive(true);
            await Task.Delay(2000);
            _enemyTurn.SetActive(false);
        }
    }
}
