using UnityEngine;

public class SpawnObject : MonoBehaviour {
    [SerializeField] GameObject toBeSpawned = null;
    [SerializeField] AudioClip sfx = null;

    private Vector3 startingPoint;

    private void Start() {
        startingPoint = transform.position;
    }

    private void Update() {
        if ((startingPoint - transform.position).magnitude >= 5f) {
            Spawn();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Spawn();
    }

    private void Spawn() {
        var newAOF = Instantiate(toBeSpawned, transform.position + (Vector3.forward * 5), Quaternion.identity);
        if (sfx) { AudioSource.PlayClipAtPoint(sfx, newAOF.transform.position); }
        Destroy(gameObject);
    }
}
