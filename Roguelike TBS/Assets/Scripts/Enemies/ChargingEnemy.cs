using Pathfinding;
using System.Collections;
using UnityEngine;

public class ChargingEnemy : Enemy {
    [SerializeField] float cooldownTime = 2f;
    [SerializeField] float nextWaypointDistance = 3f;
    [SerializeField] float walkingSpeed;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;

    private Vector3 startingPoint;
    private bool stopped = false;
    private bool charging = false;
    private float timer = 0f;

    private float deltaX = 0;
    private float deltaY = 0;

    private void Start() {
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        seeker.StartPath(enemyRigidbody2D.position, player.transform.position, OnPathComplete);
    }

    private void FixedUpdate() {
        if (health.IsDead()) { return; }
        if (path == null) { return; }

        if (stopped) {
            timer += Time.deltaTime;
            if (timer >= cooldownTime) {
                stopped = false;
            } else {
                return;
            }
        }

        if (charging) { 
            enemyRigidbody2D.velocity = new Vector2(deltaX, deltaY);
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - enemyRigidbody2D.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)direction, direction);

        if (hit.collider.gameObject.CompareTag("Player")) {
            Debug.Log("raycast hit player");
            animator.SetTrigger("Charge");
            return;
        }

        Vector2 force = direction * walkingSpeed * Time.deltaTime;
        enemyRigidbody2D.AddForce(force);

        float distance = Vector2.Distance(enemyRigidbody2D.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
    }

    private new void OnCollisionEnter2D(Collision2D other) {
        base.OnCollisionEnter2D(other);

        animator.SetTrigger("Stop");
        enemyRigidbody2D.velocity = new Vector2();
        stopped = true;
        charging = false;
        timer = 0;
    }

    public void StartCharge() {
        startingPoint = transform.localPosition;

        var offset = new Vector2(
            player.transform.position.x - gameObject.transform.position.x,
            player.transform.position.y - gameObject.transform.position.y
        );
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

        deltaX = xRatio * movementSpeed;
        deltaY = yRatio * movementSpeed;

        enemyRigidbody2D.velocity = new Vector2(deltaX, deltaY);
                
        if (Mathf.Sign(deltaX) == 1) {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            healthBar.SetLocalScale(true);
        } else {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            healthBar.SetLocalScale(false);
        }

        charging = true;
    }
    
    protected override IEnumerator Knockback(Collider2D other) {
        enemyRigidbody2D.AddForce((transform.position - other.transform.position).normalized * knockbackForce);
        yield return new WaitForSeconds(knockbackTime);
        enemyRigidbody2D.velocity = Vector3.zero;
        yield return new WaitForSeconds(knockbackTime);
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
