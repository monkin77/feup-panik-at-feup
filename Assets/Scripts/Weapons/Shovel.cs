using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : Weapon
{

    public static int MAX_ANGLE = 55;
    public static float SHOVEL_ROTATE_SPEED = 1f;
    private bool goingUp = false;

    /**
     * Attack method for the shovel.
     * The shovel will rotate back and forth between 0 and MAX_ANGLE degrees.
     * In the end of the animation, the shovel will return to its original position.
     */
    public override void Attack()
    {
        if (_isAttacking)
        {
            if (!goingUp)
            {
                if (transform.rotation.eulerAngles.z < MAX_ANGLE)
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

}
