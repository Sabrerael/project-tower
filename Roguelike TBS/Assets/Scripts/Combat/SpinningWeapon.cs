using System.Collections;
using UnityEngine;

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
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTimer);

        onCooldown = false;
    }
}
