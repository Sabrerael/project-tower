using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashingWeapon : Weapon {
    [SerializeField] float dashTimer = 1f;
    [SerializeField] float dashSpeed = 4;
    [SerializeField] float cooldownTimer = 30f;
    [SerializeField] float xMin, xMax, yMin, yMax = 0;
    [SerializeField] AudioClip sfx = null;

    private bool inAction = false;
    private bool onCooldown = false;
    private float xRatio, yRatio = 0;
    private float timer = 0;

    public override void UseActiveAbility() {
        // Swings the weapon, dashing forward through enemies
        // Need to either enable the weapon's collider or add one to act as the collider for the attack
        // Need to clamp the end positions
        
        if (weaponState != WeaponState.Swinging1 && 
            weaponState != WeaponState.Swinging2 && 
            weaponState != WeaponState.Swinging3 && Input.GetKeyDown(KeyCode.R) && !onCooldown) {
            inAction = true;
            wielder.GetComponent<BoxCollider2D>().enabled = false;

            //Set up dash collider 
            var mouse = Input.mousePosition;
            var screenPoint = Camera.main.WorldToScreenPoint(wielder.transform.localPosition);
            var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
            yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

            var translate = new Vector3(dashSpeed*xRatio*Time.deltaTime, dashSpeed*yRatio*Time.deltaTime, 0);

            wielder.transform.Translate(translate);

            if (sfx) { AudioSource.PlayClipAtPoint(sfx, wielder.transform.position); }
        } else if (inAction) {
            timer += Time.deltaTime;

            if (timer >= dashTimer) {
                wielder.GetComponent<BoxCollider2D>().enabled = true;
                StartCoroutine(CooldownTimer());
            } else {
                var translate = new Vector3(dashSpeed*xRatio*Time.deltaTime, dashSpeed*yRatio*Time.deltaTime, 0);

                wielder.transform.Translate(translate);
            }
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
