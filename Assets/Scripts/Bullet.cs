using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public RangedEnemy enemy; //set this at instantiation of the bullet

    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, Vector2.Angle(new Vector2(1, 0), enemy.aimTrajectory));
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.velocity = enemy.aimTrajectory * enemy.bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //checks if the bullet hits the player, and reduces health accordingly
        if(collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<Player>().health -= enemy.attackDamage;
        }

        Destroy(gameObject);
    }
}
