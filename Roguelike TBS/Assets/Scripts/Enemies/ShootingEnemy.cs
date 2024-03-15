using Pathfinding;
using RPG.Stats;
using System.Collections;
using UnityEngine;

public class ShootingEnemy : Enemy {
    [SerializeField] EnemyProjectile arrow = null;
    [SerializeField] float arrowSpeed = 5f;
    [SerializeField] float timeBetweenShots = 2f;
    [SerializeField] float shootTimer = -3f; // Gives player timer before Enemy starts shooting
    [SerializeField] float nextWaypointDistance = 3f;
    [SerializeField] float walkingSpeed;

    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;

    private void Start() {
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        seeker.StartPath(enemyRigidbody2D.position, player.transform.position, OnPathComplete);
    }

    private void FixedUpdate() {
        if (health.IsDead() || interrupted) { return; }
        if (path == null) { return; }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - enemyRigidbody2D.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)direction, direction);

        if (hit.collider.gameObject.CompareTag("Player")) {
            Vector2 force = direction * walkingSpeed * Time.deltaTime;
            enemyRigidbody2D.AddForce(force);

            float distance = Vector2.Distance(enemyRigidbody2D.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance) {
                currentWaypoint++;
            }
            return;
        }

        shootTimer += Time.deltaTime;

        if (shootTimer >= timeBetweenShots) {
            if (Mathf.Sign(player.transform.position.x - transform.position.x) == -1) {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                healthBar.SetLocalScale(true);
            } else {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                healthBar.SetLocalScale(false);
            }
            
            TriggerAttackAnimation();
            shootTimer = 0;
            return;
        }
    }

    private void TriggerAttackAnimation() {
        animator.SetTrigger("Attack");
    }

    public void Attack() {
        EnemyProjectile spawnedItem = Instantiate(arrow, transform.position, Quaternion.identity);
        // TODO fix these GetComponents?
        spawnedItem.SetDamage(gameObject.GetComponent<BaseStats>().GetStat(Stat.Attack));
        spawnedItem.SetWielder(this);

        var offset = new Vector2(
            player.transform.position.x - spawnedItem.transform.position.x,
            player.transform.position.y - spawnedItem.transform.position.y
        );
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

        spawnedItem.transform.rotation = Quaternion.Euler(0, 0, angle);

        spawnedItem.SetMovementValues(xRatio * arrowSpeed, yRatio * arrowSpeed);
    }

    protected override IEnumerator Knockback(Collider2D other) {
        interrupted = true;
        enemyRigidbody2D.AddForce((transform.position - other.transform.position).normalized * knockbackForce);
        yield return new WaitForSeconds(knockbackTime);
        enemyRigidbody2D.velocity = Vector3.zero;
        yield return new WaitForSeconds(knockbackTime);
        interrupted = false;
    }

    private void OnPathComplete(Path p){
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void UpdatePath() {
        if (seeker.IsDone()) {
            seeker.StartPath(enemyRigidbody2D.position, player.transform.position, OnPathComplete);
        }
    }
}
