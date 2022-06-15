using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlantEnemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int movementSpeed;
    public int distance;
    public bool verticalMovement;

    public Animator animator;
    public AudioSource source;
    public GameObject deathParticles;
    public Rigidbody2D rb;
    

    private Vector2 currentVector;
    private int currentHealth;
    private float currentPos;
    public BoxCollider2D bodyCollider;



    void Start()
    {
        currentVector = Vector2.right;
        currentHealth = maxHealth;
        rb.gravityScale = 0;
        if (verticalMovement)
        {
            currentPos = transform.position.y;
        }
        else
        {
            currentPos = transform.position.x;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SpinAttack"))
        {
            TakeDamage(100);
        }
    }
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetBool("Dead", true);

        FindObjectOfType<AudioManager>().Play("PlantDeath");
        source.mute = true;

        GameObject particle = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(particle, 5);

        rb.bodyType = RigidbodyType2D.Static;
        bodyCollider.enabled = false;
        this.enabled = false;

    }

    private void FixedUpdate()
    {
        WallPatrol();
        rb.velocity = Vector3.zero;
    }

    private void Chomp()
    {
        source.Play();
        if(bodyCollider.offset.x > 0)
        {
            bodyCollider.offset = new Vector2(-0.55f, -0.27f);
        }
        else
        {
            bodyCollider.offset = new Vector2(0.55f, -0.27f);
        }
    }

    void WallPatrol()
    {
        transform.Translate(currentVector * movementSpeed * Time.deltaTime);
        if (verticalMovement)
        {
            if (Math.Abs(transform.position.y - currentPos) >= distance)
            {
                Flip();
                currentPos = transform.position.y;
            }
        }
        else
        {
            if (Math.Abs(transform.position.x - currentPos) >= distance)
            {
                Flip();
                currentPos = transform.position.x;
            }
        }
    }

    void Flip()
    {
        if (currentVector == Vector2.right)
        {
            currentVector = Vector2.left;
        }
        else
        {
            currentVector = Vector2.right;
        }
        currentPos = transform.position.x;
    }
}
