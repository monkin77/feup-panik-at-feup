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
    private int _damage = 100;
    private bool _attackedEnemy = false;

    // The current max angle for the shovel attack animation (changes w/ orientation)
    private float _currMaxAngle = MAX_DOWN_ANGLE;
    

    /**
     * Attack method for the shovel.
     * The shovel will rotate back and forth between 0 and MAX_ANGLE degrees.
     * In the end of the animation, the shovel will return to its original position.
     */
    public override void Attack()
    {
        if (!this._isAttacking)
            return;

        if (!this._attackedEnemy)
        {
            this._attackedEnemy = true;
            // get all objects with Tag "Enemy"
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(ENEMY_TAG);
            // check if any of the enemies are in range and if so attack them
            // player can only attack one enemy at a time
            foreach (var enemy in enemies)
            {
                if (IsInAttackRange(enemy.transform.position))
                {
                    enemy.GetComponent<Enemy>().TakeDamage(_damage);
                    break;
                }
            }
        }

        float shovelRotateSpeed = this._currMaxAngle / SHOVEL_ROTATE_TIME * Time.deltaTime;

        // is attacking, rotate the shovel
        if (!goingUp)
        {
            if (transform.rotation.eulerAngles.z < this._currMaxAngle)
            {
                transform.Rotate(new Vector3(0, 0, shovelRotateSpeed));
            }
            else
            {
                goingUp = true;
            }
        }
        else
        {
            if (transform.rotation.eulerAngles.z >= shovelRotateSpeed)
            {
                transform.Rotate(new Vector3(0, 0, -shovelRotateSpeed));
            }
            else
            {
                this.stopAttacking();
                
            }
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
        this._attackedEnemy = false;
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
        print("Distance: " + distance);
        return distance < MAX_ENEMY_DISTANCE;
    }
}
