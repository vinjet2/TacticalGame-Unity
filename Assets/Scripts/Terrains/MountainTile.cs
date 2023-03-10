//***************************************//
// MoutainTile
// UnWalkable Tile
// Fait Par: Vincent
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
