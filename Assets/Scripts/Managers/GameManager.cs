using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;

    // Flag to check if the game is over
    private bool _isGameOver = false;
    public bool IsGameOver { get => _isGameOver; }
    
    /**
    * Called when the player dies.
    * It displays the Game Over UI.
    */
    public void GameOver() {
        this.gameOverUI.SetActive(true);
        this._isGameOver = true;
    }

    /**
    * Reloads the current scene.
    */
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /**
    * Loads the main menu scene.
    */
    public void Menu() {
        SceneManager.LoadScene(Utils.MENU_SCENE_INDEX);
    }
}
