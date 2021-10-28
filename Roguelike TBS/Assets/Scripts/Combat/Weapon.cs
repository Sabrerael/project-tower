using System.Collections;
using UnityEngine;

public enum WeaponState {
    Ready,
    Swinging1,
    Swinging2,
    Swinging3
}

// TODO A lot of this code needs to come out into the Fighter or a PlayerController
public class Weapon : MonoBehaviour {
    [SerializeField] float baseAttackSpeed = 10;
    [SerializeField] protected float startingAngle = -20f;
    [SerializeField] protected float endingAngle = -160f;
    [SerializeField] protected float timeBeforeNextSwing = 0.5f;
    [SerializeField] protected float comboResetTime = 1.25f;
    [SerializeField] protected WeaponConfig weaponConfig = null;
    [SerializeField] AudioClip swingSound = null;

    protected Fighter wielder = null;
    private float timeCount = 0f;
    private float attackSpeedModifier = 1;
    protected bool waitingForNextAttack = false;
    protected bool inputReceived = false;
    [SerializeField] protected WeaponState weaponState = WeaponState.Ready;

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
    public virtual int GetWeaponDamage() { return weaponConfig.GetWeaponDamage(); }
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

        if (weaponState != WeaponState.Swinging1 && weaponState != WeaponState.Swinging2 && weaponState != WeaponState.Swinging3) {
            gameObject.transform.rotation = Quaternion.Euler(0,0, startingAngle);
        }
    }
    
    public virtual void UseActiveAbility() {
        // Empty, this will be overridden in specific weapon scripts
        return;
    }

    private void CheckIfSwinging() {
        if (Input.GetMouseButtonDown(0)) {
            if (weaponState == WeaponState.Ready) {
                ChangeWeaponState();
            } else {
                inputReceived = true;
            }
            
            GetComponent<Collider2D>().enabled = true;
            AudioSource.PlayClipAtPoint(swingSound, transform.position);
            attackSpeedModifier = wielder.GetAttackSpeedModifier();
        }
    }

    private void ChangeWeaponState() {
        if (weaponState == WeaponState.Swinging3) {
            weaponState = WeaponState.Ready;
            return;
        }

        if (weaponState == WeaponState.Ready) {
            weaponState = WeaponState.Swinging1;
            return;
        }

        if (weaponState != WeaponState.Ready && !inputReceived) {
            weaponState = WeaponState.Ready;
            return;
        }

        if (inputReceived) {
            if (weaponState == WeaponState.Swinging1) {
                weaponState = WeaponState.Swinging2;
                return;
            } else if (weaponState == WeaponState.Swinging2) {
                weaponState = WeaponState.Swinging3;
                return;
            }
        }
    }

    private void SwingWeapon() {
        if (waitingForNextAttack) { return; }

        if (weaponState == WeaponState.Swinging1 || weaponState == WeaponState.Swinging2 || weaponState == WeaponState.Swinging3) {
            transform.rotation = Quaternion.Slerp(
                    Quaternion.Euler(0, 0, startingAngle),
                    Quaternion.Euler(0, 0, endingAngle),
                    timeCount);
            timeCount += Time.deltaTime * baseAttackSpeed * attackSpeedModifier;

            if (transform.rotation == Quaternion.Euler(0, 0, endingAngle)) {
                gameObject.GetComponent<Collider2D>().enabled = false;
                transform.rotation = Quaternion.Euler(0, 0, startingAngle);
                timeCount = 0;
                waitingForNextAttack = true;
                if (weaponState == WeaponState.Swinging3) {
                    StartCoroutine(WeaponSwingTimer(comboResetTime));
                } else {
                    StartCoroutine(WeaponSwingTimer(timeBeforeNextSwing));
                }
            }
        }
    }

    private IEnumerator WeaponSwingTimer(float timer) {
        yield return new WaitForSeconds(timer);
        ChangeWeaponState();
        inputReceived = false;
        waitingForNextAttack = false;
    }
}
