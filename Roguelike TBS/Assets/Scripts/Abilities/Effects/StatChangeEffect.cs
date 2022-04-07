using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat Change Effect", menuName = "Abilities/Effects/Stat Change")]
public class StatChangeEffect : EffectStrategy {
    [SerializeField] bool additiveChange;
    [SerializeField] Stat statToChange;
    [SerializeField] int timeForChange;
    [Tooltip("Flat numbers if Additive, percent change if not")]
    [SerializeField] int changeAmount;

    public override void StartEffect(AbilityData data, Action finished) {
        foreach(var target in data.GetTargets()) {
            if (timeForChange > 0) { 
                var buffs = target.GetComponent<Buffs>();

                if (additiveChange) {
                    buffs.StartAdditivetiveBuffTimer(statToChange, timeForChange, changeAmount);
                } else {
                    buffs.StartMultiplicativeBuffTimer(statToChange, timeForChange, changeAmount);
                }
            } else { // TODO Should these be handled by the Buffs component as well?
                var inventory = target.GetComponent<Inventory>();

                if (additiveChange) {
                    inventory.ModifyPassiveBonusAddition(statToChange, changeAmount);
                } else {
                    inventory.ModifyPassiveBonusPercentage(statToChange, changeAmount);
                }
            }
        }
        finished();
    }
}
