using UnityEngine;
using TMPro;
using RPG.Stats;

public class BossHealthBar : HealthBar {
    [SerializeField] TextMeshProUGUI bossTitle = null;

    protected override void Update() {
        if (healthComponent == null) { return; }

        foreground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);
    }

    public void SetHealthComponent(GameObject boss) {
        healthComponent = boss.GetComponent<Health>();
    }

    public void SetBossTitle(string title) {
        bossTitle.text = title;
    }

    public void EnableCanvas() {
        rootCanvas.gameObject.SetActive(true);
    }
}
