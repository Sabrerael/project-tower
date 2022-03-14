using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponState {
    Ready,
    Swinging1,
    Swinging2,
    Swinging3
}

public class Fighter : MonoBehaviour, IModifierProvider {
    [SerializeField] Transform handTransform = null;
    [SerializeField] WeaponConfig currentWeapon = null;
    [SerializeField] float iFramesTimeLimit = 0.75f;
    [SerializeField] float dodgeRollTime = 1f;
    [SerializeField] float comboResetTime = 1.25f;
    [SerializeField] AudioClip swingSound = null;

    private Camera mainCam;
    private int multiplicativeModifier = 0;
    private int movementModifier = 0;
    private int attackSpeedModifier = 0;
    private bool iFramesActive = false;
    private bool waitingForNextAttack = false;
    protected bool inputReceived = false;
    private WeaponState weaponState = WeaponState.Ready;

    public event Func<bool> onInitialHit;
    public event Action<Enemy> onActualHit;

    private void Start() {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        EquipWeapon(currentWeapon);
    }

    private void Update() {
        if (mainCam == null) {
            mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        }    
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (iFramesActive) { return; }

        if (onInitialHit != null && onInitialHit()) { return; }

        if (other.gameObject.tag == "Enemy") {
            gameObject.GetComponent<Animator>().SetTrigger("Hit");
            var enemyBaseStats = other.gameObject.GetComponent<BaseStats>();
            var damageTaken = enemyBaseStats.GetStat(Stat.Attack) - gameObject.GetComponent<BaseStats>().GetStat(Stat.Defense);
            gameObject.GetComponent<Health>().TakeDamage(other.gameObject, damageTaken);
            StartCoroutine(IFrameTimer(iFramesTimeLimit));
            if (onActualHit != null) { onActualHit(other.gameObject.GetComponent<Enemy>()); }
        } else if (other.gameObject.tag == "Enemy Projectile") {
            gameObject.GetComponent<Animator>().SetTrigger("Hit");
            var enemyProjectile = other.gameObject.GetComponent<EnemyProjectile>();
            var damageTaken = enemyProjectile.GetDamage() - gameObject.GetComponent<BaseStats>().GetStat(Stat.Defense);
            gameObject.GetComponent<Health>().TakeDamage(other.gameObject, damageTaken);
            StartCoroutine(IFrameTimer(iFramesTimeLimit));
        }
    }

    public void EquipWeapon(WeaponConfig weapon) {
        currentWeapon = weapon;
        weapon.Spawn(handTransform, null, this);
    }

    // Sets iFramesActive to the opposite of what it currently is
    public void ToggleIFramesActive() { iFramesActive = !iFramesActive; }

    public void DodgeRoll() {
        // Set Animator value
        GetComponent<Movement>().StartDodgeRolling();
        StartIFrameTimer(dodgeRollTime);
    }

    public void StartComboResetTimer() {
        StartCoroutine(WeaponSwingTimer(comboResetTime));
    }

    public void CheckIfSwinging() {
        if (waitingForNextAttack) {
            return;
        } else if (weaponState == WeaponState.Ready) {
            ChangeWeaponState();
        } else {
            inputReceived = true;
            ChangeWeaponState();
        }

        AudioSource.PlayClipAtPoint(swingSound, transform.position);
    }

    public void StartIFrameTimer(float time) {
        StartCoroutine(IFrameTimer(time));
    }

    public void ToggleWeaponCollider() {
        var weaponCollider = handTransform.gameObject.GetComponentInChildren<Collider2D>();
        weaponCollider.enabled = !weaponCollider.enabled;
    }

    private void ChangeWeaponState() {
        if (weaponState == WeaponState.Swinging3) {
            weaponState = WeaponState.Ready;
            return;
        }

        if (weaponState == WeaponState.Ready) {
            weaponState = WeaponState.Swinging1;
            CheckMouseLocation();
            GetComponent<Animator>().SetTrigger("AttackState1");
            return;
        }

        if (weaponState != WeaponState.Ready && !inputReceived) {
            weaponState = WeaponState.Ready;
            inputReceived = false;
            return;
        }

        if (inputReceived) {
            if (weaponState == WeaponState.Swinging1) {
                weaponState = WeaponState.Swinging2;
                CheckMouseLocation();
                GetComponent<Animator>().SetTrigger("AttackState2");
                inputReceived = false;
                return;
            } else if (weaponState == WeaponState.Swinging2) {
                weaponState = WeaponState.Swinging3;
                CheckMouseLocation();
                GetComponent<Animator>().SetTrigger("AttackState3");
                waitingForNextAttack = true;
                return;
            }
        }
    }

    private void CheckMouseLocation() {
        var offset = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        if (40 < angle && angle < 110) {
           GetComponent<Animator>().SetTrigger("MouseUp"); 
        } else if (-110 < angle && angle < -40) {
            GetComponent<Animator>().SetTrigger("MouseDown");
        }
    }

    private IEnumerator WeaponSwingTimer(float timer) {
        yield return new WaitForSeconds(timer);
        ChangeWeaponState();
        inputReceived = false;
        waitingForNextAttack = false;
        GetComponent<Animator>().ResetTrigger("AttackState2");
        GetComponent<Animator>().ResetTrigger("AttackState3");
        GetComponent<Animator>().ResetTrigger("MouseUp");
        GetComponent<Animator>().ResetTrigger("MouseDown");
    }

    public IEnumerable<int> GetAdditiveModifiers(Stat stat) {
        if (stat == Stat.Attack) {
            yield return currentWeapon.GetWeaponDamage();
        }
    }

    public IEnumerable<int> GetMultiplicativeModifiers(Stat stat) {
        if (stat == Stat.Attack) {
            yield return multiplicativeModifier;
        } else if (stat == Stat.AttackSpeed) {
            yield return attackSpeedModifier;
        } else if (stat == Stat.MovementSpeed) {
            yield return movementModifier;
        }
    }

    public float GetAttackSpeedModifier() {
        return GetComponent<BaseStats>().GetStat(Stat.AttackSpeed);
    }

    private IEnumerator IFrameTimer(float time) {
        iFramesActive = true;

        yield return new WaitForSeconds(time);

        iFramesActive = false;
    }
}
