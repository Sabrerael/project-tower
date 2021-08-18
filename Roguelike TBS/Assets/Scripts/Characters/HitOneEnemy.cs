using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "HitOneEnemy", menuName = "Class Ability/Hit One Enemy")]
public class HitOneEnemy : ClassAbility {
    [SerializeField] int damage = 1;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this ability. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        var enemyHealths = user.GetComponent<Character>().GetCurrentRoom().GetEnemiesParent().transform.GetComponentsInChildren<Health>();
        enemyHealths[0].TakeDamage(user, damage);
    }   
}
