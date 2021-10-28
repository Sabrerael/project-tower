using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "New Stat Trade Off Weapon", order = 0)]
public class TradeOffStatWeapon : WeaponConfig {
    [SerializeField] Stat statToIncrease = Stat.Health;
    [SerializeField] int statIncrease = 1;
    [SerializeField] Stat statToDecrease = Stat.Attack;
    [SerializeField] int statDecrease = 1;

    public override void HandlePassive(GameObject user) {
        user.GetComponent<Character>().ModifyPassiveBonusPercentage(statToIncrease, statIncrease);
        user.GetComponent<Character>().ModifyPassiveBonusPercentage(statToDecrease, -1 * statIncrease);
    }
}