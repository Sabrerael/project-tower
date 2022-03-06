using UnityEngine;

public class ExtendingProjectile : EnemyProjectile {
    [SerializeField] Transform projectileBody = null;

    protected override void Update() {
        float newXPos = transform.localPosition.x + (deltaX * Time.deltaTime);
        float newyPos = transform.localPosition.y + (deltaY * Time.deltaTime);

        transform.localPosition = new Vector2(newXPos, newyPos);

        GetComponent<BoxCollider2D>().size += new Vector2(deltaX * Time.deltaTime, 0);
        //projectileBody.localScale;
    }
}
