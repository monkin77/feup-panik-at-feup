using UnityEngine;

public class Panike : Collectible
{
    protected override void OnTriggerEnter2D(Collider2D target) {
        Debug.Log("Panike IS OVERRIDING");
    }
}