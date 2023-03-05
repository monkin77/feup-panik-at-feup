using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;
    
    [SerializeField] private int health = 100;
    // max health of the player
    [SerializeField] private int maxHealth = 200;
    // health increment when the player collects a panike
    [SerializeField] private int healthIncrement = 50;

    // max number of panikes
    [SerializeField] private int maxPanikes = 5;
    // Current number of panikes
    [SerializeField] private int _currPanikes = 0;
    
    private Rigidbody2D rigidBody;
    private SpriteRenderer sr;
    
    private Animator anim;

    private float movementX;
    private float movementY;
    
    [SerializeField] private int weaponIdx = 0;
    [SerializeField] private List<Weapon> weaponList;
    private int _weaponCount;

    // Stores the settings that determine where a collision can occur. Such as layers to collide with
    [SerializeField] private ContactFilter2D movementFilter;

    // Stores the collisions that occur during movement
    [SerializeField] private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    // Collision offset to prevent the player from getting stuck in walls
    [SerializeField] private float collisionOffset = 0.01f;

    // HealthBar reference
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private WeaponSwitcher weaponSwitcher;

    // Reference to the Panike Counter Text UI object
    [SerializeField] private TextMeshProUGUI panikeCounterText;

    private void Awake()
    {
        this.rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        sr = GetComponent<SpriteRenderer>();
        weaponList = new List<Weapon>();

        // Set the max health in the UI
        this.healthBar.setMaxHealth(this.maxHealth);
        this.healthBar.SetHealth(this.health); 
    }

    void Start() {
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
    void ChangeWeaponKb() {
        var weapon = weaponList[weaponIdx];
        if (Input.GetKeyDown(KeyCode.Q)) {
            weapon.gameObject.SetActive(false);
            if (weaponIdx > 0)
                weaponIdx--;
            else
            {
                weaponIdx = _weaponCount - 1;
            }
            weaponList[weaponIdx].gameObject.SetActive(true);

            // Cycle the weapon in the UI
            this.weaponSwitcher.cycleWeapon();
        } 
        else if (Input.GetKeyDown(KeyCode.E)) {
            // next weapon
            weapon.gameObject.SetActive(false);
            if (weaponIdx < _weaponCount - 1)
                weaponIdx++;
            else
                weaponIdx = 0;
            weapon = weaponList[weaponIdx];
            weapon.gameObject.SetActive(true);

            // Cycle the weapon in the UI
            this.weaponSwitcher.cycleWeapon();
        }
    }

    /**
     * Checks if attack key was pressed
     */
    void AttackKeyboard()
    {
        var weapon = weaponList[weaponIdx];
        foreach (var wpn in weaponList)
        {
            if (wpn.IsAttacking)
                return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            weapon.IsAttacking = true;
        } else if (Input.GetKeyDown(KeyCode.R)) {
            // if there are no panikes player can't power up
            if (_currPanikes == 0)
                return;

            this.setCurrPanikes(this._currPanikes - 1);

            weapon.IsPowerUp = true;
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

        // Try to move the player in the given direction
        bool success = tryMove(new Vector2(movementX, movementY), speedX, speedY);

        // If the initial move was diagonal and failed, try moving in the X and Y directions separately
        if (!success && (movementX != 0 && movementY != 0)) {
            // If the player couldn't move in the given direction, try moving in the X direction
            success = tryMove(new Vector2(movementX, 0), speedX, 0);

            if (!success) {
                // If the player couldn't move in the X direction, try moving in the Y direction
                success = tryMove(new Vector2(0, movementY), 0, speedY);
            }
        }
        
    }

    /**
    * Moves the player in the given direction
    * @param direction The direction to move the player in. X and Y values between -1 and 1 that represent the direction of movement
    * @param speedX The speed to move the player in the X direction
    * @param speedY The speed to move the player in the Y direction
    * @return true if the player was able to move, false if there was a collision
    */
    private bool tryMove(Vector2 direction, float speedX, float speedY) {
        // Check if there are collisions with the environment
        int count = this.rigidBody.Cast(
            direction,
            this.movementFilter, 
            this.castCollisions,
            moveSpeed * Time.deltaTime + this.collisionOffset
        );

        if (count == 0) {
            transform.position += new Vector3(
                speedX, 
                speedY, 
                0
            );
            return true;
        }

        return false;
    }
    
    /**
     * Changes the orientation of each weapon to the desired orientatio
     */
    void setWeaponsOrientation(WeaponOrientation orientation)
    {
        foreach (var weapon in weaponList)
        {
            weapon.ChangeOrientation(orientation);
        }
    }
    
    /**
     * Animates the player
     * To do so, rotates the weapons to the respective orientations
     * and starts the animation
     */
    void AnimatePlayer()
    {
        var weapon = weaponList[weaponIdx];
        if (movementY > 0)
        {
            setWeaponsOrientation(WeaponOrientation.Up);
            anim.SetBool(Utils.BAKER_UP_ANIMATION, true);
        }
        else if (movementY < 0)
        {
            setWeaponsOrientation(WeaponOrientation.Down);
            anim.SetBool(Utils.BAKER_DOWN_ANIMATION, true);
        } 
        else
        {
            anim.SetBool(Utils.BAKER_DOWN_ANIMATION, false);
            anim.SetBool(Utils.BAKER_UP_ANIMATION, false);
        }

        if (movementX > 0)
        {
            setWeaponsOrientation(WeaponOrientation.Right);
            anim.SetBool(Utils.BAKER_WALK_HORIZONTAL, true); 
            sr.flipX = false;
        }
        else if (movementX < 0)
        {
            setWeaponsOrientation(WeaponOrientation.Left);
            anim.SetBool(Utils.BAKER_WALK_HORIZONTAL, true);
            sr.flipX = true;
        }
        else
        {
            anim.SetBool(Utils.BAKER_WALK_HORIZONTAL, false);
        }
    }

    /**
     * Adds a collectible to the user.
     * The collectibles of this game are ammo for the
     * rpg weapon
     */
    public void AddCollectible(Collectible collectible) {
        foreach (var weapon in weaponList) {
            weapon.AddAmmo(collectible);
        }
    }
    
    /**
     * Called when the player is hit by an enemy or projectile
     * @param damage The amount of damage the player takes
     */
    public void TakeDamage(int damage) {
        this.health = Mathf.Max(this.health - damage, 0);

        // Update the health bar
        this.healthBar.SetHealth(this.health);

        if (health <= 0) {
            // TODO: do something when the player dies
            Destroy(this.gameObject);
            GameManager.instance.GameOver();
        }
    }

    /**
    Called when the player consumes a Panike
    */
    public void EatPanike() {
        // Increment the player's health
        this.health = Mathf.Min(this.maxHealth, this.health + this.healthIncrement);

        // Update the health bar
        this.healthBar.SetHealth(this.health);

        // Increment the number of panikes
        this.setCurrPanikes(this._currPanikes + 1);
    }


    /**
    * Checks if the currently equipped weapon uses ammo
    * Currently, only the RPG uses ammo (index 1)
    */
    private bool weaponUsesAmmo() {
        return this.weaponIdx == 1;
    }

    /**
    * Auxiliary function to set the current number of panikes and update the UI
    */
    private void setCurrPanikes(int newPanikesNum) {
        int aux = newPanikesNum;
        if (newPanikesNum < 0 ) {
            aux = 0;
        } else if (newPanikesNum > this.maxPanikes) {
            aux = this.maxPanikes;
        }

        this._currPanikes = aux;

        // Update the UI
        this.panikeCounterText.text = Utils.createAmmoText(this._currPanikes);
    }
}
