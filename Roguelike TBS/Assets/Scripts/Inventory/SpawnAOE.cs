using UnityEngine;

public class SpawnAOE : MonoBehaviour {
    [SerializeField] AOE aofGameObject = null;
    [SerializeField] AudioClip sfx = null;

    private Vector3 startingPoint;
    private GameObject instigator = null;

    private void Start() {
        startingPoint = transform.position;
    }

    private void Update() {
        if ((startingPoint - transform.position).magnitude >= 5f) {
            SpawnAreaOfEffect();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        SpawnAreaOfEffect();
    }

    public void SetInstigator(GameObject value) { instigator = value; }

    private void SpawnAreaOfEffect() {
        var newAOF = Instantiate(aofGameObject, transform.position + (Vector3.forward * 5), Quaternion.identity);
        newAOF.SetInstigator(gameObject.GetComponent<ThrownItem>().GetWielder().gameObject);
        if (sfx) { AudioSource.PlayClipAtPoint(sfx, newAOF.transform.position); }
        Destroy(gameObject);
    }

}
