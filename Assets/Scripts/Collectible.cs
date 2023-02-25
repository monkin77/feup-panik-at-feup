using UnityEngine;

public class Collectible : MonoBehaviour
{
    private string BAKER_TAG = "Baker";
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag(BAKER_TAG))
        {
            target.GetComponent<Player>().AddCollectible(gameObject);
            gameObject.SetActive(false);
        }
    }
}
