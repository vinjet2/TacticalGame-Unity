/*  ChangeHeroTeam
    Permet de modifier l'équipe d'unité allier utiliser par le joueur durant le combat
    Fait Par: Vincent Comptois et Mathieu Beaupré
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHeroTeam : MonoBehaviour
{
    public static ChangeHeroTeam Instance;
    private static string herochoice;

    private void Awake()
    {
        Instance = this;
    }

    public void changeTeam(int i)
    {
        if (i == 0)
        {
            herochoice = "Heroes";
        }
        else if (i == 1)
        {
            herochoice = "Team1";
        }
    }

    public string getHeroChoice() { return herochoice; }
}
