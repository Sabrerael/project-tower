using RPG.Stats;

public class Balor : Boss {
    enum BalorPhase {
        Starting,
        Idle,
        LongswordAttack,
        WhipAttack,
        FireAttack,
        Teleport
    }

    private BalorPhase phase = BalorPhase.Starting;

    private void Start() {
        SetUpBossHealthBar();
    }

    private void Update() {
        if (gameObject.GetComponent<Health>().IsDead()) { return; }

        if (phase == BalorPhase.Idle) { return; }
        
    }
}
