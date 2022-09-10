using RPG.Stats;
using UnityEngine;

public class ChargingEnemy : Enemy {
    [SerializeField] float cooldownTime = 2f;

    private Vector3 startingPoint;
    private bool moving = false;
    private float timer = 0f;

    private float deltaX = 0;
    private float deltaY = 0;

    private void Update() {
        if (gameObject.GetComponent<Health>().IsDead()) { return; }

        if (moving) {
            float newXPos = transform.localPosition.x + deltaX;
            float newyPos = transform.localPosition.y + deltaY;

            transform.localPosition = new Vector2(newXPos, newyPos);
        } else {
            timer += Time.deltaTime;
            if (timer >= cooldownTime) {
                GetComponent<Animator>().SetTrigger("Charge");
            }
        }
    }

    private new void OnCollisionEnter2D(Collision2D other) {
        base.OnCollisionEnter2D(other);

        GetComponent<Animator>().SetTrigger("Stop");
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

        deltaX = xRatio * movementSpeed * Time.deltaTime;
        deltaY = yRatio * movementSpeed * Time.deltaTime;

        float newXPos = transform.localPosition.x + deltaX;
        float newyPos = transform.localPosition.y + deltaY;

        transform.localPosition = new Vector2(newXPos, newyPos);
                
        if (Mathf.Sign(deltaX) == 1) {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        moving = true;
    }
}
