//***************************************//
// GrassTile
// Walkable Tile
// Fait Par: Vincent
//***************************************//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : Tile
{
    [SerializeField] private Color _baseColor, _offsetColor;

    //private string _Name = "GrassTile";

    public override void init(int x, int y) {
        var isOffset = (x + y) % 2 == 1;
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
