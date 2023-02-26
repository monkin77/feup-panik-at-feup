using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rpg : Weapon
{
    public static float RECOIL_TIME = 0.15f;
    public static float MAX_HORIZONTAL_REC_DISTANCE = 0.5f;
    public static float MAX_VERTICAL_REC_DISTANCE = 0.3f;
    private float lastRecoilDistance = 0f;
    private bool recoilling = false;
    
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
        Vector3 recoilDirection = Vector3.zero;
        switch (_orientation)
        {
            case WeaponOrientation.Down:
                recoilDirection = Vector3.down;
                recoilDistance = MAX_VERTICAL_REC_DISTANCE;
                break;
            case WeaponOrientation.Up:
                recoilDirection = Vector3.up;
                recoilDistance = MAX_VERTICAL_REC_DISTANCE;
                break;
            case WeaponOrientation.Left:
                recoilDirection = Vector3.left;
                recoilDistance = MAX_HORIZONTAL_REC_DISTANCE;
                break;
            case WeaponOrientation.Right:
                recoilDirection = Vector3.right;
                recoilDistance = MAX_HORIZONTAL_REC_DISTANCE;
                break;
        }
        
        float recoilSpeed = recoilDistance / RECOIL_TIME;

        // weapon is attacking, recoil it
        if (!recoilling) {

            if (lastRecoilDistance < recoilDistance)
            {
                transform.position += recoilSpeed * Time.deltaTime * recoilDirection;
                lastRecoilDistance += recoilSpeed * Time.deltaTime;
            }
            else
                recoilling = true; 
        } // end if
        else
        {
            if (lastRecoilDistance > 0)
            {
                transform.position -= recoilSpeed * Time.deltaTime * recoilDirection;
                lastRecoilDistance -= recoilSpeed * Time.deltaTime;
            }
            else
            {
                _isAttacking = false;
                recoilling = false;
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
                transform.rotation = Quaternion.Euler(0, 120, 0);
                transform.localPosition = new Vector3(1.8f, -0.1f, 0f);
                break;
            case WeaponOrientation.Down:
                sr.sortingOrder = 1;
                transform.rotation = Quaternion.Euler(0, 120, 0);
                transform.localPosition = new Vector3(0f, -0.2f, 0f);

                break;
        }
        
        _orientation = newOrientation;
    }
}
