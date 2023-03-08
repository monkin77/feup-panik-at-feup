using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakerBoss : MonoBehaviour
{
    
    [SerializeField] private float objectDestroyDelay = 1f;
    private int damage = 1000; 
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestroy());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Destroy any enemy that collides with the baker boss
        if (col.gameObject.CompareTag(Utils.ENEMY_TAG)) {
            // Deal damage to the enemy (insta kill)
            col.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }


    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(objectDestroyDelay);
        Destroy(this.gameObject);
    }
}
