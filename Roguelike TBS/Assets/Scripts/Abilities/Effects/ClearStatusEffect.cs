using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Clear Status Effect", menuName = "Abilities/Effects/Clear Status")]
public class ClearStatusEffect : EffectStrategy {
    public override void StartEffect(AbilityData data, Action finished)
    {
        Debug.Log("TODO Finish implementing this when I make statuses :p");
        finished();
    }
}
