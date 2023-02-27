using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField] private GameObject baker;

    private Rigidbody2D myBody;

    private SpriteRenderer sr;
    
    private Animator anim;
    private string WALK_ANIMATION = "isWalking";
    private string BAKER_TAG = "Baker";

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool(WALK_ANIMATION, true);
        baker = GameObject.FindWithTag(BAKER_TAG);
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        // the baker has a x and y position. Make the enemy move towards the baker
        Vector2 bakerPos = baker.transform.position;
        var bakerX = bakerPos.x;
        var bakerY = bakerPos.y;
        transform.position = Vector2.MoveTowards(
            transform.position, new Vector2(bakerX, bakerY), speed * Time.fixedDeltaTime);
        
        if (transform.position.x > bakerX)
            sr.flipX = true;
        else 
            sr.flipX = false;
        
    }
}
