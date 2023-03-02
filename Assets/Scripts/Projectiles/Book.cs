using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] private int _damage = 0;
    
    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag(Utils.BAKER_TAG))
        {
            target.GetComponent<Player>().TakeDamage(_damage);
            Destroy(gameObject);
        } else if (target.CompareTag(Utils.WALL_TAG)){
            // if the book hits the wall, then destroy it
            Destroy(gameObject);
        }
    }
}
