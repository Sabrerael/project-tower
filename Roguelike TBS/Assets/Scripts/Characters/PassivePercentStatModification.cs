using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatPercentIncrease", menuName = "Class Ability/Passive Stat Percent Increase")]
public class PassivePercentStatModification : ClassAbility {
    [SerializeField] Stat stat = Stat.Health;
    [SerializeField] int statIncrease = 1;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this ability. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        user.GetComponent<Character>().ModifyPassiveBonusPercentage(stat, statIncrease);
    }
}
