using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] private int _damage = 20;
    private string BAKER_TAG = "Baker";
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("COLLISION WITH BOOK : " + other.tag);
        if (other.CompareTag(BAKER_TAG))
        {
            other.GetComponent<Player>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
