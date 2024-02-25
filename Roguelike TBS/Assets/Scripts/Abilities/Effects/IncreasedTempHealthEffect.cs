using System;
using System.Collections;
using RPG.Stats;
using UnityEngine;

// Currently will only work with the Paladin. Will need to rework if another item can give temporary health
[CreateAssetMenu(fileName = "Increased Temporary Health Effect", menuName = "Abilities/Effects/Increased Temporary Health")]
public class IncreasedTempHealthEffect : EffectStrategy {
    [SerializeField] int temporaryHealthIncrease;

    public override void StartEffect(AbilityData data, Action finished) {
        var paladin = data.GetUser().GetComponent<Paladin>();
        paladin?.ModifyTemporaryHealthAmount(temporaryHealthIncrease);
        finished();
    }
}
