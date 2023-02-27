using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;
    
    private Rigidbody2D rigidBody;
    private SpriteRenderer sr;
    
    private Animator anim;
    private string BAKER_UP_ANIMATION = "WalkUp";
    private string BAKER_DOWN_ANIMATION = "WalkDown";
    private string BAKER_WALK_HORIZONTAL = "WalkHorizontal";

    private float movementX;
    private float movementY;
    
    private bool isAttacking;

    [SerializeField] private int weaponIdx = 0;
    [SerializeField] private List<Weapon> weaponList;
    private int _weaponCount;

    // Stores the settings that determine where a collision can occur. Such as layers to collide with
    [SerializeField] private ContactFilter2D movementFilter;

    // Stores the collisions that occur during movement
    [SerializeField] private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    // Collision offset to prevent the player from getting stuck in walls
    [SerializeField] private float collisionOffset = 0.02f;

    private void Awake()
    {
        this.rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        sr = GetComponent<SpriteRenderer>();
        weaponList = new List<Weapon>();
    }

    void Start()
    {
        for (var i = 0; i < transform.GetChild(0).childCount; i++)
        {
            GameObject go = transform.GetChild(0).GetChild(i).gameObject; 
            Weapon component = go.GetComponent<Weapon>();
            weaponList.Add(component);
            component.gameObject.SetActive(false);
        }

        _weaponCount = weaponList.Count;
        weaponList[this.weaponIdx].gameObject.SetActive(true);
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
        AttackKeyboard();
        PlayerMoveKeyboard();
        ChangeWeaponKb();
    }
    
    /**
     * Checks if weapon change keys were pressed
     * and act according to it
     */
    void ChangeWeaponKb()
    {
        var weapon = weaponList[weaponIdx];
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weapon.gameObject.SetActive(false);
            if (weaponIdx > 0)
                weaponIdx--;
            else
            {
                weaponIdx = _weaponCount - 1;
            }
            weaponList[weaponIdx].gameObject.SetActive(true);
        } 
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // next weapon
            weapon.gameObject.SetActive(false);
            if (weaponIdx < _weaponCount - 1)
                weaponIdx++;
            else
                weaponIdx = 0;
            weapon = weaponList[weaponIdx];
            weapon.gameObject.SetActive(true);
        }

    }

    /**
     * Checks if attack key was pressed
     */
    void AttackKeyboard()
    {
        var weapon = weaponList[weaponIdx];
        if (weapon.IsAttacking)
            return;
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            weapon.IsAttacking = true;
        }
    }

    /**
     * Checks if move keys were pressed
     */
    void PlayerMoveKeyboard()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");
        
        // if there's no movement, do nothing
        if (movementX == 0 && movementY == 0)
            return;

        // Determine the speed in each direction
        float speedX = movementX * moveSpeed * Time.deltaTime;
        float speedY = movementY * moveSpeed * Time.deltaTime;

        // Check if there are collisions with the environment
        int count = this.rigidBody.Cast(
            new Vector2(movementX, movementY), // X and Y values between -1 and 1 that represent the direction of movement
            this.movementFilter, 
            this.castCollisions,
            moveSpeed * Time.deltaTime + this.collisionOffset
        );
        Debug.Log("Count: " + count);

        if (count == 0)
            transform.position += new Vector3(movementX * moveSpeed * Time.deltaTime, 
                movementY * moveSpeed * Time.deltaTime, 0);
    }
    
    void setWeaponsOrientation(WeaponOrientation orientation)
    {
        foreach (var weapon in weaponList)
        {
            weapon.ChangeOrientation(orientation);
        }
    }
    
    void AnimatePlayer()
    {
        var weapon = weaponList[weaponIdx];
        if (movementY > 0)
        {
            setWeaponsOrientation(WeaponOrientation.Up);
            anim.SetBool(BAKER_UP_ANIMATION, true);
        }
        else if (movementY < 0)
        {
            setWeaponsOrientation(WeaponOrientation.Down);
            anim.SetBool(BAKER_DOWN_ANIMATION, true);
        } 
        else
        {
            anim.SetBool(BAKER_DOWN_ANIMATION, false);
            anim.SetBool(BAKER_UP_ANIMATION, false);
        }

        if (movementX > 0)
        {
            setWeaponsOrientation(WeaponOrientation.Right);
            anim.SetBool(BAKER_WALK_HORIZONTAL, true); 
            sr.flipX = false;
        }
        else if (movementX < 0)
        {
            setWeaponsOrientation(WeaponOrientation.Left);
            anim.SetBool(BAKER_WALK_HORIZONTAL, true);
            sr.flipX = true;
        }
        else
        {
            anim.SetBool(BAKER_WALK_HORIZONTAL, false);
        }
    }

    public void AddCollectible(Collectible collectible)
    {
        foreach (var weapon in weaponList)
        {
            weapon.AddAmmo(collectible);
        }
    }
}
