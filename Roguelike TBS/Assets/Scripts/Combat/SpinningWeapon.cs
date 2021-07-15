using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpinningWeapon : Weapon {
    [SerializeField] float spinSpeed = 8;
    [SerializeField] float cooldownTimer = 30;

    private bool onCooldown = false;

    public override void UseActiveAbility() {
        // Make the player and weapon spin around, damaging enemies in a circle around the player
        if (!isSwinging && Input.GetKeyDown(KeyCode.R) && !onCooldown) {

        }
    }

    private IEnumerator CooldownTimer() {
        var weaponAbilityIcon = GameObject.Find("Weapon Ability Icon");
        weaponAbilityIcon.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f,0.75f);
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTimer);

        weaponAbilityIcon.GetComponent<Image>().color = new Color(1,1,1,0.75f);
        onCooldown = false;
    }
}
