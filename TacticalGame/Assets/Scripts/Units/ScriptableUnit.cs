//***************************************//
// [Nom du Fichier] Scriptable Unit
// [Utilisation] Garder en mémoire les informations Faction & UnitType
// Fait Par: Vincent
//***************************************//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit",menuName = "Scriptable Unit")]

public class ScriptableUnit : ScriptableObject {
   public Faction Faction;
   public UnitType UnitType;
   public BaseUnit UnitPrefab;
}

public enum Faction {
    Hero = 0,
    Enemy = 1
}

public enum UnitType {
    Pion = 0,
    Rook = 1,
    Caster = 2,
    Lance = 3,
    Knight = 4,
    GGeneral = 5,
    SGeneral = 6,
    King = 7
}