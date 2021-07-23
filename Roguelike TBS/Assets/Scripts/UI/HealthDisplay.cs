using UnityEngine;
using RPG.Stats;
using TMPro;

public class HealthDisplay : MonoBehaviour {
    private Health health;

    private void Awake() {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    private void Update() {
        if (health == null) {
            GetComponent<TextMeshProUGUI>().text = "Dead";
            return;
        }
        GetComponent<TextMeshProUGUI>().text = health.GetHealthPoints().ToString() + "/" + health.GetMaxHealthPoints().ToString();
    }
}
