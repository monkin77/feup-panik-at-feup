using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveForce = 10f;
    
    [SerializeField]
    private List<string> collectibles;
    
    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    
    private Animator anim;
    private string BAKER_UP_ANIMATION = "WalkUp";
    private string BAKER_DOWN_ANIMATION = "WalkDown";
    private string BAKER_WALK_HORIZONTAL = "WalkHorizontal";

    private float movementX;
    private float movementY;
    
    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        sr = GetComponent<SpriteRenderer>();
    }
    
    // Update is called once per frame
    void Update()
    {
        PlayerMoveKeyboard();
        AnimatePlayer();
    }

    void PlayerMoveKeyboard()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");
        
        //myBody.velocity = new Vector2(movementX * moveForce, myBody.velocity.y);
        transform.position += new Vector3(movementX * moveForce * Time.deltaTime, 
            movementY * moveForce * Time.deltaTime, 0);
    }
    
    void AnimatePlayer()
    {
        if (movementY > 0)
        {
            anim.SetBool(BAKER_UP_ANIMATION, true);
        }
        else if (movementY < 0)
        {
            anim.SetBool(BAKER_DOWN_ANIMATION, true);
        } 
        else
        {
            anim.SetBool(BAKER_DOWN_ANIMATION, false);
            anim.SetBool(BAKER_UP_ANIMATION, false);
        }

        if (movementX > 0)
        {
            anim.SetBool(BAKER_WALK_HORIZONTAL, true); 
            sr.flipX = false;
        }
        else if (movementX < 0)
        {
            anim.SetBool(BAKER_WALK_HORIZONTAL, true);
            sr.flipX = true;
        }
        else
        {
            anim.SetBool(BAKER_WALK_HORIZONTAL, false);
        }
    }

    public void AddCollectible(GameObject collectible)
    {
        collectibles.Add(collectible.name);
    }
}
