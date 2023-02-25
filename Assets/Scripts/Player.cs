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
    
    private bool isAttacking;

    private int weapon_idx = 0;

    [SerializeField] private Weapon weapon;
    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        sr = GetComponent<SpriteRenderer>();
        weapon = transform.GetChild(0).GetChild(weapon_idx).gameObject.GetComponent<Weapon>();
    }
    
    // Update is called once per frame
    void Update()
    {
        ListenKbEvents();
        AnimatePlayer();
    }
    
    /**
     * Checks if attack and move keys were pressed
     * and act according to it
     */
    void ListenKbEvents()
    {
        if (!weapon.IsAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Debug.Log("PLAYER SPACE EVENT");
                weapon.IsAttacking = true;
            }
        }

        PlayerMoveKeyboard();
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
            weapon.ChangeOrientation(WeaponOrientation.Down);
            anim.SetBool(BAKER_DOWN_ANIMATION, true);
        } 
        else
        {
            anim.SetBool(BAKER_DOWN_ANIMATION, false);
            anim.SetBool(BAKER_UP_ANIMATION, false);
        }

        if (movementX > 0)
        {
            weapon.ChangeOrientation(WeaponOrientation.Left);
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
