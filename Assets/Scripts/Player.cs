using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    //To allow for easy access of the player's position, etc
    public static GameObject player;
    public static Player pl;
    public float moveForce = 0.0f;
    public float maxSpeed = 0.0f;
    public float airSpeedMult = 0.0f;
    public float jumpForce = 0.0f;
    public float initialXScale;
    public Transform attackPoint;
    public float attackRange = 0.0f;
    public float friction = 0.0f;
    public float lungeForce = 0.0f;

    private bool grounded = true;
    private bool lungeing = false;
    private int lungeFrames = 15;
    private int lungeCounter = 0;
    private bool facing = true;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        pl = this;
        initialXScale = transform.localScale.x;
        player = gameObject;
        base.Start();
    }

    void FixedUpdate()
    {
        //If the player dies, reloads the scene
        if(currHealth <= 0)
        {
            Scene currScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currScene.name);
        }
        //move the player
        if (!lungeing)
        {
            Move();
        }

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

        if(Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (!lungeing && Input.GetMouseButtonDown(1))
        {
            lungeing = true;
            lungeCounter = lungeFrames;
        }

        if (lungeing)
        {
            Lunge();
        }

    }

    private Vector2 GetDirection()
    {
        Vector2 direction = new Vector2(0, 0); 
        if (Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector3(-initialXScale, transform.localScale.y, transform.localScale.z);
            direction.x--;
            facing = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.localScale = new Vector3(initialXScale, transform.localScale.y, transform.localScale.z);
            direction.x++;
            facing = true;
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
        //apply friction
        else if(grounded)
        {
            rigidbody.velocity = rigidbody.velocity * friction;
        }
    }

    private void Attack()
    {
        //play an animation
        animator.SetBool("isAttacking", true);
        //detect enemies
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach(Collider2D hit in hits)
        {
            if(hit.gameObject.tag == "enemy")
            {
                hit.gameObject.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
    }

    private void Lunge()
    {
        //Ignore collisions with enemies during dash
        Physics2D.IgnoreLayerCollision(18, 17, true);
        //Ignore collisions with bullets during dash
        Physics2D.IgnoreLayerCollision(18, 16, true);
        //apply lunge force
        rigidbody.velocity = new Vector2((facing? 1 : -1) * lungeForce, rigidbody.velocity.y);

        //decrement frame counter
        lungeCounter--;
        if(lungeCounter == 0)
        {
            //Make collisions happen after dash is done
            Physics2D.IgnoreLayerCollision(18, 17, false);
            Physics2D.IgnoreLayerCollision(18, 16, false);
            lungeing = false;
        }
    }
}
