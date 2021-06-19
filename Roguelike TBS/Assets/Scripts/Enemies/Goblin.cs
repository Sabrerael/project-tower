using RPG.Stats;
using UnityEngine;

public enum AttackState {
    Ready,
    Attacking,
    Cooldown
}

public class Goblin : Enemy {
    [SerializeField] int weaponDamage = 10;

    private AttackState attackState = AttackState.Ready;
    // Will chase after the player, swinging their sword when in range
    private void Update() {
        if (player == null) { return; }

        if (gameObject.GetComponent<Health>().IsDead()) { return; }

        var offset = new Vector2(
            player.transform.position.x - gameObject.transform.position.x,
            player.transform.position.y - gameObject.transform.position.y
        );
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

        var deltaX = xRatio * movementSpeed * Time.deltaTime;
        var deltaY = yRatio * movementSpeed * Time.deltaTime;

        float newXPos = transform.localPosition.x + deltaX;
        float newyPos = transform.localPosition.y + deltaY;

        transform.localPosition = new Vector2(newXPos, newyPos);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (attackState == AttackState.Ready) {
                TriggerAttack();
            } else if (attackState == AttackState.Attacking) {
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
        GetComponent<Animator>().SetTrigger("Attack");
    }

    public void Attacking() {
        attackState = AttackState.Attacking;
    }

    public void ResetAttackState() {
        attackState = AttackState.Ready;
    }
}
