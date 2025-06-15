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

    
    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>(); //Возьмет компонент Rigidbody у прикреп. объекта       
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();    
    }
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        body.linearVelocity = new Vector2(horizontalInput * speed,body.linearVelocity.y); // Vector?

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1);

        if (Input.GetKey(KeyCode.Space) && isGronded()) // Проверяет Нажатие клавиши
            Jump();

        //Set animator parameters
        animator.SetBool("Run", horizontalInput != 0);
        animator.SetBool("grounded", isGronded());

        // Wall jump logic
        if (wallJumpCooldown > 0.2)
        {
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (onWall() && isGronded())
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
        if (!isGronded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            animator.SetTrigger("jump");
        }
        else if (onWall() && isGronded())
        {
            wallJumpCooldown = 0;
            body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
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
}   
