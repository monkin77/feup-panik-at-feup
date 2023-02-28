using UnityEngine;

public class Collectible : MonoBehaviour
{
    private string BAKER_TAG = "Baker";
    private string BULLET_TAG = "Bullet";
    private string ENEMY_TAG = "Enemy";
    private void OnTriggerEnter2D(Collider2D target)
    {
        // If the collectible is a bullet, then it was already collected by the baker
        if (target.CompareTag(BAKER_TAG) && !gameObject.CompareTag(BULLET_TAG))
        {
            target.GetComponent<Player>().AddCollectible(gameObject.GetComponent<Collectible>());
            // removes the food from the food spawner
            FoodSpawner.Instance.removeFood(gameObject);
            gameObject.SetActive(false);
        } else if (target.CompareTag(ENEMY_TAG) && gameObject.CompareTag(BULLET_TAG))
        {
            // If the bullet hits an enemy, then destroy the bullet and the enemy
            target.GetComponent<Enemy>().SetDieState();
            Destroy(gameObject);
        }
    }
}
