using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shovel : Weapon
{
    public static int MAX_HORIZONTAL_ANGLE = 65;
    public static int MAX_DOWN_ANGLE = 100;
    public static int MAX_UP_ANGLE = 40;
    public static float SHOVEL_ROTATE_TIME = 0.2f;
    private bool goingUp = false;

    /**
     * Attack method for the shovel.
     * The shovel will rotate back and forth between 0 and MAX_ANGLE degrees.
     * In the end of the animation, the shovel will return to its original position.
     */
    public override void Attack()
    {
        var maxAngle = 0;
        switch (_orientation)
        {
            case WeaponOrientation.Down:
                maxAngle = MAX_DOWN_ANGLE;
                break;
            case WeaponOrientation.Up:
                maxAngle = MAX_UP_ANGLE;
                break;
            case WeaponOrientation.Left:
                maxAngle = MAX_HORIZONTAL_ANGLE;
                break;
            case WeaponOrientation.Right:
                maxAngle = MAX_HORIZONTAL_ANGLE;
                break;
        }
        
        if (!_isAttacking)
            return;

        float shovelRotateSpeed = maxAngle / SHOVEL_ROTATE_TIME * Time.deltaTime;

        // is attacking, rotate the shovel
        if (!goingUp)
        {
            if (transform.rotation.eulerAngles.z < maxAngle)
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
                goingUp = false;
                _isAttacking = false;
            }
        }
    }
    
}
