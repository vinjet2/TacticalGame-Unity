//***************************************//
// [Nom du Fichier] SGeneral
// [Utilisation] Les Deplacements & Attack Range
// Fait Par: Vincent & Mathieu
//***************************************//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGeneral : BaseUnit
{
    // Constructeur 
    public SGeneral(double atk, double def, float critChance, double maxHP, double spd, string unitName):
        base(atk, def, critChance, maxHP, spd, unitName)
    {

    }

    // Deplacement possible
    //(x-1,y+1) (x,y+1)  (x+1,y+1)
    //(x-1,y)   (X,Y)    (x+1,y)
    //(x-1,y-1) (x,y-1)  (x+1,y-1)
    public override List<Vector2> Deplacement(Vector2 pos) {
        List<Vector2> _deplacement = new List<Vector2>();
        _deplacement.Add(new Vector2(pos.x,pos.y));
        _deplacement.Add(new Vector2(pos.x,pos.y+1));
        _deplacement.Add(new Vector2(pos.x,pos.y-1));
        _deplacement.Add(new Vector2(pos.x+1,pos.y+1));
        _deplacement.Add(new Vector2(pos.x+1,pos.y));
        _deplacement.Add(new Vector2(pos.x+1,pos.y-1));
        _deplacement.Add(new Vector2(pos.x-1,pos.y+1));
        _deplacement.Add(new Vector2(pos.x-1,pos.y));
        _deplacement.Add(new Vector2(pos.x-1,pos.y-1));
        return _deplacement;
    }

    // Healing possible
    //(x-1,y+1) (x,y+1)  (x+1,y+1)
    //(x-1,y)   (X,Y)    (x+1,y)
    //(x-1,y-1) (x,y-1)  (x+1,y-1)
    public override List<Vector2> AttackRange(Vector2 pos) {
        List<Vector2> _attackRange = new List<Vector2>();
        _attackRange.Add(new Vector2(pos.x,pos.y+1));
        _attackRange.Add(new Vector2(pos.x,pos.y-1));
        _attackRange.Add(new Vector2(pos.x+1,pos.y+1));
        _attackRange.Add(new Vector2(pos.x+1,pos.y));
        _attackRange.Add(new Vector2(pos.x+1,pos.y-1));
        _attackRange.Add(new Vector2(pos.x-1,pos.y+1));
        _attackRange.Add(new Vector2(pos.x-1,pos.y));
        _attackRange.Add(new Vector2(pos.x-1,pos.y-1));
        return _attackRange;
    }
}
