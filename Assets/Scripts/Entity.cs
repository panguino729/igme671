using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rigidbody;
    protected BoxCollider2D boxCollider;

    public float attackDamage;
    public float currHealth;
    public float maxHealth = 1.0f;

    private SpriteRenderer spr;
    private int iFrames = 0; //Invincibility frames after taking damage

    // Start is called before the first frame update
    protected void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        currHealth = maxHealth;
    }

    // Update is called once per frame
    public void Update()
    {
        if (spr.color == Color.red)
        {
            if (iFrames <= 0)
            {
                spr.color = Color.white;
            }
            else
            {
                iFrames--;
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (iFrames <= 0)
        {
            currHealth -= damage;
            spr.color = Color.red;
            iFrames = 40;
        }
    }
}
