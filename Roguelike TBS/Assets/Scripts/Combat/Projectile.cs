using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour {
    [SerializeField] float speed = 1;
    [SerializeField] bool isHoming = true;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] float maxLifeTime = 10;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] float lifeAfterImpact = 2;
    [SerializeField] UnityEvent onHit;

    Health target = null;
    Vector3 targetPoint;
    GameObject instigator = null;
    float xRatio, yRatio;
    int damage = 0;

    private void Start() {
        GetAimLocation();
        SetRotation();
    }

    void Update() {
        if (target != null && isHoming && !target.IsDead()) {
            GetAimLocation();
            SetRotation();
        }
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public GameObject GetInstigator() { return instigator; }

    public void SetTarget(Health target, GameObject instigator, int damage) {
        SetTarget(instigator, damage, target);
    }

    public void SetTarget(Vector3 targetPoint, GameObject instigator, int damage) {
        SetTarget(instigator, damage, null, targetPoint);
    }

    public void SetTarget(GameObject instigator, int damage, Health target=null, Vector3 targetPoint=default) {
        this.target = target;
        this.targetPoint = targetPoint;
        this.damage = damage;
        this.instigator = instigator;

        Destroy(gameObject, maxLifeTime);
    }

    private Vector3 GetAimLocation() {
        if (target == null) {
            return targetPoint;
        }

        return target.transform.position;
    }

    private void SetRotation() {
        if (target == null) {
            var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
            var offset = new Vector2(targetPoint.x - screenPoint.x, targetPoint.y - screenPoint.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
            yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

            transform.rotation = Quaternion.Euler(0, 0, angle);
        } else {
            var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            var offset = new Vector2(
                    target.transform.position.x - transform.position.x,
                    target.transform.position.y - transform.position.y
            );
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
            yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Health health = other.GetComponent<Health>();
        if (target != null && health != target) return;
        if (health == null || health.IsDead()) return;
        if (other.gameObject == instigator) return;
        health.TakeDamage(instigator, damage);

        speed = 0;

        onHit.Invoke();

        if (hitEffect != null) {
            Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        }

        foreach (GameObject toDestroy in destroyOnHit) {
            Destroy(toDestroy);
        }

        Destroy(gameObject, lifeAfterImpact);
    }
}
