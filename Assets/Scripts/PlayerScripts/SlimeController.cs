using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SlimeController : MonoBehaviour
{
    public float power = 10f;
    public float distance = 0;
    public float maxDrag = 5f;
    public float attackRange = 0.5f;
    public float attackRate = 1.5f;
    //float nextAttackTime = 0f;
    bool canAttack = true;
    bool startedDragging = false;

    public CircleCollider2D attackPoint;
    public Transform groundDetection;
    public Rigidbody2D rigidBody;
    public LineRenderer lineRenderer;
    public CinemachineVirtualCamera cinemachineVirtual;
    public LayerMask enemyLayer;


    public bool canJump = false;
    public Animator animator;

    Vector2 dragStartPos;
    Touch touch;

    /*private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }*/

    void Attack()
    {
        animator.SetTrigger("Attack");
        FindObjectOfType<AudioManager>().Play("SwordSlash");
        attackPoint.enabled = true;
        Invoke("disableAttackCollider" , 0.3f);
        /*Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(100);
        }*/
    }
    void disableAttackCollider()
    {
        attackPoint.enabled = false;
    }


    private void Update()
    {
        /*if (!cinemachineVirtual)
        {
            cinemachineVirtual = GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CinemachineVirtualCamera>();
        }*/

        if (Input.touchCount > 0 && canJump == true)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                DragStart();
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Dragging();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                DragRelease();
            }
        }

        if (Input.touchCount > 0 && canJump == false)
        {
            if (canAttack)//(Time.time >= nextAttackTime)
            {
                touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Moved)
                {
                    Attack();
                    //nextAttackTime = Time.time + 1f / attackRate;
                    canAttack = false;
                }
            }
        }

       
    }

    void DragStart()
    {
        startedDragging = true;
        dragStartPos = Camera.main.ScreenToWorldPoint(touch.position);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, dragStartPos);
    }
    void Dragging()
    {
        if (startedDragging)
        {
            Vector2 draggingPos = Camera.main.ScreenToWorldPoint(touch.position);
            Vector2 resultVector = Vector2.ClampMagnitude(new Vector2(draggingPos.x - dragStartPos.x, draggingPos.y - dragStartPos.y), maxDrag);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(1, new Vector2(resultVector.x + dragStartPos.x, resultVector.y + dragStartPos.y));

            Vector2 force = dragStartPos - draggingPos;
            Vector2 clampedForce = Vector2.ClampMagnitude(force, maxDrag) * power;
            float myForce = Convert.ToSingle(Math.Sqrt(Math.Pow(clampedForce.x, 2) + Math.Pow(clampedForce.y, 2)));

            animator.SetFloat("Force", myForce);
            /*cinemachineVirtual.m_Lens.OrthographicSize = 5 + myForce/8;*/
        }
       
    }
    void DragRelease()
    {
        if (startedDragging)
        {
            lineRenderer.positionCount = 0;
            Vector2 dragReleasePos = Camera.main.ScreenToWorldPoint(touch.position);

            if (canJump)
            {
                Vector2 force = dragStartPos - dragReleasePos;
                Vector2 clampedForce = Vector2.ClampMagnitude(force, maxDrag) * power;

                if (Math.Abs(clampedForce.y) > 0.5f || Math.Abs(clampedForce.x) > 0.5f)
                {

                    rigidBody.gravityScale = 2;
                    rigidBody.AddForce(clampedForce, ForceMode2D.Impulse);

                    lineRenderer.startColor = Color.red;
                    lineRenderer.endColor = Color.gray;

                    Vector3 characterScale = transform.localScale;
                    if (clampedForce.x > 0)
                    {
                        characterScale.x = -1;
                    }
                    else
                    {
                        characterScale.x = 1;
                    }
                    transform.localScale = characterScale;
                }
                else
                {
                    animator.SetFloat("Force", 0f);
                }
            }
            else
            {

            }
            startedDragging = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canJump = false;
        animator.SetBool("InAir", true);
        animator.SetFloat("Force", 0f);
        rigidBody.gravityScale = 2;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        FindObjectOfType<AudioManager>().Stop("SwordSlash");
        if (other.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        Quaternion hitRotation = Quaternion.FromToRotation(Vector2.up, other.contacts[0].normal);
        hitRotation = Quaternion.Euler(0,0, hitRotation.eulerAngles.z);
        transform.rotation = hitRotation;
        canJump = true;
        canAttack = true;
        attackPoint.enabled = false;
        animator.SetBool("InAir", false);

        if (other.gameObject.CompareTag("Wall"))
        {
            rigidBody.gravityScale = 0;
            FindObjectOfType<AudioManager>().Play("SlimeStick");
        }

        if (other.gameObject.CompareTag("SlipperyWall"))
        {
            rigidBody.gravityScale = 1f;
        }

        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.gray;
        
        //cinemachineVirtual.m_Lens.OrthographicSize = 5;
        

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        //canJump = true;
        animator.SetFloat("Force", 0f);
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.gray;
    }

}

    
