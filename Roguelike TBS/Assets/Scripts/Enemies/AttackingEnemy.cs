using UnityEngine;

public class AttackingEnemy : Enemy {
    [SerializeField] int weaponDamage = 10;
    [SerializeField] GameObject attackZone;

    private AttackState attackState = AttackState.Ready;
    private float cooldownTimer = 0;

    private void Update() {
        if (player == null) { return; }

        if (health.IsDead()) { return; }
        
        if (Mathf.Sign(aiPath.desiredVelocity.x) == -1) {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            healthBar.SetLocalScale(true);
        } else {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            healthBar.SetLocalScale(false);
        }

        if (attackState == AttackState.Cooldown) {
            cooldownTimer += Time.deltaTime;
        }

        if (cooldownTimer >= 2) {
            ResetAttackState();
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (attackState == AttackState.Ready) {
                TriggerAttack();
            }
        }
    }

    public void TriggerAttack() {
        animator.SetTrigger("Attack");
        attackState = AttackState.Attacking;
    }

    public void AttackCooldown() {
        attackState = AttackState.Cooldown;
        DisableAttackZone();
    }

    public void ResetAttackState() {
        attackState = AttackState.Ready;
        cooldownTimer = 0;
    }

    public void EnableAttackZone() {
        attackZone.SetActive(true);
    }

    public void DisableAttackZone() {
        attackZone.SetActive(false);
    }
}
