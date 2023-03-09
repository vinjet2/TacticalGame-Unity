//***************************************//
// Game Manager
// Verification des Stats du Jeu & le Retour au menu (State method)
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

    void Start() {
        ChangeState(GameState.GenerateGrid);
    }

    // Les États du jeu
    public void ChangeState(GameState newState){
        GameState = newState;
        switch (newState) {
            case GameState.GenerateGrid: // Creer la map
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnHeroes: // Creer les Unit
                UnitManager.Instance.SpawnHeroes();
                break;
            case GameState.SpawnEnemies: // Creer les Ennemis
                UnitManager.Instance.SpawnEnemies();
                break;
            case GameState.PlayerTurn: // Tour du Joeur
                UnitManager.Instance.ActiveActions("hero");// Rendre tous les Units leur Action Possible 
                MenuManager.Instance.ShowTurn("player");
                break;
            case GameState.EnemiesTurn: // Tour de l'ennemi
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