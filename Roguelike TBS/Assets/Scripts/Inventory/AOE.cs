using RPG.Stats;
using UnityEngine;

public class AOE : MonoBehaviour {
    [SerializeField] protected int damage = 2;
    [SerializeField] float lifeTime = 15;
    [SerializeField] protected GameObject instigator = null;

    private float lifeTimer = 0;

    private float dotTimer = 0;
    private float timerTick = 0;

    private void Update() {
        if (lifeTime == 0) { return; }
        
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifeTime) {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.GetComponent<Health>() == null) { return; }

        dotTimer += Time.deltaTime;
        if (dotTimer >= timerTick) {
            other.gameObject.GetComponent<Health>().TakeDamage(instigator, damage);
            other.gameObject.GetComponent<Animator>().SetTrigger("Hit");
            timerTick += 1;
        }
    }

    public void SetInstigator(GameObject value) { instigator = value; }
}
