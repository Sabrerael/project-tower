using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Single Enemy Targeting", menuName = "Abilities/Targeting/Single Enemy")]
public class SingleEnemyTargeting : TargetingStrategy {
    public override void StartTargeting(AbilityData data, Action finished) {
        
        Character character = data.GetUser().GetComponent<Character>();
        if (character.GetCurrentRoom().GetEnemiesParent().transform.childCount == 0) {
            finished();
            return;
        }

        data.SetTargets(new GameObject[]{character.GetCurrentRoom().GetEnemiesParent().transform.GetChild(0).gameObject});
        finished();
    }
}
