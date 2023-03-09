//***************************************//
// Pathfinder - Trouver le prochain et le meilleure Chemin 
// Fonction Important pour que le AI trouve son chemin
// Fait Par: Vincent
//***************************************//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Numerics;

public static class Pathfinder {
    // Trouver le meilleure Chemin!
    public static void trouverChemin(BaseUnit AI, BaseUnit cible, List<Vector2> _deplacement, List<Vector2> _attackRange) {
        Vector2 _ciblePosition = cible.transform.position;
        List<Vector2> DeplacementAtteintTarget = new List<Vector2>();
        foreach (Vector2 attack in _attackRange) {
            Vector2 _posAtteintTarget = new Vector2((_ciblePosition.x - attack.x),(_ciblePosition.y - attack.y));
            if (!DeplacementAtteintTarget.Contains(_posAtteintTarget)){
                DeplacementAtteintTarget.Add(_posAtteintTarget);
            }
        }
        foreach (Vector2 move in _deplacement) {
            Tile tile = GridManager.Instance.GetTileAtPosition(move);
            if (tile !=  null){
                if(tile.Walkable){
                    foreach (Vector2 DepAT in DeplacementAtteintTarget){
                        if (move == DepAT){
                            AIScore.Instance.setTileScore(move,100);
                            //MonoBehaviour.print("La cible est attaquable à cette Position : "+ DepAT+"!");
                        }
                    }
                }
            }
        }
    }
    // Trouver le prochain Move
    public static void trouverNextMove(BaseUnit AI, BaseUnit cible, List<Vector2> _deplacement, List<Vector2> _attackRange) {
        Vector2 _ciblePosition = cible.transform.position;
        double _scoreDistTarget = 0;
        foreach(Vector2 pos in _deplacement){
            Tile tile = GridManager.Instance.GetTileAtPosition(pos);
            if (tile !=  null){
                if(tile.Walkable){
                    //Dependament du UnitType
                    switch(AI.UnitType){
                        case UnitType.Pion:
                        case UnitType.King:
                        case UnitType.SGeneral:
                            //MonoBehaviour.print("Square");
                            _scoreDistTarget = tile.GetDistance(_ciblePosition);
                            break;
                        case UnitType.Lance:
                        case UnitType.GGeneral:
                            //MonoBehaviour.print("y + 2");
                            _scoreDistTarget = tile.GetDistance(_ciblePosition);
                            break;
                        case UnitType.Knight:
                        case UnitType.Caster:
                            //MonoBehaviour.print("Losange");
                            _scoreDistTarget = tile.GetDistance(_ciblePosition);
                            break;
                        case UnitType.Rook:
                            //MonoBehaviour.print(">>");
                            _scoreDistTarget = tile.GetDistance(_ciblePosition);
                            break;
                        default:
                            break;
                    }
                    AIScore.Instance.setTileScore(pos,(_scoreDistTarget*-1));
                }
            }
        }
    }

    public static void squareDistance() {
        
    }
}
