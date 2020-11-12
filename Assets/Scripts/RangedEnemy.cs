using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public GameObject bullet;
    public float bulletSpeed;

    public float fireInterval;
    public float time;

    private void Start()
    {
        if(attackDamage == 0)
        {
            attackDamage = 5;
        }
        if(bulletSpeed == 0)
        {
            bulletSpeed = 10;
        }
        if(fireInterval == 0)
        {
            fireInterval = 1.5f;
        }
        time = fireInterval;
        base.Start();
    }
    private void Update()
    {
        time -= Time.deltaTime;
        //Causes the ranged enemy to fire in the direction it's moving on a set time interval
        if(time <= 0)
        {
            Fire();
            time = fireInterval;
        }
        base.Update();
    }
    void Fire()
    {
        Bullet newBullet = Instantiate(bullet, new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z), Quaternion.identity).GetComponent<Bullet>();
        newBullet.bulletSpeed = bulletSpeed;
        newBullet.bulletDirection = moveDirection;
        newBullet.bulletDamage = attackDamage;
    }
}
