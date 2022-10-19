using RPG.Stats;
using UnityEngine;

public class Spirit : Enemy {
    [SerializeField] GameObject magicBlob = null;
    [SerializeField] float magicBlobSpeed = 5f;
    [SerializeField] float timeBetweenShots = 2f;
    [SerializeField] float shootTimer = -3f;

    private void Update() {
        if (health.IsDead()) { return; }

        shootTimer += Time.deltaTime;

        if (shootTimer >= timeBetweenShots) {
            TriggerAttackAnimation();
        }

        MoveEnemy();
    }

    private void TriggerAttackAnimation() {
        animator.SetTrigger("Attack");
        shootTimer = 0;
    }

    private void MoveEnemy() {
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

    private void ShootMagic() {
        GameObject spawnedItem = Instantiate(magicBlob, gameObject.transform.position, Quaternion.identity);
        // TODO fix these GetComponents?
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

        spawnedItem.GetComponent<EnemyProjectile>().SetMovementValues(xRatio * magicBlobSpeed, yRatio * magicBlobSpeed);
    }
}
