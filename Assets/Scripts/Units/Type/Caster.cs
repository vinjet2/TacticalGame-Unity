//***************************************//
// [Nom du Fichier] Caster
// [Utilisation] Les Deplacements & Attack Range
// Fait Par: Vincent & Mathieu
//***************************************//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caster : BaseUnit
{
    // Constructeur
    public Caster(double atk, double def, float critChance, double maxHP, double spd, string unitName) :
        base(atk, def, critChance, maxHP, spd, unitName)
    { }
    
    //Mouvement possible
    //                          (x,y+1)
    //                (x-1,y)   (X,Y)    (x+1,y)
    //                          (x,y-1)
    public override List<Vector2> Deplacement(Vector2 pos) {
        List<Vector2> _deplacement = new List<Vector2>();
        _deplacement.Add(new Vector2(pos.x,pos.y));
        _deplacement.Add(new Vector2(pos.x-1,pos.y));
        _deplacement.Add(new Vector2(pos.x,pos.y+1));
        _deplacement.Add(new Vector2(pos.x,pos.y-1));
        _deplacement.Add(new Vector2(pos.x+1,pos.y));
        return _deplacement;
    }
    //Range possible ((X,Y) non possible)
    //                           (x,y+3)
    //                 (x-1,y+2) (x,y+2) (x+1,y+2)
    //         (x-2,y+1)                        (x+2,y+1)
    // (x-3,y) (x-2,y)           (X,Y)          (x+2,y) (x+3,y)
    //         (x-2,y-1)                        (x+2,y-1)
    //                 (x-1,y-2) (x,y-2) (x+1,y-2)
    //                           (x,y-3)
    public override List<Vector2> AttackRange(Vector2 pos) {
        List<Vector2> _attackRange = new List<Vector2>();
        _attackRange.Add(new Vector2(pos.x+2,pos.y));
        _attackRange.Add(new Vector2(pos.x+2,pos.y-1));
        _attackRange.Add(new Vector2(pos.x+1,pos.y+2));
        _attackRange.Add(new Vector2(pos.x+1,pos.y-2));
        _attackRange.Add(new Vector2(pos.x,pos.y+3));
        //_attackRange.Add(new Vector2(pos.x,pos.y+2));
        //_attackRange.Add(new Vector2(pos.x,pos.y-2));
        _attackRange.Add(new Vector2(pos.x,pos.y-3));
        _attackRange.Add(new Vector2(pos.x-1,pos.y+2));
        _attackRange.Add(new Vector2(pos.x-1,pos.y-2));
        _attackRange.Add(new Vector2(pos.x-2,pos.y+1));
        //_attackRange.Add(new Vector2(pos.x-2,pos.y));
        _attackRange.Add(new Vector2(pos.x-2,pos.y-1));
        _attackRange.Add(new Vector2(pos.x-3,pos.y));

        return _attackRange;
    }
}
