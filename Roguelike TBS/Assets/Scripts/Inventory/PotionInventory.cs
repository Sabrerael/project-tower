using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public class PotionInventory : MonoBehaviour {
    [Tooltip("Base Number of Potions")]
    [SerializeField] int numOfPotions = 2;
    [Tooltip("Targeting Strategy")]
    [SerializeField] TargetingStrategy targetingStrategy = null;
    [Tooltip("Effect Strategies")]
    [SerializeField] List<EffectStrategy> effectStrategies = null;

    private Health health;
    private int potionsForUse;

    private void Awake() {
        health = GetComponent<Health>();
        potionsForUse = numOfPotions;
    }

    public void AddEffect(EffectStrategy effect) {
        effectStrategies.Add(effect);
    }

    public void ResetInventory() {
        potionsForUse = numOfPotions;
    }

    public void UsePotion() {
        if (numOfPotions == 0) { return; }
        AbilityData data = new AbilityData(gameObject);
        targetingStrategy.StartTargeting(data,
            () => {
                foreach (var effect in effectStrategies) {
            effect.StartEffect(data, EffectFinished);
        }
            });
        potionsForUse--;
    }

    private void EffectFinished() {}
}
