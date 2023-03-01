using UnityEngine;

public class Panike : Collectible {
    [SerializeField] private Player player;

    protected override void OnTriggerEnter2D(Collider2D target) {
        if (target.CompareTag(Utils.BAKER_TAG)) {
            // If the baker collects the panike

            // Add the panike to the baker
            this.player.EatPanike();

            // Set the panike inactive
            this.gameObject.SetActive(false);
        }
    }
}