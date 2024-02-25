using RPG.Stats;
using UnityEngine;

public class AttackZone : MonoBehaviour {
    [SerializeField] BaseStats enemyBaseStats;
    [SerializeField] AttackingEnemy enemy;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Animator>().SetTrigger("Hit");
            var playerBaseStats = other.gameObject.GetComponent<BaseStats>();
            var damageDealt = enemyBaseStats.GetStat(Stat.Attack);
            other.gameObject.GetComponent<Health>().TakeDamage(gameObject, damageDealt);
            other.GetComponent<Fighter>().StartIFrameTimer(0.75f);
            enemy.AttackCooldown();
        }
    }
}
