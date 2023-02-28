using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int cost;

    [SerializeField] private int health = 100;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }
    
    [SerializeField] protected int ENEMY_DAMAGE = 30;
    [SerializeField] private float speed = 1f;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField] protected GameObject baker;

    private Rigidbody2D myBody;
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

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        baker = GameObject.FindWithTag(BAKER_TAG);
        sr = GetComponent<SpriteRenderer>();
    }

    /**
     * Move the enemy towards the baker if it is not in attack range
     * If the enemy is in attack range, attack the baker
     */
    private void FixedUpdate()
    {
        // if baker is dead then stop chasing her
        if (!baker)
        {
            anim.SetBool(IDLE_ANIMATION, true);
            anim.SetBool(WALK_ANIMATION, false);
            return;
        }
        
        // if zombie is not walking then don't move towards the baker
        if (anim.GetBool(WALK_ANIMATION) == false)
            return;

        // the baker has a x and y position. Make the enemy move towards the baker
        Vector2 bakerPos = baker.transform.position;
        var bakerX = bakerPos.x;
        var bakerY = bakerPos.y;
        
        // if the enemy is in attack range then attack the baker
        if (IsInAttackRange())
        {
            anim.SetBool(WALK_ANIMATION, false);
            anim.SetTrigger(ATTACK_TRIGGER);
            return;
        }

        // move towards the baker
        Vector2 newPos = Vector2.MoveTowards(transform.position, bakerPos, speed * Time.fixedDeltaTime);
        
        Vector2 direction = newPos - new Vector2(transform.position.x, transform.position.y);

        // Check if there are collisions
        int count = this.myBody.Cast(
            direction, 
            this.movementFilter,
            this.castCollisions, 
            this.collisionOffset
        );
        if (count == 0) {
            // no collisions
            transform.position = newPos;
        }


        if (transform.position.x > bakerX)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    /**
     * Abstract method to Check if the enemy is in attack range of the baker.
     */
    protected virtual bool IsInAttackRange()
    {
        throw new System.NotImplementedException();
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
    public void TakeDamage(int damage)
    {
        Health -= damage;
        
        if (Health <= 0)
        {
            SetDieState();
        }
    }
    
}
