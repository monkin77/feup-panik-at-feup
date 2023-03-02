using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher : Enemy
{
    
    public static float BOSS_HP_MULTIPLIER = 2.0f;
    private static float BOSS_BSPEED_MULTIPLIER = 1.5f;
    [SerializeField] private GameObject bookPrefab;
    [SerializeField] private float _bulletSpeed = 1f;
    [SerializeField] private float _attackTime = 1.5f;
    
    private bool _isAttacking = false;
    

    /**
     * Attack the baker.
     * This function is called after the attack animation as a trigger
     * and takes damage to the baker in case she is in attack range
     */
    public void AttackBaker()
    {
        if (IsInAttackRange() && !_isAttacking)
        {
            // create a game object with prefab Book and set its velocity towards the baker
            GameObject book = Instantiate(bookPrefab, transform.position, Quaternion.identity);
            book.GetComponent<Book>().SetDamage(ENEMY_DAMAGE);
            book.GetComponent<Rigidbody2D>().velocity = 
                (baker.transform.position - transform.position).normalized * _bulletSpeed;
            
            StartCoroutine(AttackReset());
        }
        else 
            anim.SetBool(WALK_ANIMATION, true);
    }
    
    public override void transfBoss() {
        this.Health *= BOSS_HP_MULTIPLIER;
        this._bulletSpeed *= BOSS_BSPEED_MULTIPLIER;
    }
    
    IEnumerator AttackReset()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(_attackTime);
        _isAttacking = false;
    }
}
