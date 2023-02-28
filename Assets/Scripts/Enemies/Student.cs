using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : Enemy
{
    private float xCollideOffset = 0.25f;
    private float yCollideOffset = 0.10f;
    
    /**
     * Check if the baker is in attack range of the enemy
     * @return true if the baker is in attack range of the enemy, false otherwise
     */
    protected override bool IsInAttackRange()
    {
        if (baker.gameObject == null)
            return false;
        
        Vector2 bakerPos = baker.transform.position;
        var bakerX = bakerPos.x;
        var bakerY = bakerPos.y;
        
        var position = transform.position;
        var xPos = position.x;
        var yPos = position.y;
        
        return bakerX - xCollideOffset < xPos && xPos < bakerX + xCollideOffset &&
               bakerY - yCollideOffset < yPos && yPos < bakerY + yCollideOffset;
    }

    /**
     * Attack the baker.
     * This function is called after the attack animation as a trigger
     * and takes damage to the baker in case she is in attack range
     */
    public void AttackBaker()
    {
        if (IsInAttackRange())
            baker.GetComponent<Player>().TakeDamage(ENEMY_DAMAGE);
        else 
            anim.SetBool(WALK_ANIMATION, true);
    }
}
