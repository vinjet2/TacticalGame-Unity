//***************************************//
// [Nom du Fichier] MoutainTile
// [Utilisation] UnWalkable Tile
// Fait Par: Vincent & Mathieu
//***************************************//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainTile : Tile
{
    void Start() 
    {
        this.setIsWall(true);
    }
}
