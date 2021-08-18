using UnityEngine;

[CreateAssetMenu(fileName = "StatPercentIncrease", menuName = "Class Ability/Activate Ability Stat Percent Increase")]
public class ActiveAbilityPercentStatIncrease : ClassAbility {
    [SerializeField] Stat stat = Stat.Health;
    [SerializeField] int statIncrease = 1;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this ability. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        //Need to include the stat and make modifications
        user.GetComponent<Character>().ModifyAbilityBonusPercentage(stat, statIncrease);
    }
} 
