using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    public Animator animator;
    public AudioSource source;
    public Rigidbody2D rb;
    public BoxCollider2D bodyCollider;
    public LayerMask wallLayer;
    public Transform groundDetection;
    public GameObject deathParticles;

    public int maxHealth = 100;
    int currentHealth;

    public bool mustPatrol;
    public bool groundPatrol;
    public float distance;
    public float walkSpeed;

   
    void Start()
    {
        currentHealth = maxHealth;
        mustPatrol = true;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if(currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        animator.SetBool("Dead", true);

        FindObjectOfType<AudioManager>().Play("SkeletonDeath");
        source.mute = true;

        GameObject particle = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(particle, 5);


        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;     
        
    }

    // Update is called once per frame
    void Update()
    {
        if (groundPatrol == true)
        {
            GroundPatrol();
        }
        else
        {
            PlatformPatrol();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SpinAttack")){
            TakeDamage(100);
        }
    }


    void PlatformPatrol()
    {
        transform.Translate(Vector2.right * walkSpeed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);

        if (groundInfo.collider == false)
        {
            Flip();
        }
    }

    void GroundPatrol()
    {
        transform.Translate(Vector2.right * walkSpeed * Time.deltaTime);

        if (bodyCollider.IsTouchingLayers(wallLayer))
        {
            Flip();
        }
    }

    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        mustPatrol = true;
    }
}
