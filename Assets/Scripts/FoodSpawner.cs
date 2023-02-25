using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] foodReference;
    
    private List<GameObject> spawnedFood;

    [SerializeField] private float SPAWN_MIN_TIME = 1f;
    [SerializeField] private float SPAWN_MAX_TIME = 5f;
    [SerializeField] private float DESTROY_TIME = 5f;
    
    [SerializeField] private float SPAWN_X_MIN = -8f;
    [SerializeField] private float SPAWN_X_MAX = 8f;
    [SerializeField] private float SPAWN_Y_MIN = -4f;
    [SerializeField] private float SPAWN_Y_MAX = 4f;

    private void Awake()
    {
        spawnedFood = new List<GameObject>();
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFood());
    }

    /**
     * Spawn a random type of food in a random position
     * After a certain time, destroy the food
     */
    IEnumerator SpawnFood()
    {
        while (true)
        {
            // Wait for a random time
            yield return new WaitForSeconds(Random.Range(SPAWN_MIN_TIME, SPAWN_MAX_TIME));
            
            // Spawn random type of food
            int randomIndex = Random.Range(0, foodReference.Length);
            
            // Spawn food at random position
            Vector3 position = new Vector3(Random.Range(SPAWN_X_MIN, SPAWN_X_MAX), 
                Random.Range(SPAWN_Y_MIN, SPAWN_Y_MAX), 0);
            
            // Check if food is already spawned at this position
            List<Vector3> foodPositions = spawnedFood.ConvertAll(food => food.transform.position);
            while (foodPositions.Contains(position))
            {
                position = new Vector3(Random.Range(SPAWN_X_MIN, SPAWN_X_MAX), 
                    Random.Range(SPAWN_Y_MIN, SPAWN_Y_MAX), 0);
            }

            // Create new food
            GameObject newFood = Instantiate(foodReference[randomIndex]);
            newFood.name = newFood.name.Replace("(Clone)", "");
            newFood.transform.position = position;
            
            spawnedFood.Add(newFood);
            
            // Destroy food after a certain time
            StartCoroutine(DestroyFood(newFood));
        } // end while loop
    }

    /**
     * Destroys food after a certain time
     * after it was spawned if it wasn't eaten.
     */
    IEnumerator DestroyFood(GameObject food)
    {
        yield return new WaitForSeconds(DESTROY_TIME);
        if (spawnedFood.Contains(food))
        {
            spawnedFood.Remove(food);
            Destroy(food);
        }
    }
}
