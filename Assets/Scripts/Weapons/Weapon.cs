using UnityEngine;

public enum WeaponOrientation
{
    Left,
    Right,
    Up,
    Down
}

public class Weapon : MonoBehaviour
{
    protected WeaponOrientation _orientation = WeaponOrientation.Down;
    public WeaponOrientation Orientation
    {
        get { return _orientation; }
        set { _orientation = value; }
    }
    
    protected SpriteRenderer sr;

    protected bool _isAttacking = false;
    public bool IsAttacking { get => _isAttacking; set => _isAttacking = value; }
    
    protected bool _isPowerUp = false;
    public bool IsPowerUp { get => _isPowerUp; set => _isPowerUp = value; }


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsAttacking)
        {
            if (this.IsPowerUp)
                PowerUpAttack();
            else 
                Attack();
        }
    }
    
    /**
     * Base attack method for all weapons.
     * It resets the _isAttacking flag to false after the animation
     * is over.
     */
    public virtual void Attack()
    {
        // Base attack method for all weapons
    }
    
    /**
     * Power up attack method for all weapons.
     * It resets the _isAttacking flag to false after the animation is over.
     * It is called when the player is in power up mode (has panikes and pressed R).
     */
    public virtual void PowerUpAttack()
    {
        // Power up attack method for all weapons
    }
    
    
    /**
     * Sets the weapon orientation according to the player orientation
     */
    public virtual void ChangeOrientation(WeaponOrientation newOrientation)
    {

    }
    
    public virtual void AddAmmo(Collectible ammo) {}

    /**
    * Resets the weapon to its original state
    */
    protected virtual void stopAttacking() {}

    public static Vector2 vecFromOrientation(WeaponOrientation orientation) {
        switch (orientation) {
            case WeaponOrientation.Left:
                return Vector2.left;
            case WeaponOrientation.Right:
                return Vector2.right;
            case WeaponOrientation.Up:
                return Vector2.up;
            case WeaponOrientation.Down:
                return Vector2.down;
            default:
                return Vector2.down;
        }
    }
}
