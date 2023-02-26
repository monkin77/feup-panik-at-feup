using System;
using System.Collections;
using System.Collections.Generic;
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
    public bool IsAttacking
    {
        get { return _isAttacking; }
        set { _isAttacking = value; }
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAttacking)
            Attack();
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
     * Sets the weapon orientation according to the player orientation
     */
    public virtual void ChangeOrientation(WeaponOrientation newOrientation)
    {

    }
}
