using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public class DrainingAOE : MonoBehaviour {
    [SerializeField] float healthToDrain = 10;
    [SerializeField] float lifeTime = 0.5f;

    private GameObject instigator = null;
    private float lifeTimer = 0;

    private void Update() {
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifeTime) {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.GetComponent<Health>() == null) { return; }

        other.GetComponent<Health>().TakeDamage(instigator, (int) healthToDrain);
        instigator.GetComponent<Health>().Heal((int)healthToDrain/2);
    }

    public void SetInstigator(GameObject value) { instigator = value; }
}
