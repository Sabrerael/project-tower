using UnityEngine;

public class EnemyProjectile : MonoBehaviour {
    [SerializeField] int contactDamage = 1;
    [SerializeField] protected float deltaX, deltaY = 0;
    
    protected Enemy wielder = null;

    protected virtual void Update() {
        float newXPos = transform.localPosition.x + (deltaX * Time.deltaTime);
        float newyPos = transform.localPosition.y + (deltaY * Time.deltaTime);

        transform.localPosition = new Vector2(newXPos, newyPos);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag != "Enemy" || other.gameObject.tag != "Enemy Projectile") {
            Destroy(gameObject);
        }
    }

    public int GetDamage() { return contactDamage; }
    public Enemy GetWielder() { return wielder; }

    public void SetDamage(int damage) { contactDamage = damage; }
    public void SetWielder(Enemy enemy) { wielder = enemy; }
    public void SetMovementValues(float x, float y) { 
        deltaX = x;
        deltaY = y;
    }
}
