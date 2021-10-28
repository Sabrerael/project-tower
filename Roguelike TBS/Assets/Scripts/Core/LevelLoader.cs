using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public void LoadMainMenu() {
        DestroyPlayerObjects();

        SceneManager.LoadScene(0);
    }

    public void LoadControls() {
        SceneManager.LoadScene(1);
    }

    public void LoadPlayerSelect() {
        SceneManager.LoadScene(2);
    }

    public void LoadLevelOne() {
        SceneManager.LoadScene(3);
    }

    public void LoadLevelTwo() {
        SceneManager.LoadScene(4);
    }

    public void LoadLevelThree() {
        SceneManager.LoadScene(5);
    }

    public void LoadWinScreen() {
        DestroyPlayerObjects();

        SceneManager.LoadScene(6);
    }

    public void LoadGameOver() {
        DestroyPlayerObjects();
        
        SceneManager.LoadScene(7);
    }

    public void QuitGame() {
        Application.Quit();
    }

    private void DestroyPlayerObjects() {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player) {
            GameObject.Destroy(GameObject.Find("Pause Menu"));
            GameObject.Destroy(GameObject.Find("Level Up Bonuses Menu"));
            GameObject.Destroy(GameObject.Find("HUD"));
            GameObject.Destroy(player);
        }
    }
}
