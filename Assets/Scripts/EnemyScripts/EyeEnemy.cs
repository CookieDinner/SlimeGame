using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EyeEnemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int flySpeed;
    public int distance;

    public Animator animator;
    public AudioSource source;
    public GameObject deathParticles;
    public Rigidbody2D rb;
    public Transform groundDetection;

    private Vector2 currentVector;
    private int currentHealth;
    private float currentY;
    public BoxCollider2D bodyCollider;
    public LayerMask wallLayer;
    private bool isDead;



    void Start()
    {
        currentY = transform.position.y;
        currentHealth = maxHealth;
        rb.gravityScale = 0;
        currentVector = Vector2.up;
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

        FindObjectOfType<AudioManager>().Play("EyeDeath");
        source.mute = true;

        GameObject particle = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(particle, 5);

        isDead = true;

        rb.gravityScale = 3;
        bodyCollider.enabled = false;

    }

    private void FixedUpdate()
    {
        AirPatrol();
        if (!isDead)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            Debug.Log(rb.velocity.y);
            if (rb.velocity.y < -7f)
            {
                rb.velocity = new Vector2(0, -7f);
            }
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.05f);
            if (groundInfo.collider && groundInfo.collider.tag == "Wall")
            {
                rb.bodyType = RigidbodyType2D.Static;
                GetComponent<Collider2D>().enabled = false;
                this.enabled = false;
            }
        }
    }

    void AirPatrol()
    {
        transform.Translate(currentVector * flySpeed * Time.deltaTime);
        if (Math.Abs(transform.position.y - currentY) >= distance || bodyCollider.IsTouchingLayers(wallLayer))
        {
            Flip();
        }
    }

    void Flip()
    {
        if (currentVector == Vector2.up)
        {
            currentVector = Vector2.down;
        }
        else
        {
            currentVector = Vector2.up;
        }
        currentY = transform.position.y;
    }

}
