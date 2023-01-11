//***************************************//
// [Nom du Fichier] CombatSystem
// [Utilisation] Le Calcul de Combat , La Formule de combat & Verification si le Unit est InRange
// Fait Par: Vincent & Mathieu
//***************************************//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatSystem
{

    public static void CalculCombat(bool _combat, bool _fictif, BaseUnit _attaquant, BaseUnit _defendant)
    {
        double _degat = 0, _degatRecu = 0, remainingHpAtk = _attaquant.getCurrentHP();

        _degat = FormuleDeCombat(_attaquant, _defendant);
        if (!_fictif)
        {
            MonoBehaviour.print(_attaquant.getUnitName() + " Attaque " + _defendant.getUnitName() + " de " + _degat + "Hp.");
        }

        // Check si Defendant est mort
        double _remainingHpDef = _defendant.getCurrentHP() - _degat;
        if (_remainingHpDef <= 0)
        {
            if (_fictif)
            { // Ajout 100 points au Score
                AIScore.Instance.setTargetScore(_defendant, (100 + _degat));

            }
            else  if (_combat){
                _defendant.takesDamage(_degat);
                _defendant.setDeath();
                MonoBehaviour.print(_defendant.getUnitName() + " is Dead");
            }
        }
        else
        {
            if (_fictif)
            { // Ajout le Score
                AIScore.Instance.setTargetScore(_defendant, _degat);
            }
            else if (_combat)
            {
                _defendant.takesDamage(_degat);
            }
            // SI l'attaquant est dans l'attackrange du Defendant
            List<Vector2> _defAtkRange = _defendant.AttackRange(_defendant.transform.position);
            Vector2 attackpos = _attaquant.transform.position;
            //MonoBehaviour.print("Inrange : " + Inrange + "Pos" + attackpos);
            if (unitInRange(_defAtkRange, attackpos))
            {
                _degatRecu = FormuleDeCombat(_defendant, _attaquant);// CalculCombat() invers�
                if (!_fictif)
                {
                    MonoBehaviour.print(_defendant.getUnitName() + " ContreAttaque " + _attaquant.getUnitName() + " de " + _degatRecu + "Hp.");
                }
                // Check si l'attaquant est encore vivant
                remainingHpAtk = _attaquant.getCurrentHP() - _degatRecu;
                if (remainingHpAtk <= 0)
                {
                    if (_fictif)
                    { // Enl�ve (50 points + degat) au score
                        AIScore.Instance.setTargetScore(_defendant, ((50 + _degat) * -1));
                    }
                    else if (_combat)
                    {
                        _attaquant.takesDamage(_degatRecu);
                        _attaquant.setDeath();
                        MonoBehaviour.print(_attaquant.getUnitName() + " is Dead");
                    }
                }
                else
                {
                    if (_fictif)
                    { // Enl�ve le score
                        AIScore.Instance.setTargetScore(_defendant, (_degat * -1));
                    }
                    else if (_combat)
                    {
                        _attaquant.takesDamage(_degatRecu);
                    }
                }
            }
        }
        MenuManager.Instance.ShowCombatUI(_degat, _degatRecu, _remainingHpDef, remainingHpAtk);
    }

    public static double FormuleDeCombat(BaseUnit _attaquant, BaseUnit _defendant)
    {
        double _degat;
        _degat = (_attaquant.getAtk() * _attaquant.getCritChance() + 2) - _defendant.getDef();
        return _degat;
    }

    public static bool unitInRange(List<Vector2> _attackrange, Vector2 _enemyPos)
    {
        bool _inRange = false;
        foreach (Vector2 attack in _attackrange)
        {
            if (attack == _enemyPos)
            {
                _inRange = true;
            }
        }
        return _inRange;
    }
}