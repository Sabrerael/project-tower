using TMPro;
using UnityEngine;

public class PurseDisplay : MonoBehaviour {
    private Purse purse;

    private void Start() {
        purse = GameObject.FindGameObjectWithTag("Player").GetComponent<Purse>();
    }

    private void Update() {
        GetComponent<TextMeshProUGUI>().text = purse.GetBalance().ToString();
    }
}