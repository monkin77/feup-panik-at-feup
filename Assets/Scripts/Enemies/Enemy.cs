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

        transform.position = Vector2.MoveTowards(
            transform.position, new Vector2(bakerX, bakerY), speed * Time.fixedDeltaTime);

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
