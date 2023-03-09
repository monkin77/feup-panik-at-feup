using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameOverUI;

    // Flag to check if the game is over
    private bool _isGameOver = false;
    public bool IsGameOver { get => _isGameOver; }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    
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
        this._isGameOver = false;
    }
}
