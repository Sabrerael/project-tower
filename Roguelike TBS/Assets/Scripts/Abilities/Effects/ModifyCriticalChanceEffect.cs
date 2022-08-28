using System;
using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "Modify Critical Chance Effect", menuName = "Abilities/Effects/Modifiy Critical Chance")]
public class ModifyCriticalChanceEffect : EffectStrategy {
    [SerializeField] int critChangeModification;

    public override void StartEffect(AbilityData data, Action finished) {
        Character character = data.GetUser().GetComponent<Character>();
        character.ModifyCriticalChance(critChangeModification);

        finished();
    }
}
