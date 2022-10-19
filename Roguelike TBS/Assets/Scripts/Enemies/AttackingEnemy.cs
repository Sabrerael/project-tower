using RPG.Stats;
using UnityEngine;

public class AttackingEnemy : Enemy {
    [SerializeField] int weaponDamage = 10;

    private AttackState attackState = AttackState.Ready;

    private void Update() {
        if (player == null) { return; }

        if (health.IsDead()) { return; }

        var offset = new Vector2(
            player.transform.position.x - gameObject.transform.position.x,
            player.transform.position.y - gameObject.transform.position.y
        );
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

        var deltaX = xRatio * movementSpeed;
        var deltaY = yRatio * movementSpeed;

        enemyRigidbody2D.velocity = new Vector2(deltaX, deltaY);

        if (Mathf.Sign(deltaX) == -1) {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (attackState == AttackState.Ready) {
                TriggerAttack();
            } else if (attackState == AttackState.Attacking) {
                // TODO Maybe these other calls move into one of the Player scripts as a single function call
                other.gameObject.GetComponent<Animator>().SetTrigger("Hit");
                var playerBaseStats = other.gameObject.GetComponent<BaseStats>();
                var damageDealt = gameObject.GetComponent<BaseStats>().GetStat(Stat.Attack) - playerBaseStats.GetStat(Stat.Defense);
                other.gameObject.GetComponent<Health>().TakeDamage(gameObject, damageDealt);
                other.GetComponent<Fighter>().StartIFrameTimer(0.75f);
                attackState = AttackState.Cooldown;
            }
        }
    }

    public void TriggerAttack() {
        animator.SetTrigger("Attack");
    }

    public void Attacking() {
        attackState = AttackState.Attacking;
    }

    public void ResetAttackState() {
        attackState = AttackState.Ready;
    }
}
