using UnityEngine;


public class Shovel : Weapon
{
    public static int MAX_HORIZONTAL_ANGLE = 65;
    public static int MAX_DOWN_ANGLE = 100;
    public static int MAX_UP_ANGLE = 50;
    public static float SHOVEL_ROTATE_TIME = 0.15f;
    private bool goingUp = false;

    // The current max angle for the shovel attack animation (changes w/ orientation)
    private float _currMaxAngle = MAX_DOWN_ANGLE;

    /**
     * Attack method for the shovel.
     * The shovel will rotate back and forth between 0 and MAX_ANGLE degrees.
     * In the end of the animation, the shovel will return to its original position.
     */
    public override void Attack()
    {
        if (!_isAttacking)
            return;

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
                goingUp = false;
                _isAttacking = false;
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
    
}
