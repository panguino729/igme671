using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class FlyingRangedEnemy : RangedEnemy
{
    //If the distance to the player is less than the range, can potentially fire
    public float range;
    //Will check if there is a line of sight for firing at the player
    private RaycastHit2D toPlayer;
    private float distToPlayer;
    //Enemy will move between points on a path - if 1 is input, moves between that point and a point to its left. 
    //If none are put in, moves between its starting location and a point to the left of that.
    public List<Vector3> path;
    public AudioSource attackAudioSource;
    //The index of the point in the path the enemy is seeking
    private int targetIndex;
    private float initialXScale;

    // Audio
    [FMODUnity.EventRef]
    public string flyingAttackPath;
    [FMODUnity.EventRef]
    public string flyingDefeatPath;
    [FMODUnity.EventRef]
    public string flyingDamagePath;
    [FMODUnity.EventRef]
    public string flyingMovementPath;

    private EventInstance flyingAttack;
    private EventInstance flyingDefeat;
    private EventInstance flyingDamage;
    private EventInstance flyingMovement;

    // Start is called before the first frame update
    void Start()
    {
        flyingMovement = FMODUnity.RuntimeManager.CreateInstance(flyingMovementPath);
        flyingDamage = FMODUnity.RuntimeManager.CreateInstance(flyingDamagePath);
        flyingDefeat = FMODUnity.RuntimeManager.CreateInstance(flyingDefeatPath);

        flyingMovement.start();

        spr = GetComponent<SpriteRenderer>(); 
        rigidbody = GetComponent<Rigidbody2D>();
        initialXScale = transform.localScale.x;
        if(range == 0)
        {
            range = 10;
        }
        if(path.Count == 0)
        {
            path.Add(transform.position);
        }
        if(path.Count == 1)
        {
            path.Add(new Vector3(path[0].x - 5, path[0].y, 0));
        }
        if (attackDamage == 0)
        {
            attackDamage = 5;
        }
        if (bulletSpeed == 0)
        {
            bulletSpeed = 10;
        }
        if (fireInterval == 0)
        {
            fireInterval = 1.5f;
        }
        if (moveMagnitude == 0)
        {
            moveMagnitude = 15;
        }
        //The layer consisting of platforms
        lm = (1 << LayerMask.NameToLayer("Platform"));
    }

    // Update is called once per frame
    void Update()
    {
        if (currHealth <= 0)
        {
            flyingMovement.stop(STOP_MODE.IMMEDIATE);
            flyingDefeat.start();
            Destroy(gameObject);
        }
        Move();
        time -= Time.deltaTime;
        //Causes the ranged enemy to check if it can fire on a time interval
        if (time <= 0)
        {
            distToPlayer = (gameObject.transform.position - Player.player.transform.position).magnitude;
            //If enemy is in range, it can fire
            if (distToPlayer <= range)
            {
                Fire();
            }
        }
    }
    void Fire()
    {
        //Raycasts from the flying enemy to the player to check for platforms in between
        toPlayer = Physics2D.Raycast(gameObject.transform.position, Player.player.transform.position - gameObject.transform.position, distToPlayer, lm);
        //If there is no platform in the way, fires
        if (toPlayer.collider == null)
        {
            //attackAudioSource.Play();
            Bullet newBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity).GetComponent<Bullet>();
            newBullet.bulletSpeed = bulletSpeed;
            newBullet.BulletDirection = (Player.player.transform.position - gameObject.transform.position).normalized;
            newBullet.bulletDamage = attackDamage;
            time = fireInterval;
            return;
        }
        //If there is a platform in the way, waits a little bit to make the check again
        time = 0.5f;
    }
    void Move()
    {
        //The vector from the current position to the target position
        Vector3 vToTarget = path[targetIndex] - gameObject.transform.position;
        //If the enemy reaches its target, changes its target to the next in the list
        if (vToTarget.magnitude <= 1)
        {
            if(targetIndex == path.Count - 1)
            {
                targetIndex = 0;
            }
            else
            {
                targetIndex++;
            }
        }
        rigidbody.velocity = vToTarget.normalized * moveMagnitude;
        //Reverses the enemy's direction if applicable
        if(rigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-initialXScale, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(initialXScale, transform.localScale.y, transform.localScale.z);
        }
    }

    public void TakeDamage(float damage)
    {
        flyingDamage.start();
        base.TakeDamage(damage);
    }
}
