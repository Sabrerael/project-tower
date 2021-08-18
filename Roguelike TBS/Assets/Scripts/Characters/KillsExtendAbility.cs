using UnityEngine;

[CreateAssetMenu(fileName = "KillsExtendAbility", menuName = "Class Ability/KillsExtendAbility")]
public class KillsExtendAbility : ClassAbility {
    [SerializeField] float timerIncrease = 1f;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this ability. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        user.GetComponent<Barbarian>().AddTimeToAbility(timerIncrease);
    }
} 
