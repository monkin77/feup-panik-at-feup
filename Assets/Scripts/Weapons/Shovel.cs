using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shovel : Weapon
{

    public static int MAX_HORIZONTAL_ANGLE = 55;
    public static int MAX_DOWN_ANGLE = 150;
    public static int MAX_UP_ANGLE = 40;
    public static float SHOVEL_ROTATE_SPEED = 1f;
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

        // is attacking, rotate the shovel
        if (!goingUp)
        {
            if (transform.rotation.eulerAngles.z < maxAngle)
            {
                transform.Rotate(new Vector3(0, 0, SHOVEL_ROTATE_SPEED));
            }
            else
            {
                goingUp = true;
            }
        }
        else
        {
            if (transform.rotation.eulerAngles.z > SHOVEL_ROTATE_SPEED)
            {
                transform.Rotate(new Vector3(0, 0, -SHOVEL_ROTATE_SPEED));
            }
            else
            {
                goingUp = false;
                _isAttacking = false;
            }
        }
    }
    
}
