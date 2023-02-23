using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] foodReference;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFood());

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    IEnumerator SpawnFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 5));
            
            int randomIndex = Random.Range(0, foodReference.Length);
            
            Vector3 position = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);
            
            GameObject spawnedFood = Instantiate(foodReference[randomIndex]);
            spawnedFood.transform.position = position;
        }
    }
}
