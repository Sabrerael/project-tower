using RPG.Stats;

public class Beholder : Boss {
    enum BeholderPhase {
        Starting,
        Idle,
        Bite,
        MainEyeLazer,
        LittleEyeLazers,
        SwingStalks
    }

    private BeholderPhase phase = BeholderPhase.Starting;

    private void Start() {
        SetUpBossHealthBar();
    }

    private void Update() {
        if (gameObject.GetComponent<Health>().IsDead()) { return; }

        if (phase == BeholderPhase.Idle) { return; }
        
    }

    private void ChangeBossPhase() {

    }

    private void Bite() {

    }

    private void FireMainEyeLazer(){

    }

    private void FireLittleEyeLazers() {

    }

    private void SwingStalks() {

    }
}
