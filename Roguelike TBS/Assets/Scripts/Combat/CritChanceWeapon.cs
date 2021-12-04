using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CritChanceWeapon : Weapon {
    [SerializeField] float cooldownTimer = 30;

    private bool onCooldown = false;
    private bool instantCritActive = false;

    public override void UseActiveAbility() {
        // Swings the weapon, spawns an AOE that affects the enemies within it
        if (/*weaponState != WeaponState.Swinging1 && 
            weaponState != WeaponState.Swinging2 && 
            weaponState != WeaponState.Swinging3 &&*/
            !onCooldown) {
            instantCritActive = true;
        }
    }

    public override int GetWeaponDamage() {
        if (instantCritActive) { return weaponConfig.GetWeaponDamage() * 2;}

        return weaponConfig.GetWeaponDamage();
    }

    private IEnumerator CooldownTimer() {
        var weaponAbilityIcon = GameObject.Find("Weapon Ability Icon");
        weaponAbilityIcon.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f,0.25f);
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTimer);

        weaponAbilityIcon.GetComponent<Image>().color = new Color(1,1,1,0.75f);
        onCooldown = false;
    }
}