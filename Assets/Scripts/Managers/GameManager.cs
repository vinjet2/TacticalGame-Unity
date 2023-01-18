//***************************************//
// [Nom du Fichier] Game Manager
// [Utilisation] Verification des Stats du Jeu & le Retour au menu
// Fait Par: Vincent
//***************************************//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public GameState GameState;


    void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    // Battle Systeme
    public void ChangeState(GameState newState){
        GameState = newState;
        switch (newState) {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnHeroes:
                UnitManager.Instance.SpawnHeroes();
                break;
            case GameState.SpawnEnemies:
                UnitManager.Instance.SpawnEnemies();
                break;
            case GameState.PlayerTurn:
                UnitManager.Instance.ActiveActions("hero");// Rendre tous les Units leur Action Possible 
                MenuManager.Instance.ShowTurn("player");
                break;
            case GameState.EnemiesTurn:
                UnitManager.Instance.ActiveActions("enemy");
                MenuManager.Instance.ShowTurn("enemy");
                UnitManager.Instance.EnemiesTurn();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
    public void RetournMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}



public enum GameState {
    GenerateGrid = 0,
    SpawnHeroes = 1,
    SpawnEnemies = 2,
    PlayerTurn = 3,
    EnemiesTurn = 4
}