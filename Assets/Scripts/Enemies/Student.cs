using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : Enemy
{

    public static float BOSS_MULTIPLIER = 1.5f;
    
    /**
     * Attack the baker.
     * This function is called after the attack animation as a trigger
     * and takes damage to the baker in case she is in attack range
     */
    public void AttackBaker() {
        if (IsInAttackRange())
            baker.GetComponent<Player>().TakeDamage(ENEMY_DAMAGE);
        else 
            anim.SetBool(WALK_ANIMATION, true);
    }

    public override void transfBoss() {
        this.speed *= BOSS_MULTIPLIER;
    }
}
