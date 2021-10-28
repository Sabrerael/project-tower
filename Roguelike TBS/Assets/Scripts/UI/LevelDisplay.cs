using UnityEngine;
using RPG.Stats;
using TMPro;

public class LevelDisplay : MonoBehaviour {
    private BaseStats baseStats;

    private void Start() {
        baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
    }

    private void Update() {
        GetComponent<TextMeshProUGUI>().text = "Level " + baseStats.GetLevel().ToString();
    }
}
