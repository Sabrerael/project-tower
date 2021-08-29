using RPG.Stats;
using UnityEngine;

public class ShootingEnemy : Enemy {
    [SerializeField] GameObject arrow = null;
    [SerializeField] float arrowSpeed = 5f;
    [SerializeField] float timeBetweenShots = 2f;
    [SerializeField] float shootTimer = -3f; // Gives player timer before Enemy starts shooting

    // Update is called once per frame
    private void Update() {
        if (gameObject.GetComponent<Health>().IsDead()) { return; }

        shootTimer += Time.deltaTime;

        if (shootTimer >= timeBetweenShots) {
            if (Mathf.Sign(player.transform.position.x - transform.position.x) == -1) {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            } else {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
            TriggerAttackAnimation();
            shootTimer = 0;
        }
    }

    private void TriggerAttackAnimation() {
        GetComponent<Animator>().SetTrigger("Attack");
    }

    public void Attack() {
        GameObject spawnedItem = Instantiate(arrow, transform.position, Quaternion.identity);
        spawnedItem.GetComponent<EnemyProjectile>().SetDamage(gameObject.GetComponent<BaseStats>().GetStat(Stat.Attack));
        spawnedItem.GetComponent<EnemyProjectile>().SetWielder(gameObject.GetComponent<Enemy>());

        var offset = new Vector2(
            player.transform.position.x - spawnedItem.transform.position.x,
            player.transform.position.y - spawnedItem.transform.position.y
        );
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

        spawnedItem.transform.rotation = Quaternion.Euler(0, 0, angle);

        spawnedItem.GetComponent<EnemyProjectile>().SetMovementValues(xRatio * arrowSpeed, yRatio * arrowSpeed);
    }
}
