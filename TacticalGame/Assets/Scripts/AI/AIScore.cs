//***************************************//
// [Nom du Fichier] AI Score
// [Utilisation] Les Dictionnaires qui garde le Score en Memoire Pour le AI
// Fait Par: Vincent
//***************************************//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIScore : MonoBehaviour
{   
    public static AIScore Instance;

    // Pos, Score
    private Dictionary<Vector2, double> _tilesScores;
    // BaseUnit , Score
    private Dictionary<BaseUnit, double> _targetsScores;

    void Awake() {
        Instance = this;
    }

    // Estimé un score pour chaque Tile de Déplacement possible
    public  void Score() {
        // Distance de chaque Ally

        // Distance de chaque Ennemy

        // Si je peut attaquer ma cible, Combien de Hp vas-t-il perdre.

    }

    public void initDictTiles() {
        _tilesScores = new Dictionary<Vector2,double>();
    }

    public  void initDictTargets() {
        _targetsScores = new Dictionary<BaseUnit, double>();
    }

    public void printTilesScore() {
        foreach(KeyValuePair<Vector2,double> tile in _tilesScores){
            print("Tile : " + tile.Key + " Score : " + tile.Value);
        }
    }

    public void printTargetScore() {
        foreach(KeyValuePair<BaseUnit,double> target in _targetsScores){
            print("Target : " + target.Key + " Score : " + target.Value);
        }
    }

    public  double getTileScore(Vector2 _tile){
        return _tilesScores.Where(t=>t.Key == _tile).First().Value;
    }

    public  double getTargetScore(BaseUnit _target){
        return _targetsScores.Where(t=>t.Key == _target).First().Value;
    }

    public Vector2 getBestTile() {
        return _tilesScores.Aggregate((x,y) => x.Value > y.Value ? x : y).Key;
    }

    public BaseUnit getBestTarget(){
        if (_targetsScores.Count() == 0){return null;}
        return _targetsScores.Aggregate((x,y) => x.Value > y.Value ? x : y).Key;
    }

    public  double getBestTileScore() {
        return _tilesScores.Values.Max();
    } 

    public  double getBestTargetScore() {
        return _targetsScores.Values.Max();
    }
    public  void setTileScore(Vector2 _tile, double _score) {
        if(_tilesScores.TryGetValue(_tile,out double score)) {
            score += _score;
            _tilesScores[_tile] = score;
        }
        else {
            _tilesScores.Add(_tile, _score);
        }
    }

    public  void setTargetScore(BaseUnit _target, double _score) {
        if (_targetsScores.TryGetValue(_target, out double score)){
            score += _score;
            _targetsScores[_target] = score;
        }
        else {
            _targetsScores.Add(_target,_score);
        }
    }
}
