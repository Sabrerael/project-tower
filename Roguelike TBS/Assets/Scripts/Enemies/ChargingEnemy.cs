using UnityEngine;

public class ChargingEnemy : Enemy {
    [SerializeField] float cooldownTime = 2f;

    private Vector3 startingPoint;
    private bool moving = false;
    private float timer = 0f;

    private float deltaX = 0;
    private float deltaY = 0;

    private void Update() {
        if (health.IsDead()) { return; }

        if (!moving) {
            timer += Time.deltaTime;
            if (timer >= cooldownTime) {
                animator.SetTrigger("Charge");
            }
        }
    }

    private new void OnCollisionEnter2D(Collision2D other) {
        base.OnCollisionEnter2D(other);

        animator.SetTrigger("Stop");
        enemyRigidbody2D.velocity = new Vector2();
        moving = false;
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
        moving = true;
    }
}
