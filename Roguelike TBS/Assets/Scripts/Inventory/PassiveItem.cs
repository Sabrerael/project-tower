using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

/// <summary>
/// An inventory item that can be picked up to add a Passive Effect to the player.
/// </summary>
/// <remarks>
/// This class should be used as a base.
/// method.
/// </remarks>
[CreateAssetMenu(menuName = ("Inventory/Passive Item"))]
public class PassiveItem : InventoryItem {
    [Tooltip("Targeting Strategy")]
    [SerializeField] protected TargetingStrategy targetingStrategy = null;
    [Tooltip("Effect Strategies")]
    [SerializeField] protected EffectStrategy[] effectStrategies = null;

    public void TriggerPassiveEffect(GameObject user) {
        AbilityData data = new AbilityData(user);
        targetingStrategy.StartTargeting(data,
            () => {
                TargetAcquired(data);
            });
    }

    private void EffectFinished() {}

    private void TargetAcquired(AbilityData data) {
        foreach (var effect in effectStrategies) {
            effect.StartEffect(data, EffectFinished);
        }
    }
}
