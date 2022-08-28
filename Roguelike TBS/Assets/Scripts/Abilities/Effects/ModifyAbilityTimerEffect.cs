using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Modify Ability Timer Effect", menuName = "Abilities/Effects/Modify Ability Timer")]
public class ModifyAbilityTimerEffect : EffectStrategy {
    [SerializeField] float timerChange;

    public override void StartEffect(AbilityData data, Action finished) {
        Character character = data.GetUser().GetComponent<Character>();
        // TODO This should be usable by more than just the Barbarian. Change the override to be a full function in the Character class
        character.AddTimeToAbility(timerChange);

        finished();
    }
}
