using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{   //set these at instantiation of the bullet
    public Vector2 bulletDirection;
    public float bulletSpeed;
    public float bulletDamage;
    //When this is 0, the bullet will be deleted so that there aren't an ever increasing # of them - can be changed if bullets need to stay longer
    public float timeLeft; 
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(timeLeft == 0)
        {
            timeLeft = 3;
        }
        if(bulletSpeed == 0)
        {
            bulletSpeed = 10;
        }
        rb.velocity = bulletDirection * bulletSpeed;
        int added = 0; //Since atan2 will only return an angle in a range of 180 degrees, the bullet's rotation needs to be reversed if the angle is in the other 180 degrees.
        if(bulletDirection.x > 0)
        {
            added = 180;
        }
         transform.rotation = Quaternion.Euler(0, 0, math.atan2(bulletDirection.y, bulletDirection.x) + 90 + added);
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //checks if the bullet hits the player, and reduces health accordingly
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().currHealth -= bulletDamage;
        }
        if (collision.gameObject.tag != "enemy")
        {
            Destroy(gameObject);
        }
    }
}
