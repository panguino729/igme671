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
    public AudioSource attackAudioSource;

    private float attackAnimationTime = 0.7f;
    private float attackTimeLeft = 0.0f;

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
        attackTimeLeft -= Time.deltaTime;
        if(attackTimeLeft <= 0 && isAttacking)
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
        }
        time -= Time.deltaTime;
        //Animation plays somewhat before the attack to choreograph it
        if(time <= 0.4)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
        }
        //Causes the ranged enemy to fire in the direction it's moving on a set time interval
        if (time <= 0)
        {
            Fire();
            time = fireInterval;
        }
        base.Update();
    }
    void Fire()
    {
        //attackAudioSource.Play();
        Bullet newBullet = Instantiate(bullet, new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z), Quaternion.identity).GetComponent<Bullet>();
        newBullet.bulletSpeed = bulletSpeed;
        newBullet.BulletDirection = moveDirection;
        newBullet.bulletDamage = attackDamage;
    }
}
