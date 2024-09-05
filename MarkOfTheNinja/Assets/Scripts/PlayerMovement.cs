using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;

    CapsuleCollider2D myBodyCollider;
    public bool isAlive = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        /*Si no hay input 
        if(moveInput.x == 0)
        {
           myRigidbody.velocity= new Vector2((myRigidbody.velocity.x*0.8f), myRigidbody.velocity.y);
        }*/
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, 0/*myRigidbody.velocity.y*/);
        myRigidbody.AddForce(playerVelocity, ForceMode2D.Force);
        //myRigidbody.velocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        bool isTouchingGround = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if(!isTouchingGround) 
        {
            return;
        }
        if(value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }
}
