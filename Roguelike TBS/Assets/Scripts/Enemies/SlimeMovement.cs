using Pathfinding;
using System.Collections;
using UnityEngine;

public class SlimeMovement : Enemy {
    [SerializeField] float nextWaypointDistance = 3f;

    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;

    private void Start() {
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        seeker.StartPath(enemyRigidbody2D.position, player.transform.position, OnPathComplete);
    }

    private void FixedUpdate() {
        if (health.IsDead()) { return; }
        if (path == null) { return; }
        if (interrupted) {return; }

        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - enemyRigidbody2D.position).normalized;

        enemyRigidbody2D.MovePosition(enemyRigidbody2D.position + (direction * (movementSpeed * Time.fixedDeltaTime)));

        float distance = Vector2.Distance(enemyRigidbody2D.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
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

    public void RunContactKnockback(Collider2D other) {
        StartCoroutine(ContactKnockback(other));
    }

    private IEnumerator ContactKnockback(Collider2D other) {
        interrupted = true;
        enemyRigidbody2D.AddForce((transform.position - other.transform.position).normalized * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackTime);
        enemyRigidbody2D.velocity = Vector3.zero;
        yield return new WaitForSeconds(knockbackTime);
        interrupted = false;
    }
}
