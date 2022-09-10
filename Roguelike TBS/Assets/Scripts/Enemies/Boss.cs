using UnityEngine;

public class Boss : Enemy {
    [SerializeField] string bossName = "Default Boss Name";

    public void ToggleCollider() {
        GetComponent<Collider2D>().enabled = !GetComponent<Collider2D>().enabled;
    }

    protected void SetUpBossHealthBar() {
        var bossHealthBar = GameObject.Find("Boss Health Bar").GetComponent<BossHealthBar>();

        bossHealthBar.EnableCanvas();
        bossHealthBar.SetBossTitle(bossName);
        bossHealthBar.SetHealthComponent(gameObject);
    }
}
