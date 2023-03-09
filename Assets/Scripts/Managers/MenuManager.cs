using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    /**
    * Starts the game.
    */
    public void StartGame() {
        SceneManager.LoadScene(Utils.GAME_SCENE_INDEX);
    }

    public void QuitGame() {
        // Quit the game
        Application.Quit();
    }
}
