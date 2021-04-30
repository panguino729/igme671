using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

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
    public GameObject bullet;
    public SpriteRenderer spriteRenderer;
    public float bulletSpeed;
    public int attackType = 0;
    public int lungeCooldown;

    // Audio
    private Bus sfxBus;

    [FMODUnity.EventRef]
    public string playerAttackPath;
    [FMODUnity.EventRef]
    public string playerJumpPath;
    [FMODUnity.EventRef]
    public string playerDashPath;
	[FMODUnity.EventRef]
	public string playerHealthPath;
    [FMODUnity.EventRef]
    public string playerLandPath;
    [FMODUnity.EventRef]
    public string playerRangedAttackPath;
    [FMODUnity.EventRef]
    public string playerDamagePath;

    [FMODUnity.EventRef]
    public string whispersPath;

    private EventInstance playerAttack;
    private EventInstance playerJump;
    private EventInstance playerDash;
	private EventInstance playerHealth;
    private EventInstance playerLand;
    private EventInstance playerRangedAttack;
    private EventInstance playerDamage;

    private EventInstance whispers;

    EventDescription playerHealthD;
    PARAMETER_DESCRIPTION playerHealthPD;
    PARAMETER_ID playerHealthPID;

    //The time it takes to go back to the walking animation after attacking
    private float attackAnimationTime = 0.5f;
    private float attackTimeLeft = 0.5f;
    private bool grounded = true;
    private bool lungeing = false;
    private int lungeFrames = 7;
    private int currLungeCooldown = 0;
    private int lungeCounter = 0;
    private bool facing = true;

    void Start()
    {
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");

        playerAttack = FMODUnity.RuntimeManager.CreateInstance(playerAttackPath);
        playerJump = FMODUnity.RuntimeManager.CreateInstance(playerJumpPath);
        playerDash = FMODUnity.RuntimeManager.CreateInstance(playerDashPath);
		playerHealth = FMODUnity.RuntimeManager.CreateInstance(playerHealthPath);
        playerLand = FMODUnity.RuntimeManager.CreateInstance(playerLandPath);
        playerRangedAttack = FMODUnity.RuntimeManager.CreateInstance(playerRangedAttackPath);
        playerDamage = FMODUnity.RuntimeManager.CreateInstance(playerDamagePath);


        playerHealth.getDescription(out playerHealthD);
        playerHealthD.getParameterDescriptionByName("playerHealth", out playerHealthPD);
        playerHealthPID = playerHealthPD.id;

        whispers = FMODUnity.RuntimeManager.CreateInstance(whispersPath);

		playerHealth.start();
        whispers.start();

        Physics2D.IgnoreLayerCollision(18, 19, true);
        if (lungeCooldown == 0)
        {
            lungeCooldown = 10;
        }
        animator = GetComponent<Animator>();
        initialXScale = transform.localScale.x;
        player = gameObject;
        base.Start();
        //xOffset = spriteRenderer.bounds.max.x - spriteRenderer.bounds.center.x;
    }

    void FixedUpdate()
    {
        //playerHealth.setParameterByName("playerHealth", (currHealth / maxHealth
        float health = currHealth / maxHealth;
        playerHealth.setParameterByID(playerHealthPID, (float)health);

        float num;
        playerHealth.getParameterByID(playerHealthPID, out num);
        Debug.Log(num);

        if(lungeCooldown > 0)
        {
            lungeCooldown--;
        }
        if(Math.Abs(rigidbody.velocity.x) <= 0.1 || !grounded)
        {
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }
        attackTimeLeft -= Time.deltaTime;
        //Once the attack animation has finished playing, return the animation to normal
        if(isAttacking && attackTimeLeft <= 0)
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
        }
        //If the player dies, reloads the scene
        if(currHealth <= 0)
        {
            Scene currScene = SceneManager.GetActiveScene();
            StopSound();
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
            //xOffset = spriteRenderer.bounds.max.x - spriteRenderer.bounds.center.x;
            Attack();
        }

        if (!lungeing && Input.GetMouseButtonDown(1) && currLungeCooldown <= 0)
        {
            //lungeAudioSource.Play();
            playerDash.start();
            currLungeCooldown = lungeCooldown;
            animator.SetBool("isDodging", true);
            lungeing = true;
            lungeCounter = lungeFrames;
        }

        if (lungeing)
        {
            Lunge();
        }

        //fix hover glitch
        if(rigidbody.velocity.y != 0)
        {
            grounded = false;
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
            //jumpAudioSource.Play();
            playerJump.start();
            rigidbody.AddForce(new Vector2(0, jumpForce));
            
            grounded = false;
            animator.SetBool("isJumping", true);
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
            playerLand.start();
            animator.SetBool("isJumping", false);
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
        //attackAudioSource.Play();
        //play an animation
        animator.SetBool("isAttacking", true);
        attackTimeLeft = attackAnimationTime;
        isAttacking = true;
        //detect enemies
        if (attackType == 0)
        {
            playerAttack.start();

            //detect enemies
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject.tag == "enemy")
                {
                    hit.gameObject.GetComponent<Enemy>().TakeDamage(attackDamage);
                }
            }
        }
        if(attackType == 1)
        {
            playerRangedAttack.start();

            Bullet newBullet = Instantiate(bullet, new Vector3(transform.position.x /*+ xOffset*/, transform.position.y, transform.position.z), Quaternion.identity).GetComponent<Bullet>();
            newBullet.bulletSpeed = bulletSpeed;
            newBullet.BulletDirection = facing ? new Vector2(1,0) : new Vector2(-1,0);
            newBullet.bulletDamage = attackDamage;
            newBullet.isPlayers = true;
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
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            //Make collisions happen after dash is done
            Physics2D.IgnoreLayerCollision(18, 17, false);
            Physics2D.IgnoreLayerCollision(18, 16, false);
            lungeing = false;
            animator.SetBool("isDodging", false);
        }
    }

    public void TakeDamage(float damage)
    {
        playerDamage.start();
        base.TakeDamage(damage);
    }

    public void StopSound()
    {
        playerHealth.stop(STOP_MODE.IMMEDIATE);
        playerHealth.release();

        whispers.stop(STOP_MODE.ALLOWFADEOUT);
        whispers.release();

        //sfxBus.stopAllEvents(STOP_MODE.ALLOWFADEOUT);

        Debug.Log("stop sound");
    }
}
