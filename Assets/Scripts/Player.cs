using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : Entity
{
    //To allow for easy access of the player's position, etc.
    public static GameObject player;
    public float moveForce = 0.0f;
    public float maxSpeed = 0.0f;
    public float airSpeedMult = 0.0f;
    public float jumpForce = 0.0f;
    public Transform attackPoint;
    public float attackRange = 0.0f;

    private bool grounded = true;

    void Start()
    {
        player = gameObject;
        base.Start();
    }

    void FixedUpdate()
    {
        //move the player
        Move();

        //if player is grounded check for jumping
        if (grounded)
        {
            Jump();
        }
        //else check to see when the player becomes grounded
        else
        {
            CheckGrounded();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }
    }

    private Vector2 GetDirection()
    {
        Vector2 direction = new Vector2(0, 0); 
        if (Input.GetKey(KeyCode.A))
        {
            direction.x--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x++;
        }
        return direction;
    }

    private void Jump()
    {
        //the player is on the ground and presses jump
        if(grounded && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)))
        {
            rigidbody.AddForce(new Vector2(0, jumpForce));
            
            grounded = false;
        }
    }

    private void CheckGrounded()
    {
        //cast a small box under the player to check if they are grounded
        Vector2 size = boxCollider.bounds.size;
        size.x *= 0.05f;

        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, size, 0f, Vector2.down, .03f, Physics2D.AllLayers, 0, 0);
        if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.tag == "platform")
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    private void Move()
    {
        Vector2 direction = GetDirection();

        if (direction.x != 0)
        {
            //player is on the ground
            if (grounded)
            {
                //player is not moving
                if (rigidbody.velocity.x == 0)
                {
                    rigidbody.AddForce(direction * moveForce);
                }
                //the player is turning around
                else if (rigidbody.velocity.x * direction.x < 0)
                {
                    rigidbody.velocity = -rigidbody.velocity * 0.7f;
                }
                //cap the speed
                else if (math.abs(rigidbody.velocity.x) > maxSpeed)
                {
                    rigidbody.velocity = new Vector2(direction.x * maxSpeed, rigidbody.velocity.y);
                }
                //move as normal
                else
                {
                    rigidbody.AddForce(direction * moveForce);
                }
            }
            //player is in the air
            else
            {
                //player is not moving
                if (rigidbody.velocity.x == 0 && direction.x != 0)
                {
                    rigidbody.AddForce(direction * moveForce * airSpeedMult);
                }
                //cap the speed
                else if (math.abs(rigidbody.velocity.x) > maxSpeed)
                {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x / math.abs(rigidbody.velocity.x) * maxSpeed, rigidbody.velocity.y);
                }
                //move as normal
                else
                {
                    rigidbody.AddForce(direction * moveForce * airSpeedMult);
                }
            }
        }
    }

    private void Attack()
    {
        //play an animation

        //detect enemies
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach(Collider2D hit in hits)
        {
            if(hit.gameObject.tag == "enemy")
            {
                hit.gameObject.GetComponent<Enemy>().currHealth -= attackDamage;
            }
        }
    }
}
