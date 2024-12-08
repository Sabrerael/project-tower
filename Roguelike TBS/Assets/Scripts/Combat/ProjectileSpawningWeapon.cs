﻿//TODO REMOVE?

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ProjectileSpawningWeapon : Weapon {
    [SerializeField] ThrownItem projectile = null;
    [SerializeField] float cooldownTimer = 30f;
    [SerializeField] AudioClip sfx = null;
    
    private bool onCooldown = false;

    public override void UseActiveAbility() {
        // Swings the weapon, spawning a projectile
        if (weaponState != WeaponState.Swinging1 && 
            weaponState != WeaponState.Swinging2 && 
            weaponState != WeaponState.Swinging3 &&
            !onCooldown) {
            ThrownItem projectileInstance = Instantiate(projectile, wielder.transform.position, Quaternion.identity);
            projectileInstance.SetWielder(wielder);

            var mouse = Mouse.current.position.ReadValue();
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
        var weaponAbilityIcon = GameObject.Find("Weapon Ability Icon");
        weaponAbilityIcon.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f,0.25f);
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTimer);

        weaponAbilityIcon.GetComponent<Image>().color = new Color(1,1,1,0.75f);
        onCooldown = false;
    }
}
