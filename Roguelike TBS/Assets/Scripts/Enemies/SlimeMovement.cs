using RPG.Stats;
using UnityEngine;

public class SlimeMovement : Enemy {
    private void Update() {
        if (player == null) { return; }

        if (gameObject.GetComponent<Health>().IsDead()) { return; }

        var offset = new Vector2(
            player.transform.position.x - gameObject.transform.position.x,
            player.transform.position.y - gameObject.transform.position.y
        );
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

        var deltaX = xRatio * movementSpeed * Time.deltaTime;
        var deltaY = yRatio * movementSpeed * Time.deltaTime;

        float newXPos = transform.localPosition.x + deltaX;
        float newyPos = transform.localPosition.y + deltaY;

        transform.localPosition = new Vector2(newXPos, newyPos);

        if (Mathf.Sign(deltaX) == -1) {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }
}
