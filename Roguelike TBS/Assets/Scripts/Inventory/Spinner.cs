using UnityEngine;

public class Spinner : MonoBehaviour {
    [SerializeField] float rotation = 1f;
    
    private void Update() {
        transform.Rotate(0, 0, rotation * Time.deltaTime);
    }
}
