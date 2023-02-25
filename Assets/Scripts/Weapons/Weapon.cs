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
    
    private SpriteRenderer sr;

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
    public void ChangeOrientation(WeaponOrientation newOrientation)
    {
        if (newOrientation == _orientation)
            return;
        

        switch (newOrientation)
        {
            case WeaponOrientation.Left:
                sr.sortingOrder = -1;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localPosition = new Vector3(0.7f, 0.1f, 0f);
                break;
            case WeaponOrientation.Right:
                sr.sortingOrder = 1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.localPosition = new Vector3(0.9f, -0.3f, 0f);
                break;
            case WeaponOrientation.Up:
                transform.rotation = Quaternion.Euler(0, 120, 0);
                transform.localPosition = new Vector3(1.5f, -0.2f, 0f);
                break;
            case WeaponOrientation.Down:
                sr.sortingOrder = 1;
                transform.rotation = Quaternion.Euler(0, 45, 0);
                transform.localPosition = new Vector3(0f, 0f, 0f);

                break;
        }
        
        _orientation = newOrientation;
    }
}
