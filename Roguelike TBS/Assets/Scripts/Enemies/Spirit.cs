using RPG.Stats;
using UnityEngine;

public class Spirit : Enemy {
    [SerializeField] GameObject magicBlob = null;
    [SerializeField] float magicBlobSpeed = 5f;
    [SerializeField] float timeBetweenShots = 2f;
    [SerializeField] float shootTimer = -3f;

    // Update is called once per frame
    private void Update() {
        if (gameObject.GetComponent<Health>().IsDead()) { return; }

        shootTimer += Time.deltaTime;

        if (shootTimer >= timeBetweenShots) {
            ShootMagic();
        }

        MoveEnemy();
    }

    private void MoveEnemy() {
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;

        float translateX = (playerX - transform.position.x) * movementSpeed;
        float translateY = (playerY - transform.position.y) * movementSpeed;

        transform.Translate(translateX  * Time.deltaTime, translateY  * Time.deltaTime, 0f);
    }

    private void ShootMagic() {
        GameObject spawnedItem = Instantiate(magicBlob, gameObject.transform.position, Quaternion.identity);
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

        shootTimer = 0;
    }
}
