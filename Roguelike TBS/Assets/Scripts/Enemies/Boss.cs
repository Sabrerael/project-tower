using UnityEngine;

public class Boss : Enemy {
    [SerializeField] string bossName = "Default Boss Name";

    // CACHE
    private Collider2D bossCollider2D;

    private new void Awake() {
        base.Awake();
        bossCollider2D = GetComponent<Collider2D>();
    }

    public void ToggleCollider() {
        bossCollider2D.enabled = !bossCollider2D.enabled;
    }

    protected void SetUpBossHealthBar() {
        var bossHealthBar = GameObject.Find("Boss Health Bar").GetComponent<BossHealthBar>();

        bossHealthBar.EnableCanvas();
        bossHealthBar.SetBossTitle(bossName);
        bossHealthBar.SetHealthComponent(gameObject);
    }
}
