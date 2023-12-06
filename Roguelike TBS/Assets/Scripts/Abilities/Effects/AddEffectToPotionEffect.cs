using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Add Effect to Potion Effect", menuName = "Abilities/Effects/Add Effect To Potion")]
public class AddEffectToPotionEffect : EffectStrategy {
    [SerializeField] EffectStrategy effectToAdd;

    public override void StartEffect(AbilityData data, Action finished) {
        foreach(var target in data.GetTargets()) {
            var potionInventory = target.GetComponent<PotionInventory>();
            potionInventory.AddEffect(effectToAdd);
        }
        finished();
    }
}
