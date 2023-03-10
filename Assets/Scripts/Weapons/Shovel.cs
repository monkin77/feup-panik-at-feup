using System;
using System.Collections.Generic;
using UnityEngine;


public class Shovel : Weapon
{
    public static float MAX_ENEMY_DISTANCE = 0.3f;
    public static int MAX_HORIZONTAL_ANGLE = 65;
    public static int MAX_DOWN_ANGLE = 100;
    public static int MAX_UP_ANGLE = 50;
    public static float SHOVEL_ROTATE_TIME = 0.15f;
    
    private bool goingUp = false;
    
    private string ENEMY_TAG = "Enemy";
    private int _damage = 30;

    // The current max angle for the shovel attack animation (changes w/ orientation)
    private float _currMaxAngle = MAX_DOWN_ANGLE;

    // Audio for shovel hit
    [SerializeField] private AudioSource hitAudio;

    [SerializeField] private AudioSource thunderAudio;


    private void Start() {
        this.powerUpPfbMoveDuration = 0.5f;
    }

    /**
     * Attack method for the shovel.
     * The shovel will rotate back and forth between 0 and MAX_ANGLE degrees.
     * In the end of the animation, the shovel will return to its original position.
     */
    public override void Attack()
    {
        if (!this._isAttacking) return;
        
        float shovelRotateSpeed = this._currMaxAngle / SHOVEL_ROTATE_TIME * Time.deltaTime;

        // is attacking, rotate the shovel
        if (!goingUp)
        {
            if (transform.rotation.eulerAngles.z < this._currMaxAngle)
                transform.Rotate(new Vector3(0, 0, shovelRotateSpeed));
            else
            {
                // just finished the attack animation, check if there are enemies in range
                // and attack them
                AttackEnemiesInRange();
                goingUp = true;
            }
        }
        else
        {
            if (transform.rotation.eulerAngles.z >= shovelRotateSpeed)
                transform.Rotate(new Vector3(0, 0, -shovelRotateSpeed));
            else
                this.stopAttacking();
        }
    }
    
    public override void PowerUpAttack()
    {
        if (!this._isAttacking)
            return;

        // throws a lightning with the baker
        if (!this.goingUp) {
            this.goingUp = true;
            
            Vector2 orientation = Weapon.vecFromOrientation(this._orientation);
            // move the thunder a bit to the side of the baker if the baker is facing left or right
            Vector3 thunderFinalPos = this.transform.position + 
                                        new Vector3(orientation.x, orientation.y, 0) * powerUpPositionOffset;
            
            Instantiate(this.powerUpPrefab, thunderFinalPos, Quaternion.identity);

            // Play the thunder sound
            this.thunderAudio.Play();
            
            StartCoroutine(PowerUpMovementReset());

            // Remove 1 panike from the Baker
            GameObject.FindWithTag(Utils.BAKER_TAG).GetComponent<Player>().decrementPanike();
        }
    }

    /**
     * Sets the weapon orientation according to the player orientation
     */
    public override void ChangeOrientation(WeaponOrientation newOrientation)
    {
        // If the orientation is the same, do nothing
        if (newOrientation == _orientation)
            return;
        
        if (this._isAttacking) this.stopAttacking();

        switch (newOrientation)
        {
            case WeaponOrientation.Left:
                // Move the shovel behind the player
                sr.sortingOrder = -1;

                // Adjust the shovel position and rotation
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localPosition = new Vector3(0.7f, 0.1f, 0f);

                // Update the current max angle
                this._currMaxAngle = MAX_HORIZONTAL_ANGLE;
                break;
            case WeaponOrientation.Right:
                sr.sortingOrder = 1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.localPosition = new Vector3(0.9f, -0.3f, 0f);

                this._currMaxAngle = MAX_HORIZONTAL_ANGLE;
                break;
            case WeaponOrientation.Up:
                transform.rotation = Quaternion.Euler(0, 120, 0);
                transform.localPosition = new Vector3(1.5f, -0.2f, 0f);

                this._currMaxAngle = MAX_UP_ANGLE;
                break;
            case WeaponOrientation.Down:
                sr.sortingOrder = 1;
                transform.rotation = Quaternion.Euler(0, 45, 0);
                transform.localPosition = new Vector3(0f, 0f, 0f);

                this._currMaxAngle = MAX_DOWN_ANGLE;
                break;
        }
        
        _orientation = newOrientation;
    }
    
    /**
    * Resets the weapon to its original state
    */
    protected override void stopAttacking() {
        this.goingUp = false;
        this._isAttacking = false;
        this._isPowerUp = false;
    }

    /**
     * Handles the collision with enemies
     * @returns true if enemy's distance to the shovel is lower
     * than a max_distance, false otherwise
     */
    private bool IsInAttackRange(Vector2 position)
    {
        Vector2 shovelPosition = transform.position;
        float distance = Vector2.Distance(shovelPosition, position);
        return distance < MAX_ENEMY_DISTANCE;
    }

    /**
     * Attack all enemies in range
     */
    private void AttackEnemiesInRange() {
        // get all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(ENEMY_TAG);

        bool hitEnemy = false;
        // attack all enemies in range
        foreach (var enemy in enemies) {
            if (IsInAttackRange(enemy.transform.position)) {
                enemy.GetComponent<Enemy>().TakeDamage(_damage);
                hitEnemy = true;
            }
        }

        if (hitEnemy) {
            // play hit audio
            this.hitAudio.Play();
        }
    }
}
