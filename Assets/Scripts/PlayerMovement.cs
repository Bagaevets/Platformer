using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator animator;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>(); //Возьмет компонент Rigidbody у прикреп. объекта       
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        body.linearVelocity = new Vector2(horizontalInput * speed,body.linearVelocity.y);

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1);

        if (Input.GetKey(KeyCode.Space)) // Проверяет Нажатие клавиши
            body.linearVelocity = new Vector2(body.linearVelocity.x, speed);

        //Set animator parameters
        animator.SetBool("Run", horizontalInput != 0);
    }

}   
