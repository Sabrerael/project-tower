using System;
using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Effect", menuName = "Abilities/Effects/Health")]
public class HealthEffect : EffectStrategy {
    [SerializeField] int healthChange;

    public override void StartEffect(AbilityData data, Action finished) {
        foreach(var target in data.GetTargets()) {
            var health = target.GetComponent<Health>();
            if (health) {
                if (healthChange > 0) {
                    health.Heal(healthChange);
                } else {
                    health.TakeDamage(data.GetUser(), healthChange);
                }
            }
        }
        finished();
    }
}