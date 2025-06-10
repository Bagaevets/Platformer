using System.Runtime.CompilerServices;
using UnityEngine;
//прекол
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;

    private void Awake() // Hey
    {
        body = GetComponent<Rigidbody2D>(); //Возьмет компонент Rigidbody у прикреп. объекта       
    }

    private void Update()
    {
        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal"),body.linearVelocity.y);   
    }

}   
