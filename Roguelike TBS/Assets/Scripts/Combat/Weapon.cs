using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] float baseAttackSpeed = 10;
    [SerializeField] protected float startingAngle = -20f;
    [SerializeField] protected float endingAngle = -160f;
    [SerializeField] WeaponConfig weaponConfig = null;
    [SerializeField] AudioClip swingSound = null;

    protected Fighter wielder = null;
    private float timeCount = 0f;
    protected bool isSwinging = false;
    private float attackSpeedModifier = 1;

    private void Update() {
        CheckIfSwinging();

        SwingWeapon();
    }

    private void FixedUpdate() {
        UseActiveAbility();
    }

    public float GetBaseAttackSpeed() { return baseAttackSpeed; }
    public float GetStartingAngle() { return startingAngle; }
    public float GetEndingAngle() { return endingAngle; }
    public int GetWeaponDamage() { return weaponConfig.GetWeaponDamage(); }
    public Fighter GetWielder() { return wielder; }

    public void SetWielder(Fighter fighter) {
        wielder = fighter;
    }

    public virtual void UpdateWeaponAngles(Direction direction) {
        if (direction == Direction.Up) {
            // Sword is over head
            startingAngle = -45f;
            endingAngle = 95f;
        } else if (direction == Direction.Right) {
            // Sword is to the right
            startingAngle = -20f;
            endingAngle = -160f;
        } else if (direction == Direction.Down) {
            // Sword is under
            startingAngle = -100f;
            endingAngle = -240f;
        } else if (direction == Direction.Left){ 
            // Sword is to the left
            startingAngle = 20f;
            endingAngle = 160f;
        }

        if (!isSwinging) {
            gameObject.transform.rotation = Quaternion.Euler(0,0, startingAngle);
        }
    }

    private void CheckIfSwinging() {
        if (!isSwinging && Input.GetMouseButtonDown(0)) {
            isSwinging = true;
            GetComponent<Collider2D>().enabled = true;
            AudioSource.PlayClipAtPoint(swingSound, transform.position);
            attackSpeedModifier = wielder.GetAttackSpeedModifier();
        }
    }

    private void SwingWeapon() {
        if (isSwinging) {
            transform.rotation = Quaternion.Slerp(
                    Quaternion.Euler(0, 0, startingAngle),
                    Quaternion.Euler(0, 0, endingAngle),
                    timeCount);
            timeCount += Time.deltaTime * baseAttackSpeed * attackSpeedModifier;
        }

        if (isSwinging && transform.rotation == Quaternion.Euler(0, 0, endingAngle)) {
            isSwinging = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            transform.rotation = Quaternion.Euler(0, 0, startingAngle);
            timeCount = 0;
        }
    }

    public virtual void UseActiveAbility() {
        // Empty, this will be overridden in specific weapon scripts
        return;
    }
}
