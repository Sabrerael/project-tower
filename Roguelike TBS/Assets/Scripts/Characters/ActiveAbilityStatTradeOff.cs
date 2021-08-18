using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatTradeOff", menuName = "Class Ability/Activate Ability Stat Trade Off")]
public class ActiveAbilityStatTradeOff : ClassAbility {
    [SerializeField] Stat statToIncrease = Stat.Health;
    [SerializeField] int statIncrease = 1;
    [SerializeField] Stat statToDecrease = Stat.Attack;
    [SerializeField] int statDecrease = 1;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this ability. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        user.GetComponent<Character>().ModifyAbilityBonusPercentage(statToIncrease, statIncrease);
        user.GetComponent<Character>().ModifyAbilityBonusPercentage(statToDecrease, -1 * statIncrease);
    }
}
