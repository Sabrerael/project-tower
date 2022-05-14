using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability Stat Change Effect", menuName = "Abilities/Effects/Ability Stat Change")]
public class AbilityStatChangeEffect : EffectStrategy {
    [SerializeField] bool additiveChange;
    [SerializeField] Stat statToChange;
    [Tooltip("Flat numbers if Additive, percent change if not")]
    [SerializeField] int changeAmount;

    public override void StartEffect(AbilityData data, Action finished) {
        foreach(var target in data.GetTargets()) {
            var character = target.GetComponent<Character>();
            if (additiveChange) {
                character.ModifyPassiveBonusAddition(statToChange, changeAmount);
            } else {
                character.ModifyAbilityBonusPercentage(statToChange, changeAmount);
            }
        }
        finished();
    }
}
