/*  GameUIScript
    Permet de gérer le UI de combat qui montre le nom des unités et leurs statistiques.
    Fait Par: Vincent Comptois et Mathieu Beaupré
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIScript : MonoBehaviour
{
    public static GameUIScript Instance;

    public TextMeshProUGUI unitNameText, unitHP, unitAtk, unitDef, unitSpd, unitCrit;

    public Canvas GameUI;

    private void Awake()
    {
        Instance = this;
        GameUI = this.GetComponent<Canvas>();
        this.setAllDisable();
    }

    public void setUnit(BaseUnit unit, bool active)
    {
        GameUI.enabled = active;
        if (unit != null){
            if (unit.Faction == Faction.Hero)
            {
                unitNameText.color = Color.green;
            }
            else
            {
                unitNameText.color = Color.red;
            }
            unitNameText.text = unit.getUnitName();
            unitHP.text = "HP: " + unit.getCurrentHP().ToString() + " / " + unit.getMaxHP().ToString();
            unitAtk.text = "Atk: " + unit.getAtk().ToString();
            unitDef.text = "Def: " + unit.getDef().ToString();
            unitSpd.text = "Speed: " + unit.getSpeed().ToString();
            unitCrit.text = "Crit Chance: " + unit.getCritChance().ToString();
        }
    }

    public void setAllDisable()
    {
        GameUI.enabled = false;
    }
}
