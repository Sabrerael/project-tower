using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {
    [SerializeField] float timeToDelete = 10;

    private float timer = 0;

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= timeToDelete) {
            Destroy(gameObject);
        }
    }
}
