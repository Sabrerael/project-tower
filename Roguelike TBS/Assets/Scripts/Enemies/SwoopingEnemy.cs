using RPG.Stats;
using UnityEngine;

public class SwoopingEnemy : Enemy {
    [SerializeField] float distance = 5f;
    [SerializeField] float cooldownTime = 2f;
    [SerializeField] float xMin, xMax, yMin, yMax = 0;

    private Vector3 startingPoint;
    private bool moving = false;
    private float timer = 0f;

    private float deltaX = 0;
    private float deltaY = 0;

    private void Update() {
        if (health.IsDead()) { return; }

        if (moving) {
            if ((startingPoint - transform.localPosition).magnitude >= distance) {
                enemyRigidbody2D.velocity = new Vector3();
                moving = false;
            }
        } else {
            timer += Time.deltaTime;
            if (timer >= cooldownTime) {
                animator.SetTrigger("Swoop");
            }
        }
    }

    public void StartSwoop() {
        timer = 0;

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
                
        if (Mathf.Sign(deltaX) == -1) {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        moving = true;
    }
}
