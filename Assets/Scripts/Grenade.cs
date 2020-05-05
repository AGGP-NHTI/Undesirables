using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Actor
{
    public GameObject explosion;
    private float damageAmount = 100.0f;
    public float movementSpeed = 5f;
    public float lifetime = 2.5f;
    public Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();        
        rb.velocity = (Vector2.up * movementSpeed) + (Vector2.right * movementSpeed);
    }

    private void Update()
    {
        lifetime = lifetime - Time.deltaTime;
        if (lifetime <= 0f)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.tag != "Player") && (other.gameObject.tag != "ground"))
        {
            Actor OtherActor = other.gameObject.GetComponentInParent<Actor>();
            if (OtherActor)
            {
                OtherActor.TakeDamage(this, damageAmount, null, Owner);
            }
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
