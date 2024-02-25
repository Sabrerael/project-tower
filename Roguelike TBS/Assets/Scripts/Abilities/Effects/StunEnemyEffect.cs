using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Stun Enemy Effect", menuName = "Abilities/Effects/Stun Enemy")]
public class StunEnemyEffect : EffectStrategy {
    [SerializeField] float totalTimeOfEffect;
    [SerializeField] float tickRate;

    public override void StartEffect(AbilityData data, Action finished) {
        foreach(var target in data.GetTargets()) {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy) {
                enemy.StartCoroutine(StunTimer(enemy, finished));
            }
        }

        finished();
    }

    private IEnumerator StunTimer(Enemy enemy, Action finished) {
        float timeElapsed = 0;
        enemy.TogglePath();
        while (timeElapsed <= totalTimeOfEffect || totalTimeOfEffect == -1) {
            yield return new WaitForSeconds(tickRate);
            timeElapsed += tickRate;
        }
        enemy.TogglePath();

        finished();
    }
}
