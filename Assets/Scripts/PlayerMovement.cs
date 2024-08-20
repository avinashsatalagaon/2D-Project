 
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField]private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        //Grab references for Rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        

        //flip player when moving right-left
        if(horizontalInput>0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        

        //set animation parameter
        anim.SetBool("run",horizontalInput!=0);
        anim.SetBool("grounded",isGrounded());

        //wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            //with speed player to move both direction
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            //when user jump on wall he have to stuck and jump avoid fall down
            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;

            //to jump with current speed on x axis if key entered jump to y axis 
            if (Input.GetKey(KeyCode.Space))
                jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
     }

    private void jump()
    {
        if (isGrounded()) 
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if(onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                //flip the player in opposite direction when he jumps away from the wall
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z); 

            }
            else
                //on wall forceback(3) and jump upwards(6)
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            wallJumpCooldown = 0;
        }
    }
 

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider !=null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,new Vector2(transform.localScale.x,0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        //player can attack if this returns false player cant attack
        return horizontalInput ==0 && isGrounded() && !onWall();    
    }
}
