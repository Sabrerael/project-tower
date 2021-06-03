using RPG.Stats;
using UnityEngine;

public class Cloaker : Enemy {
    [SerializeField] float distance = 5f;
    [SerializeField] float cooldownTime = 2f;
    [SerializeField] float xMin, xMax, yMin, yMax = 0;

    private Vector3 startingPoint;
    private bool moving = false;
    private float timer = 0f;

    private float deltaX = 0;
    private float deltaY = 0;

    private void Update() {
        if (gameObject.GetComponent<Health>().IsDead()) { return; }

        if (moving) {
            float newXPos = Mathf.Clamp(transform.localPosition.x + deltaX, xMin, xMax);
            float newyPos = Mathf.Clamp(transform.localPosition.y + deltaY, yMin, yMax);

            transform.localPosition = new Vector2(newXPos, newyPos);

            if ((startingPoint - transform.localPosition).magnitude >= distance) {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                moving = false;
            }
        } else {
            timer += Time.deltaTime;
            if (timer >= cooldownTime) {
                timer = 0;

                startingPoint = transform.localPosition;

                var offset = new Vector2(
                    player.transform.position.x - gameObject.transform.position.x,
                    player.transform.position.y - gameObject.transform.position.y
                );
                var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

                var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
                var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

                deltaX = xRatio * movementSpeed * Time.deltaTime;
                deltaY = yRatio * movementSpeed * Time.deltaTime;

                float newXPos = Mathf.Clamp(transform.localPosition.x + deltaX, xMin, xMax);
                float newyPos = Mathf.Clamp(transform.localPosition.y + deltaY, yMin, yMax);

                transform.localPosition = new Vector2(newXPos, newyPos);
                moving = true;
            }
        }
    }
}
