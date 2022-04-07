using UnityEngine;

public class SpawnAOE : MonoBehaviour {
    [SerializeField] AOE aofGameObject = null;
    [SerializeField] AudioClip sfx = null;

    private Vector3 startingPoint;
    private GameObject instigator = null;

    private void Start() {
        startingPoint = transform.position;
        instigator = GetComponent<Projectile>().GetInstigator(); //This might cause issues somewhere else
    }

    private void Update() {
        if ((startingPoint - transform.position).magnitude >= 5f) {
            SpawnAreaOfEffect();
        }
    }

    public void SpawnAreaOfEffect() {
        var newAOF = Instantiate(aofGameObject, transform.position + (Vector3.forward * 5), Quaternion.identity);
        newAOF.SetInstigator(instigator);
        if (sfx) { AudioSource.PlayClipAtPoint(sfx, newAOF.transform.position); }
        Destroy(gameObject);
    }
}
