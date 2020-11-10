using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    private RaycastHit2D rayHit;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    //The offset from the center of the enemy for the raycast - the raycast's origin is from the right of the enemy if the enemy is moving right, and vice versa.
    private float xOffset;
    //The layer of the platforms
    private LayerMask lm;
    //The direction in which the object moves - will be reversed
    public Vector2 moveDirection; 
    public float moveMagnitude; //The speed at which the object moves
    // Start is called before the first frame update
    void Start()
    {
        if (moveDirection == new Vector2(0, 0))
        {
            moveDirection = new Vector2(1, 0);
        }
        if (moveMagnitude == 0)
        {
            moveMagnitude = 15;
        }
        lm = (1 << LayerMask.NameToLayer("Platform"));
        moveDirection = moveDirection.normalized;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (moveDirection.x > 0)
        {
            xOffset = spriteRenderer.size.x;
        }
        else
        {
            moveDirection.x = -spriteRenderer.size.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //If the enemy is colliding with a platform, check if it has reached the edge of that platform
        if (collision.gameObject.tag.Equals("platform"))
        {
            //The distance of the raycast - short so that it doesn't detect platforms below the initial one
            float distance = 1;
            //The origin of the raycast - starts from the bottom left or right of object to determine  if the object is about to reach the edge of the platform
            Vector2 origin = new Vector2(transform.position.x + xOffset, transform.position.y - spriteRenderer.size.y); 
            rayHit = Physics2D.Raycast(origin, -Vector3.up, distance, lm); //Check if the object has reached the edge of the platform
            //If the enemy reached the edge of the platform, reverses its direction
            if(rayHit.collider != null)
            {
                Debug.Log("reached2");
            }
            if(rayHit.collider == null)
            {
                moveDirection = new Vector2(moveDirection.x * -1, moveDirection.y * -1);
                Debug.Log("reached1");
            }
        }
        rb.velocity = moveDirection * moveMagnitude;
    }
}
