using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;

public class Fighter : MonoBehaviour, IModifierProvider {
    [SerializeField] Transform handTransform = null;
    [SerializeField] WeaponConfig currentWeapon = null;
    [SerializeField] float iFramesTimeLimit = 0.75f;
    [SerializeField] float dodgeRollTime = 1f;

    private int multiplicativeModifier = 0;
    private int movementModifier = 0;
    private int attackSpeedModifier = 0;
    private bool iFramesActive = false;

    private void Start() {
        EquipWeapon(currentWeapon);
    }

    private void Update() {
        UpdateWeaponPlacement();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (iFramesActive) { return; }

        if (other.gameObject.tag == "Enemy") {
            gameObject.GetComponent<Animator>().SetTrigger("Hit");
            var enemyBaseStats = other.gameObject.GetComponent<BaseStats>();
            var damageTaken = enemyBaseStats.GetStat(Stat.Attack) - gameObject.GetComponent<BaseStats>().GetStat(Stat.Defense);
            gameObject.GetComponent<Health>().TakeDamage(other.gameObject, damageTaken);
            StartCoroutine(IFrameTimer(iFramesTimeLimit));
        } else if (other.gameObject.tag == "Enemy Projectile") {
            gameObject.GetComponent<Animator>().SetTrigger("Hit");
            var enemyProjectile = other.gameObject.GetComponent<EnemyProjectile>();
            var damageTaken = enemyProjectile.GetDamage() - gameObject.GetComponent<BaseStats>().GetStat(Stat.Defense);
            gameObject.GetComponent<Health>().TakeDamage(other.gameObject, damageTaken);
            StartCoroutine(IFrameTimer(iFramesTimeLimit));
        }
    }

    private void UpdateWeaponPlacement() {
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        if (angle > 45 && angle <= 135) {
            // Sword is over head
            handTransform.position = gameObject.transform.position + new Vector3(0f, 0.15f, 0);
            handTransform.GetComponentInChildren<Weapon>().UpdateWeaponAngles(Direction.Up);
            handTransform.GetChild(0).transform.localScale = new Vector2 (-1, 1);
        } else if (angle > -45 && angle <= 45) {
            // Sword is to the right
            handTransform.position = gameObject.transform.position + new Vector3(0.45f, -0.2f, 0);
            handTransform.GetComponentInChildren<Weapon>().UpdateWeaponAngles(Direction.Right);
            handTransform.GetChild(0).transform.localScale = new Vector2 (1, 1);
        } else if (angle < -45 && angle >= -135) {
            // Sword is under
            handTransform.position = gameObject.transform.position + new Vector3(0f, -0.35f, 0);
            handTransform.GetComponentInChildren<Weapon>().UpdateWeaponAngles(Direction.Down);
            handTransform.GetChild(0).transform.localScale = new Vector2 (1, 1);
        } else if (angle > 135 || angle < -135){ 
            // Sword is to the left
            handTransform.position = gameObject.transform.position + new Vector3(-0.45f, -0.2f, 0);
            handTransform.GetComponentInChildren<Weapon>().UpdateWeaponAngles(Direction.Left);
            handTransform.GetChild(0).transform.localScale = new Vector2 (-1, 1);
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

    public void StartIFrameTimer(float time) {
        StartCoroutine(IFrameTimer(time));
    }

    private IEnumerator IFrameTimer(float time) {
        iFramesActive = true;

        yield return new WaitForSeconds(time);

        iFramesActive = false;
    }
}
