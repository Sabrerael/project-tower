using UnityEngine;

public class Boss : Enemy {
    public void ToggleCollider() {
        GetComponent<Collider2D>().enabled = !GetComponent<Collider2D>().enabled;
    }

    protected void SetUpBossHealthBar() {
        var bossHealthBar = GameObject.Find("Boss Health Bar");
        bossHealthBar.GetComponent<BossHealthBar>().EnableCanvas();
        bossHealthBar.GetComponent<BossHealthBar>().SetBossTitle(gameObject.name);
        bossHealthBar.GetComponent<BossHealthBar>().SetHealthComponent(gameObject);
    }
}
