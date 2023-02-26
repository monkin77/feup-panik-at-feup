using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rpg : Weapon
{
    public static string BULLET_TAG = "Bullet";
    public static float BULLET_SPEED = 5f;
    public static float RECOIL_TIME = 0.15f;
    public static float MAX_HORIZONTAL_REC_DISTANCE = 0.15f;
    public static float MAX_VERTICAL_REC_DISTANCE = 0.08f;
    private float lastRecoilDistance = 0f;
    private bool recoilling = false;
    private bool firstShot = true;
    private Queue<Collectible> rpgAmmo = new Queue<Collectible>();
    
    /**
     * Attack method for the shovel.
     * The shovel will rotate back and forth between 0 and MAX_ANGLE degrees.
     * In the end of the animation, the shovel will return to its original position.
     */
    public override void Attack()
    {
        if (!_isAttacking)
            return;
        
        float recoilDistance = 0f;
        Vector2 recoilDirection = Vector2.zero;
        switch (_orientation)
        {
            case WeaponOrientation.Down:
                recoilDirection = Vector2.up;
                recoilDistance = MAX_VERTICAL_REC_DISTANCE;
                break;
            case WeaponOrientation.Up:
                recoilDirection = Vector2.up;
                recoilDistance = MAX_VERTICAL_REC_DISTANCE;
                break;
            case WeaponOrientation.Left:
                recoilDirection = Vector2.right;
                recoilDistance = MAX_HORIZONTAL_REC_DISTANCE;
                break;
            case WeaponOrientation.Right:
                recoilDirection = Vector2.left;
                recoilDistance = MAX_HORIZONTAL_REC_DISTANCE;
                break;
        }
        
        // first shot, create a new bullet
        if (firstShot && rpgAmmo.Count > 0)
        {
            firstShot = false;
            Collectible ammo = rpgAmmo.Dequeue();
            GameObject bullet = ammo.gameObject;
            bullet.transform.position = transform.position;
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().velocity = recoilDirection * BULLET_SPEED;
        }
        
        
        float recoilSpeed = recoilDistance / RECOIL_TIME;

        // weapon is attacking, recoil it
        if (!recoilling) {

            if (lastRecoilDistance < recoilDistance)
            {
                Vector2 recoilVector = recoilSpeed * Time.deltaTime * recoilDirection;
                transform.position += new Vector3(recoilVector.x, recoilVector.y, 0);
                lastRecoilDistance += recoilSpeed * Time.deltaTime;
            }
            else
                recoilling = true; 
        } // end if
        else
        {
            if (lastRecoilDistance > 0)
            {
                Vector2 recoilVector = recoilSpeed * Time.deltaTime * recoilDirection;
                transform.position -= new Vector3(recoilVector.x, recoilVector.y, 0);
                lastRecoilDistance -= recoilSpeed * Time.deltaTime;
            }
            else
            {
                _isAttacking = false;
                recoilling = false;
                firstShot = true;
            }
        }
    }
    


    /**
     * Sets the weapon orientation according to the player orientation
     */
    public override void ChangeOrientation(WeaponOrientation newOrientation)
    {
        if (newOrientation == _orientation)
            return;
        
        switch (newOrientation)
        {
            case WeaponOrientation.Left:
                sr.sortingOrder = -1;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localPosition = new Vector3(0f, 0f, 0f);
                break;
            case WeaponOrientation.Right:
                sr.sortingOrder = 1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.localPosition = new Vector3(1f, -0.3f, 0f);
                break;
            case WeaponOrientation.Up:
                sr.sortingOrder = 1;

                transform.rotation = Quaternion.Euler(0, 120, 0);
                transform.localPosition = new Vector3(1.8f, -0.1f, 0f);
                break;
            case WeaponOrientation.Down:
                // Move the weapon behind the player
                sr.sortingOrder = -1;

                // Adjust the weapon rotation and position
                transform.rotation = Quaternion.Euler(0, 230, 0);
                transform.localPosition = new Vector3(0.1f, -0.2f, 0f);

                break;
        }
        
        _orientation = newOrientation;
    }
    
    public override void AddAmmo(Collectible ammo)
    {
        ammo.tag = BULLET_TAG;
        rpgAmmo.Enqueue(ammo);
    }
}
