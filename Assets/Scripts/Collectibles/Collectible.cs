using UnityEngine;

public class Collectible : MonoBehaviour
{
    private string BAKER_TAG = "Baker";
    private string BULLET_TAG = "Bullet";
    private void OnTriggerEnter2D(Collider2D target)
    {
        // If the collectible is a bullet, then it was already collected by the baker
        if (target.CompareTag(BAKER_TAG) && !gameObject.CompareTag(BULLET_TAG))
        {
            target.GetComponent<Player>().AddCollectible(gameObject.GetComponent<Collectible>());
            gameObject.SetActive(false);
        }
    }
}
