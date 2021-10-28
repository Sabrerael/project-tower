using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "New Shielding Weapon", order = 0)]
public class ShieldingWeapon : WeaponConfig {
    [SerializeField] int numerator = 1;
    [SerializeField] int denominator = 20;
    
    public override void HandlePassive(GameObject user) {
        user.GetComponent<Health>().onDamageTaken += WillShield;
    }

    public int WillShield(int damage) {
        var chance = Random.Range(1, denominator);

        if (chance <= numerator) {
            return Mathf.CeilToInt(damage/2);
        }
        return damage;
    }
}