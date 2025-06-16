using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private float jumpPower = 12f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Движение влево/вправо, если не в кулдауне
        if (wallJumpCooldown > 0.2f)
        {
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            // Поворот персонажа по направлению движения
            if (horizontalInput > 0.01f)
                transform.localScale = Vector3.one;
            else if (horizontalInput < -0.01f)
                transform.localScale = new Vector3(-1, 1, 1);

            // Прыжок с земли или от стены
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (IsGrounded())
                {
                    Jump();
                }
                else if (OnWall())
                {
                    WallJump();
                }
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }

        // Скользим вниз по стене, а не зависаем
        if (OnWall() && !IsGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, Mathf.Max(body.linearVelocity.y, -2f));
        }

        // Анимации
        animator.SetBool("Run", horizontalInput != 0);
        animator.SetBool("grounded", IsGrounded());
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
        animator.SetTrigger("jump");
    }

    private void WallJump()
    {
        wallJumpCooldown = 0f;

        // Направление — от стены
        float wallDir = OnRightWall() ? -1 : 1;

        // Прыжок ВВЕРХ и немного в сторону
        body.linearVelocity = new Vector2(wallDir * speed * 0.3f, jumpPower);
        animator.SetTrigger("jump");
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0f,
            Vector2.down,
            0.1f,
            groundLayer
        );
        return hit.collider != null;
    }

    private bool OnWall()
    {
        return OnLeftWall() || OnRightWall();
    }

    private bool OnLeftWall()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0f,
            Vector2.left,
            0.1f,
            wallLayer
        );
        return hit.collider != null;
    }

    private bool OnRightWall()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0f,
            Vector2.right,
            0.1f,
            wallLayer
        );
        return hit.collider != null;
    }
}
