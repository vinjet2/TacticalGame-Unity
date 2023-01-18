/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using Assets.Scripts.Terrains.Tile;

// Tarodev Pathfinding Code (https://github.com/Matthew-J-Spencer/Pathfinding/blob/main/_Scripts/Pathfinding.cs)
public class AStar : MonoBehaviour
{
    public static void FindPath(Tile startNode, Tile targetNode) {
        var toSearch = new List<Tile>() { startNode };
        var processed = new List<Tile>(); // list de Position déja calculé 

        while(toSearch.Any()) {
            var current = toSearch[0];
            // Trouver la meilleure option
            //foreach(var t in toSearch)
            //    if (t.F < current || t.F == current.F && t.H < current.H) current = t;

            processed.Add(current);
            toSearch.Remove(current);

            //Check target obtained 
            if (current == targetNode) {
                var currentPathTile = targetNode;
                var path = new List<Tile>();
                var count = 100;
                while (currentPathTile != startNode) {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                    count--;
                    //if (count < 0) throw new Exception();
                    Debug.Log("adding" + currentPathTile);
                }
                //return path;
            }
            //Pour chaque options de mouvement possible ET si la liste processed ne la contient pas 
            foreach (var neighbor in current.Neighbors.Where(t => t.Walkable && !processed.Contains(t))) {
                var inSearch = toSearch.Contains(neighbor);

                var costToNeighbor = current.G + current.GetDistance(neighbor);

                if (!inSearch || costToNeighbor < neighbor.G) {
                    neighbor.SetG(costToNeighbor);
                    neighbor.SetConnection(current);

                    if (!inSearch) {
                        neighbor.SetH(neighbor.GetDistance(targetNode));
                        toSearch.Add(neighbor);
                    }
                }
            }
        }
        //return null;
    }
}*/
