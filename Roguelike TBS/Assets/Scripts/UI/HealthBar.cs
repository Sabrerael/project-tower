using RPG.Stats;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    [SerializeField] protected Health healthComponent = null;
    [SerializeField] protected RectTransform foreground = null;
    [SerializeField] protected Canvas rootCanvas = null;

    protected virtual void Update() {
        if (Mathf.Approximately(healthComponent.GetFraction(), 0) || healthComponent.IsAtMaxHealth()) {
            rootCanvas.enabled = false;
            return;
        }

        rootCanvas.enabled = true;

        foreground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);
    }

    public void SetLocalScale(bool negativeLocalScale) {
        if (negativeLocalScale) {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }
}
