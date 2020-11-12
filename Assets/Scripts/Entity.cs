using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rigidbody;
    protected BoxCollider2D boxCollider;

    public float attackDamage;
    public float currHealth = 0.0f;
    public float maxHealth;

    // Start is called before the first frame update
    protected void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
