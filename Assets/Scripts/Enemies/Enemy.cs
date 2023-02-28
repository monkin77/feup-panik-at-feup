using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private int ENEMY_DAMAGE = 30;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField] private GameObject baker;

    private Rigidbody2D myBody;

    private SpriteRenderer sr;
    
    private Animator anim;
    private string WALK_ANIMATION = "Walking";
    private string BAKER_TAG = "Baker";
    private string ATTACK_TRIGGER = "Attack";
    private string IDLE_ANIMATION = "Idle";
    private string DIE_ANIMATION = "Die";
    
    private float xCollideOffset = 0.25f;
    private float yCollideOffset = 0.10f;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        baker = GameObject.FindWithTag(BAKER_TAG);
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!baker)
        {
            anim.SetBool(IDLE_ANIMATION, true);
            anim.SetBool(WALK_ANIMATION, false);
            return;
        }

        // the baker has a x and y position. Make the enemy move towards the baker
        Vector2 bakerPos = baker.transform.position;
        var bakerX = bakerPos.x;
        var bakerY = bakerPos.y;
        
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
     * Check if the enemy is in attack range of the baker
     */
    private bool IsInAttackRange()
    {
        if (baker.gameObject == null)
        {
            print("SHE DIED DUDE");
            return false;
        }

        Vector2 bakerPos = baker.transform.position;
        var bakerX = bakerPos.x;
        var bakerY = bakerPos.y;
        
        var position = transform.position;
        var xPos = position.x;
        var yPos = position.y;
        
        return bakerX - xCollideOffset < xPos && xPos < bakerX + xCollideOffset &&
               bakerY - yCollideOffset < yPos && yPos < bakerY + yCollideOffset;
    }
    
    /**
     * Attack the baker.
     * This function is called after the attack animation as a trigger
     * and takes damage to the baker in case she is in attack range
     */
    private void AttackBaker()
    {
        if (IsInAttackRange())
            baker.GetComponent<Player>().TakeDamage(ENEMY_DAMAGE);
        else 
            anim.SetBool(WALK_ANIMATION, true);
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }


    public void SetDieState()
    {
        anim.SetBool(DIE_ANIMATION, true);
    }
    
}
