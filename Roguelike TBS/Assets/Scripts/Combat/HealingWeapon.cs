using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "New Healing Weapon", order = 0)]
public class HealingWeapon : WeaponConfig {
    [SerializeField] ActivatationPoint activatationPoint = ActivatationPoint.OnRoomClear;
    [SerializeField] int healthPoints = 5;
    
    public override void HandlePassive(GameObject user) {
        user.GetComponent<Character>().onRoomClear += Heal;
    }

    public void Heal(GameObject user) {
        user.GetComponent<Health>().Heal(healthPoints);
    }
}