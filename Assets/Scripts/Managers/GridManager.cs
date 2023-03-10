//***************************************//
// Grid Manager
// La création de la map et le lien Entre les UI et leur Object (Deplacement & AttackRange)
// Fait Par: Vincent
//***************************************//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class GridManager : MonoBehaviour {
    public static GridManager Instance;

    [SerializeField] private int _width ,_height;

    [SerializeField] private Tile _grassTile, _mountainTile;

    [SerializeField] private Transform _cam;


    private Dictionary<Vector2, Tile> _tiles;

    void Awake() {
        Instance = this; 
    }

  
    // Creation de Grid & Map Variation
    public void GenerateGrid() {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                //A faire - Systeme de Poids pour augmenter les chances que 2 montagnes Ou 2 Cours d'eau soient ensembles.
                var randomTile = Random.Range(0, 10) == 5 ? _mountainTile : _grassTile;
                var spawnedTile = Instantiate(randomTile, new Vector3(x,y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                spawnedTile.init(x,y);

                _tiles[new Vector2(x,y)] = spawnedTile;
            }
        }
        _cam.transform.position = new Vector3((float)_width/2 -0.5f, (float)_height/2 -0.5f, -10); // x,y,z (-10 position Standart)

        GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public Tile GetHeroSpawnTile() {
        return _tiles.Where(t=>t.Key.x < _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetEnemySpawnTile() {
        return _tiles.Where(t=>t.Key.x > _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetTileAtPosition(Vector2 pos) {
        if(_tiles.TryGetValue(pos, out var tile)) {
            return tile;
        }
       return null;
    }

    // Active le UI de deplacement
    public void ShowMovement(List<Vector2> _deplacement) {
        if (_deplacement == null){
            //print("Refresh UI Deplacement");
            foreach(KeyValuePair<Vector2,Tile> tile in _tiles){
                tile.Value.ActivateMovement(false);
            }
        }
        else {
            foreach(Vector2 tuile in _deplacement){
                Tile tile = GetTileAtPosition(tuile);
                if (tile != null){
                    if (tile.Walkable)
                    tile.ActivateMovement(true);
                }
            }     
        }
    }

    // Active le UI d' Attaque Range
    public void ShowAttackRange(List<Vector2> _attackRange) {
        if (_attackRange == null) {
            //print("Refresh UI AttackRange");
            foreach(KeyValuePair<Vector2,Tile> tile in _tiles){
                tile.Value.ActivateAttackRange(false);
            }
        }
        else {
            foreach(Vector2 tuile in _attackRange){
                Tile tile = GetTileAtPosition(tuile);
                if (tile != null){
                    tile.ActivateAttackRange(true);
                }
            }
        }
    }
}
