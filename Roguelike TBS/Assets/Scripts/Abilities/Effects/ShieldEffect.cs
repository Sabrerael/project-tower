using System;
using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield Effect", menuName = "Abilities/Effects/Add Shield")]
public class ShieldEffect : EffectStrategy {
    public override void StartEffect(AbilityData data, Action finished) {
        foreach(var target in data.GetTargets()) {
            var health = target.GetComponent<Health>();
            if (health) {
                health.SetShieldUp();
            }
        }
        finished();
    }
}
