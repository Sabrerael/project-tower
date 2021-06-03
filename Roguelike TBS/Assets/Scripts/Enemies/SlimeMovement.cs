using RPG.Stats;
using UnityEngine;

public class SlimeMovement : Enemy {
    // Update is called once per frame
    private void Update() {
        if (player == null) { return; }

        if (gameObject.GetComponent<Health>().IsDead()) { return; }
        
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;

        float translateX = (playerX - transform.position.x) * movementSpeed;
        float translateY = (playerY - transform.position.y) * movementSpeed;

        transform.Translate(translateX  * Time.deltaTime, translateY  * Time.deltaTime, 0f);
    }
}
