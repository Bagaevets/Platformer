using System.Runtime.CompilerServices;
using UnityEngine;
//������
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;

    private void Awake() // Hey
    {
        body = GetComponent<Rigidbody2D>(); //������� ��������� Rigidbody � �������. �������       
    }

    private void Update()
    {
        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal"),body.linearVelocity.y);   
    }

}   
