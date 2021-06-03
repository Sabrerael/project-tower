using System.Collections;
using UnityEngine;

public class ProjectileSpawningWeapon : Weapon {
    [SerializeField] ThrownItem projectile = null;
    [SerializeField] float cooldownTimer = 30f;
    [SerializeField] AudioClip sfx = null;
    
    private bool onCooldown = false;

    public override void UseActiveAbility() {
        // Swings the weapon, spawning a projectile
        if (!isSwinging && Input.GetKeyDown(KeyCode.R) && !onCooldown) {
            ThrownItem projectileInstance = Instantiate(projectile, wielder.transform.position, Quaternion.identity);
            projectileInstance.SetWielder(wielder);

            var mouse = Input.mousePosition;
            var screenPoint = Camera.main.WorldToScreenPoint(projectileInstance.transform.localPosition);
            var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
            var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

            projectileInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
            projectileInstance.GetComponent<Rigidbody2D>().velocity = new Vector3(xRatio * 20, yRatio * 20, 0);

            if (sfx) { AudioSource.PlayClipAtPoint(sfx, wielder.transform.position); }
            StartCoroutine(CooldownTimer());
        }
    }

    private IEnumerator CooldownTimer() {
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTimer);

        onCooldown = false;
    }
}
