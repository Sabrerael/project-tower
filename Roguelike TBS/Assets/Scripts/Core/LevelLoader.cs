using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void LoadControls() {
        SceneManager.LoadScene(1);
    }

    public void LoadLevelOne() {
        SceneManager.LoadScene(2);
    }

    public void LoadLevelTwo() {
        SceneManager.LoadScene(3);
    }

    public void LoadLevelThree() {
        SceneManager.LoadScene(4);
    }

    public void LoadWinScreen() {
        SceneManager.LoadScene(5);
    }

    public void LoadGameOver() {
        SceneManager.LoadScene(6);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
