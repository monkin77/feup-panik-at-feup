using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int cost;
    
    // TODO: CHECK THIS COLLIDE OFFSETS
    // These offsets are used to determine if the enemy is in attack range
    // and are the default value on melee enemies. Ranged enemies will override these values
    [SerializeField] protected float ATTACK_RANGE = 0.3f;
    [SerializeField] protected float health = 100;
    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    
    [SerializeField] protected int ENEMY_DAMAGE = 30;
    [SerializeField] protected float speed = 1f;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField] protected GameObject baker;

    private Rigidbody2D rigidBody;
    private SpriteRenderer sr;
    
    protected Animator anim;
    protected string WALK_ANIMATION = "Walking";
    protected string BAKER_TAG = "Baker";
    protected string ATTACK_TRIGGER = "Attack";
    protected string IDLE_ANIMATION = "Idle";
    protected string DIE_ANIMATION = "Die";
    

    // Stores the settings that determine where a collision can occur. Such as layers to collide with
    [SerializeField] private ContactFilter2D movementFilter;

    // Stores the collisions that occur during movement
    [SerializeField] private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    // Collision offset to prevent the player from getting stuck in walls
    private float collisionOffset = 0.02f;

    void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        baker = GameObject.FindWithTag(BAKER_TAG);
        sr = GetComponent<SpriteRenderer>();
    }

    /**
     * Move the enemy towards the baker if it is not in attack range
     * If the enemy is in attack range, attack the baker
     */
    private void FixedUpdate() {
        // if the game is over then don't move the enemy
        if (GameManager.instance.IsGameOver) return;
        
        // if zombie is not walking then don't move towards the baker
        if (anim.GetBool(WALK_ANIMATION) == false)
            return;

        // the baker has a x and y position. Make the enemy move towards the baker
        Vector2 bakerPos = baker.transform.position;
        var bakerX = bakerPos.x;
        var bakerY = bakerPos.y;
        
        // if the enemy is in attack range then attack the baker
        if (IsInAttackRange()) {
            anim.SetBool(WALK_ANIMATION, false);
            anim.SetTrigger(ATTACK_TRIGGER);
            return;
        }

        // move towards the baker
        Vector2 newPos = Vector2.MoveTowards(transform.position, bakerPos, speed * Time.fixedDeltaTime);
        Vector2 moveVector = newPos - new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = moveVector.normalized;

        // Try to move the enemy in the given direction
        bool success = tryMove(direction, moveVector, Mathf.Max(Mathf.Abs(moveVector.x), Mathf.Abs(moveVector.y)));

        // If the initial move was diagonal and failed, try moving in the X and Y directions separately
        if (!success && (moveVector.x != 0 && moveVector.y != 0)) {
            // If the enemy couldn't move in the given direction, try moving in the X direction
            success = tryMove(new Vector2(direction.x, 0), new Vector2(moveVector.x, 0), Mathf.Abs(moveVector.x));

            if (!success) {
                // If the enemy couldn't move in the X direction, try moving in the Y direction
                success = tryMove(new Vector2(0, direction.y), new Vector2(0, moveVector.y), Mathf.Abs(moveVector.y));
            }
        }

        if (transform.position.x > bakerX)
            sr.flipX = true;
        else
            sr.flipX = false;
    }
    
    
    /**
     * Kill the enemy and remove it from the game
     * by destroying the game object. This is a trigger
     * called after the die animation
     */
    public void Die()
    {
        Destroy(gameObject);
    }

    /**
     * Set the die animation state
     */
    public void SetDieState()
    {
        anim.SetBool(DIE_ANIMATION, true);
        anim.SetBool(WALK_ANIMATION, false);
        anim.SetBool(IDLE_ANIMATION, false);
    }

    /**
     * Take damage and if the enemy's health is 0 or less then it dies
     */
    public void TakeDamage(int damage) {
        Health -= damage;
        
        if (Health <= 0)
        {
            SetDieState();
        }
    }
    
    /**
    * Transforms the enemy into a boss with better stats
    */
    public virtual void transfBoss() {
        throw new System.NotImplementedException();
    }

    /**
    * Moves the Enemy in the given direction
    * @param direction The direction to move the player in. X and Y values between -1 and 1 that represent the direction of movement
    * @param speedVec The speed to move the player at (x, y)
    * @param moveSpeed The speed to move the player at
    * @return true if the player was able to move, false if there was a collision
    */
    private bool tryMove(Vector2 direction, Vector2 speedVec, float moveSpeed) {
        // Check if there are collisions with players or walls
        int count = this.rigidBody.Cast(
            direction,
            this.movementFilter, 
            this.castCollisions,
            moveSpeed + this.collisionOffset
        );

        if (count == 0) {
            transform.position += new Vector3(
                speedVec.x, 
                speedVec.y, 
                0
            );
            return true;
        }

        return false;
    }
    
    /**
     * Check if the baker is in attack range of the enemy
     * @return true if the baker is in attack range of the enemy, false otherwise
     */
    protected bool IsInAttackRange() {
        if (baker.gameObject == null)
            return false;
        
        Vector2 bakerPos = baker.transform.position;

        var distance = Vector2.Distance(bakerPos, transform.position);
        
        return distance <= ATTACK_RANGE;
    }
}
