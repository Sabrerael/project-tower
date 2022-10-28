using RPG.Stats;
using UnityEngine;

public class AttackZone : MonoBehaviour {
    [SerializeField] BaseStats enemyBaseStats;
    [SerializeField] AttackingEnemy enemy;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Animator>().SetTrigger("Hit");
            var playerBaseStats = other.gameObject.GetComponent<BaseStats>();
            var damageDealt = enemyBaseStats.GetStat(Stat.Attack) - playerBaseStats.GetStat(Stat.Defense);
            other.gameObject.GetComponent<Health>().TakeDamage(gameObject, damageDealt);
            other.GetComponent<Fighter>().StartIFrameTimer(0.75f);
            enemy.AttackCooldown();
        }
    }
}
