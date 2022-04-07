using System;
using System.Collections;
using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Over Time Effect", menuName = "Abilities/Effects/Health Over Time")]
public class HealthOverTimeEffect : EffectStrategy {
    [SerializeField] int healthChange;
    [SerializeField] float totalTimeOfEffect;
    [SerializeField] float tickRate;

    public override void StartEffect(AbilityData data, Action finished) {
        foreach(var target in data.GetTargets()) {
            var health = target.GetComponent<Health>();
            if (health) {
                health.StartCoroutine(EffectHealthOverTime(data.GetUser(), health, finished));
            }
        }
    }

    private IEnumerator EffectHealthOverTime(GameObject user, Health health, Action finished) {
        float timeElapsed = 0;
        while (timeElapsed <= totalTimeOfEffect || totalTimeOfEffect == -1) {
            if (healthChange > 0) {
                health.Heal(healthChange);
            } else {
                health.TakeDamage(user, healthChange);
            }
            yield return new WaitForSeconds(tickRate);
            timeElapsed += tickRate;
        }
        finished();
    }
}
