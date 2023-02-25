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

    IEnumerator SpawnFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(SPAWN_MIN_TIME, SPAWN_MAX_TIME));
            
            int randomIndex = Random.Range(0, foodReference.Length);
            
            Vector3 position = new Vector3(Random.Range(SPAWN_X_MIN, SPAWN_X_MAX), 
                Random.Range(SPAWN_Y_MIN, SPAWN_Y_MAX), 0);
            
            GameObject newFood = Instantiate(foodReference[randomIndex]);
            newFood.transform.position = position;
            
            spawnedFood.Add(newFood);
            StartCoroutine(DestroyFood(newFood));
        }
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
    
    /**
     * Called by the player when he eats food
     */
    public void collectFood(GameObject food)
    {
        spawnedFood.Remove(food);
    }
}
