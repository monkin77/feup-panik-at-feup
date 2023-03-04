using UnityEngine;

public class Panike : Collectible {
    [SerializeField] private Player player;

    // Boolean to check if the panike has been spawned in the boss wave
    private bool _spawned = false;

    protected override void OnTriggerEnter2D(Collider2D target) {
        if (target.CompareTag(Utils.BAKER_TAG)) {
            // If the baker collects the panike

            // Add the panike to the baker
            this.player.EatPanike();

            // Set the panike inactive
            this.gameObject.SetActive(false);
        }
    }

    /**
    * Sets the active state of the panike object
    */
    public void setActive(bool active) {
        this.gameObject.SetActive(active);
    }

    public void setSpawned(bool spawned) {
        this._spawned = spawned;
    }

    public bool getSpawned() {
        return this._spawned;
    }
}