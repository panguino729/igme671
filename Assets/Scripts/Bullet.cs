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
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(bulletSpeed == 0)
        {
            bulletSpeed = 10;
        }
        rb.velocity = bulletDirection * bulletSpeed;
        transform.rotation = Quaternion.Euler(0, 0, math.atan2(bulletDirection.y, bulletDirection.x));
    }

    // Update is called once per frame
    void Update()
    {
        //We should put something to delete bullets that are out of bounds eventually
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //checks if the bullet hits the player, and reduces health accordingly
        if(collision.gameObject.tag == "player")
        {
            collision.gameObject.GetComponent<Player>().health -= bulletDamage;
        }
        if (collision.gameObject.tag != "enemy")
        {
            Destroy(gameObject);
        }
    }
}
