using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Alter Enemy Movement Effect", menuName = "Abilities/Effects/Alter Enemy Movement")]
public class AlterEnemyMovementEffect : EffectStrategy {
    [SerializeField] float movementAdjustment = 0f;

    public override void StartEffect(AbilityData data, Action finished) {
        foreach(var target in data.GetTargets()) {
            if (target.GetComponent<Enemy>()) {
                target.GetComponent<Enemy>().ModifyMovementSpeed(movementAdjustment);
            }
        }
        finished();
    }

}
