using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    /**
    * Starts the game.
    */
    public void StartGame() {
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        // Quit the game
        Application.Quit();
    }
}
