using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "HitAllEnemies", menuName = "Class Ability/Hit All Enemies")]
public class HitAllEnemies : ClassAbility {
    [SerializeField] int damage = 1;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this ability. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        var enemyHealths = user.GetComponent<Character>().GetCurrentRoom().GetEnemiesParent().transform.GetComponentsInChildren<Health>();
        foreach (var enemyHealth in enemyHealths) {
            enemyHealth.TakeDamage(user, damage);
        }
    }   
}
