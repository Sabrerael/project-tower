using UnityEngine;
using RPG.Stats;
using TMPro;

public class ExperienceDisplay : MonoBehaviour {
    private Experience experience;

    private void Awake() {
        experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
    }

    private void Update() {
        GetComponent<TextMeshProUGUI>().text = experience.GetPoints().ToString();
    }
}
