using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // List of possible enemies
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    // List of enemies prefabs to spawn in the current wave
    private List<GameObject> enemiesToSpawn = new List<GameObject>();

    private int currWave = 0;

    [SerializeField] private float timeBetweenWaves = 5f;

    [SerializeField]
    private float spawnFrequency = 1f;
    // Stores the time to spawn the next enemy
    private float spawnTimer = 1f;

    // boolean to check if a wave is currently in progress
    private bool waveInProgress = false;

    void Awake() {
        Debug.Log("WaveSpawner: Awake()");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WaveSpawner: Start()");
        // Generate the first wave
        generateWave();
    }

    // fixedUpdate is called at a fixed interval
    void FixedUpdate()
    {
        Debug.Log("WaveSpawner: FixedUpdate()");

        // If there are still enemies to spawn in the current wave
        if (this.enemiesToSpawn.Count > 0) {
            if (this.spawnTimer <= 0) {
            // Spawn an enemy
      
            // TODO: Change this to spawn at a random position
            Vector3 spawnPos = new Vector3(Random.Range(-3f, 3f), Random.Range(-1.5f, 2f), 0);

            // Instantiate the enemy
            Instantiate(this.enemiesToSpawn[0], spawnPos, Quaternion.identity);
            this.enemiesToSpawn.RemoveAt(0);

            // Reset the spawn timer
            this.spawnTimer = this.spawnFrequency;
            } else {
                this.spawnTimer -= Time.fixedDeltaTime;
            }
        } else {    // If there are no more enemies to spawn in the current wave
            // Check if there are any enemies left in the scene. If so, do nothing

            // If there are no enemies left in the scene, generate a new wave

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

        // wait for a certain amount of time before starting the next wave
        StartCoroutine(waitForNextWave());

        List<GameObject> generatedEnemies = new List<GameObject>();

        // Increment the wave number
        this.currWave++;
        int waveValue = GetWaveValue(this.currWave);

        // Verify if current wave is a Boss Wave (Panike TIME!)
        if (this.currWave % 4 == 0) {
            // TODO: Enemies have increased stats
        }

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
    @param wave: the wave number
    @return The wave value that will be used to determine the number of enemies to spawn
    */
    private static int GetWaveValue(int wave) {
        return 4 + wave * 2;
    }

    /**
    Waits for a certain amount of time before starting the next wave
    */
    private IEnumerator waitForNextWave() {
        yield return new WaitForSeconds(this.timeBetweenWaves);
    }
}
