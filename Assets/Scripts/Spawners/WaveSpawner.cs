using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    // List of possible enemies
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    // List of enemies prefabs to spawn in the current wave
    private List<GameObject> enemiesToSpawn = new List<GameObject>();

    private int currWave = 0;

    [SerializeField] private float timeBetweenWaves = 5f;

    // Stores the time to start the next wave
    private float timeForNextWave = 5f;

    [SerializeField] private int BOSS_WAVE = 4;

    [SerializeField]
    private float spawnFrequency = 1f;
    // Stores the time to spawn the next enemy
    private float spawnTimer = 1f;

    // boolean to check if a wave is currently in progress
    private bool waveInProgress = false;
    
    private string ENEMY_TAG = "Enemy";
    
    private bool _isBossWave = false;

    // Game Object to store the Text UI element that shows the time left for the next wave
    [SerializeField] private TextMeshProUGUI timeLeftText;

    // Start is called before the first frame update
    void Start()
    {
        return;
    }

    // fixedUpdate is called at a fixed interval
    void FixedUpdate()
    {
        // If there is no wave in progress, generate a new wave
        if (!this.waveInProgress) {
            // If the time for the next wave has not passed, do nothing
            if (!checkNextWave()) return;

            // If the time for the next wave has passed, generate a new wave
            generateWave();
            return;
        }

        // If there are still enemies to spawn in the current wave
        if (this.enemiesToSpawn.Count > 0) {
            if (this.spawnTimer <= 0) {
            // Spawn an enemy
      
            // TODO: Change this to spawn at a random position
            Vector3 spawnPos = new Vector3(Random.Range(-3f, 3f), Random.Range(-1.5f, 2f), 0);

            // Instantiate the enemy
            GameObject enemy = Instantiate(this.enemiesToSpawn[0], spawnPos, Quaternion.identity);
            this.enemiesToSpawn.RemoveAt(0);
            
            // If the current wave is a boss wave, double the speed of the enemies
            if (this._isBossWave) {
                print("PANIKE TIME!");
                enemy.GetComponent<Enemy>().transfBoss();
            }

            // Reset the spawn timer
            this.spawnTimer = this.spawnFrequency;
            } else {
                this.spawnTimer -= Time.fixedDeltaTime;
            }
        } else {
            // If there are no more enemies to spawn in the current wave
            // Check if there are any enemies left in the scene. If so, do nothing
            GameObject[] enemyInstances = GameObject.FindGameObjectsWithTag(ENEMY_TAG);
            if (enemyInstances.Length > 0) {
                return;
            }

            // If there are no enemies left in the scene, set the wave as completed 
            this.waveInProgress = false;
        }
    }

    /**
    Prepares a new wave by generating a list of enemies to spawn
    This method will be called upon Start or when the current wave is completed
    */
    public void generateWave() {
        if (this.waveInProgress) {
            return;
        }

        List<GameObject> generatedEnemies = new List<GameObject>();

        // Increment the wave number
        this.currWave++;
        int waveValue = GetWaveValue(this.currWave);

        // Verify if current wave is a Boss Wave (Panike TIME!)
        this._isBossWave = this.currWave % BOSS_WAVE == 0;
        // if the current wave is a boss wave, double the speed of the enemies
        

        // while the wave value is greater than 0
        while (waveValue > 0) {
            int randEnemyIdx = Random.Range(0, this.enemies.Count);
            Enemy currEnemy = this.enemies[randEnemyIdx].GetComponent<Enemy>();
            
            int enemyCost = currEnemy.cost;

            if (waveValue - enemyCost >= 0) {
                generatedEnemies.Add(this.enemies[randEnemyIdx]);
                waveValue -= enemyCost;
            } else {
                break;
            }
        }

        this.enemiesToSpawn.Clear();
        this.enemiesToSpawn = generatedEnemies;

        this.spawnTimer = this.spawnFrequency;
        this.waveInProgress = true;
    }

    /**
    @return true if the time for the next wave has passed, false otherwise
    */
    public bool checkNextWave() {
        if (this.timeForNextWave > 0) {
            // Subtract the time passed since the last frame
            this.timeForNextWave -= Time.fixedDeltaTime;

            // Show the time left for the next wave
            int timeLeft = Mathf.RoundToInt(this.timeForNextWave);
            this.timeLeftText.text = $"Next wave in {timeLeft} seconds";

            return false;
        } else {
            this.timeForNextWave = this.timeBetweenWaves;

            // Hide the time left for the next wave
            this.timeLeftText.text = "";

            return true;
        }
    }

    /**
    @param wave: the wave number
    @return The wave value that will be used to determine the number of enemies to spawn
    */
    private static int GetWaveValue(int wave) {
        return 4 + wave * 2;
    }
}
