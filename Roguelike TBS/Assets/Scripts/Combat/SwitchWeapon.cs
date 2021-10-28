using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWeapon : Weapon {
    [SerializeField] float cooldownTimer = 5;
    [SerializeField] WeaponConfig otherSide = null;
    [SerializeField] WeaponConfig activeWeapon = null;

    private bool onCooldown = false;

    public override void UseActiveAbility() {
        // Swings the weapon, spawns an AOE that affects the enemies within it
        if (weaponState != WeaponState.Swinging1 && 
            weaponState != WeaponState.Swinging2 && 
            weaponState != WeaponState.Swinging3 && Input.GetKeyDown(KeyCode.R) && !onCooldown) {
            if (activeWeapon == weaponConfig) {
                activeWeapon = otherSide;

            } else {
                activeWeapon = weaponConfig;
            }

            //Code to swing weapon around
            wielder.EquipWeapon(activeWeapon);
        }
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