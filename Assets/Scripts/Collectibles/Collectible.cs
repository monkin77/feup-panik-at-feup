using UnityEngine;

public class Collectible : MonoBehaviour
{
    private int _bulletDamage = 100;

    protected virtual void OnTriggerEnter2D(Collider2D target)
    {
        // If the collectible is a bullet, then it was already collected by the baker
        if (target.CompareTag(Utils.BAKER_TAG) && !gameObject.CompareTag(Utils.BULLET_TAG))
        {
            target.GetComponent<Player>().AddCollectible(gameObject.GetComponent<Collectible>());
            // removes the food from the food spawner
            FoodSpawner.Instance.removeFood(gameObject);
            gameObject.SetActive(false);
        } else if (target.CompareTag(Utils.ENEMY_TAG) && gameObject.CompareTag(Utils.BULLET_TAG))
        {
            // If the bullet hits an enemy, then destroy the bullet and deal damage to the enemy
            target.GetComponent<Enemy>().TakeDamage(_bulletDamage);
            Destroy(gameObject);
        } else if (target.CompareTag(Utils.WALL_TAG)){
            // If the bullet hits an obstacle or if the food is spawned on an obstacle, then destroy the bullet / food

            // removes the food from the food spawner if it was a food
            if (!gameObject.CompareTag(Utils.BULLET_TAG)) FoodSpawner.Instance.removeFood(gameObject);

            Destroy(gameObject);
        }
    }
}
