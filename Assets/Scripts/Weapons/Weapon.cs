using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected bool _isAttacking = false;
    public bool IsAttacking
    {
        get { return _isAttacking; }
        set { _isAttacking = value; }
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
}
