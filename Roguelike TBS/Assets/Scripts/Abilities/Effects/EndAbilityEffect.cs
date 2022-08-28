using System;
using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "End Ability Effect", menuName = "Abilities/Effects/End Ability")]
public class EndAbilityEffect : EffectStrategy {
    public override void StartEffect(AbilityData data, Action finished) {
        // TODO I don't like how this works. Either rename it to be clearer or rework it
        Character character = data.GetUser().GetComponent<Character>();
        Health health = data.GetUser().GetComponent<Health>();
        health.onDamageTaken += character.EndAbilityCoroutine;

        finished();
    }
}
