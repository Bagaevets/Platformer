using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    
    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
  /*
    public float HorizontalInput { get { return horizontalInput; } set { horizontalInput = value; } }
    public float GetHorizontalInput() 
    { 
        return horizontalInput; 
    }
    public void SetHorizontalInput(float value) 
    {
        horizontalInput = value;
    }
   */
    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>(); //Возьмет компонент Rigidbody у прикреп. объекта       
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();    
    }
    private void Update()
    {
         horizontalInput = Input.GetAxis("Horizontal");

    

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1);

        

        //Set animator parameters
        animator.SetBool("Run", horizontalInput != 0);
        animator.SetBool("grounded", isGronded());

        // Wall jump logic
        if (wallJumpCooldown > 0.2)
        {
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (onWall() && !isGronded())
            {
                body.gravityScale = 0;
                body.linearVelocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
    
    }

    private void Jump()
    {
        if (isGronded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            animator.SetTrigger("jump");
        }
        else if (onWall() && !isGronded())
        {
            if (horizontalInput == 0)
            {
                var v = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x),transform.localScale.y, transform.localScale.z);
            }
            else 
            {
                var v = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
                body.linearVelocity = v;
                Debug.Log($"wallJump. {v}");
            }
            wallJumpCooldown = 0;
            
        }
    }

    private bool isGronded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    { 
        return horizontalInput  == 0 && isGronded() && !onWall();
    }
}   
