using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakerBoss : MonoBehaviour
{
    
    [SerializeField] private float objectDestroyDelay = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestroy());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Destroy any enemy that collides with the baker boss
        if (col.gameObject.CompareTag(Utils.ENEMY_TAG))
        {
            Destroy(col.gameObject);
        }
    }


    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(objectDestroyDelay);
        Destroy(this.gameObject);
    }
}
