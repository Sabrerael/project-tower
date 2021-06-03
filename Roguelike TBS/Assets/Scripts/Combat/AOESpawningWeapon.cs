using System.Collections;
using UnityEngine;

public class AOESpawningWeapon : Weapon {
    [SerializeField] GameObject aoeToSpawn = null;
    [SerializeField] float cooldownTimer = 30;

    private bool onCooldown = false;

    public override void UseActiveAbility() {
        // Swings the weapon, spawns an AOE that affects the enemies within it
        if (!isSwinging && Input.GetKeyDown(KeyCode.R) && !onCooldown) {
            var aoe = Instantiate(aoeToSpawn, transform.position, Quaternion.identity);
            aoe.GetComponent<DrainingAOE>().SetInstigator(wielder.gameObject);
        }
    }

    private IEnumerator CooldownTimer() {
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTimer);

        onCooldown = false;
    }
}
