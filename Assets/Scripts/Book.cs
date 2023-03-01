using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] private int _damage = 20;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Utils.BAKER_TAG))
        {
            other.GetComponent<Player>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
