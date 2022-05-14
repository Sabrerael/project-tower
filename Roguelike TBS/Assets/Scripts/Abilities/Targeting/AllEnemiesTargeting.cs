using System;
using UnityEngine;

[CreateAssetMenu(fileName = "All Enemies Targeting", menuName = "Abilities/Targeting/All Enemies")]
public class AllEnemiesTargeting : TargetingStrategy {
    public override void StartTargeting(AbilityData data, Action finished) {
        
        Character character = data.GetUser().GetComponent<Character>();
        Transform enemyParent = character.GetCurrentRoom().GetEnemiesParent().transform;
        GameObject[] targets = null;

        if (enemyParent.childCount == 0) {
            finished();
            return;
        }

        for (int i = 0; i < enemyParent.childCount; i++) {
            targets.SetValue(enemyParent.GetChild(i), i);
        }
        data.SetTargets(targets);
        finished();
    }
}
