using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "HealPlayer", menuName = "Class Ability/Heal Player")]
public class HealPlayer : ClassAbility {
    [SerializeField] int healAmount = 1;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this ability. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        user.GetComponent<Health>().Heal(healAmount);
    }
}
