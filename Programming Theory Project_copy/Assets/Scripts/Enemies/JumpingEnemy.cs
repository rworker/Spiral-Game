using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : Enemy
{
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Rigidbody rb;

    public override void FixedUpdate() 
    {
        //bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        //if (isGrounded)
        //{
        //    direction.y = jumpForce;
        //}

        base.FixedUpdate();
    }

    public override void Move(Vector3 direction, float speed)
    {
        /*bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        print(isGrounded);

        if (isGrounded)
        {
            print(direction.y);
            direction.y = jumpForce; //doesn't apply gravity when on the ground
        }

        else //if not on the ground
        {
            direction.y += gravity * Time.deltaTime; //only applies gravity when not on the ground
        }*/

        //enemyController.Move(direction * Time.deltaTime * speed);
        rb.MovePosition(transform.position +(direction * speed * Time.deltaTime));

        print(direction);
        

        //base.Move(direction, speed);
    }
}
