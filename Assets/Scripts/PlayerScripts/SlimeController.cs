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
    public float maxDrag = 5f;
    public Rigidbody2D rigidBody;
    public LineRenderer lineRenderer;
    private bool canJump;
    private bool onGround;
    private Vector2 dragStartPos;
    private Touch touch;
    private Animator animator;
    private float timeSinceJumped;

    private void Start()
    {
        animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        if (!canJump && onGround && timeSinceJumped > 0.3f)
        {
            timeSinceJumped = 0.0f;
            Debug.Log("FIXED");
            canJump = true;
            animator.enabled = true;
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.gray;
        }

        if (!canJump)
        {
            timeSinceJumped += Time.deltaTime;
        }
        if (Input.touchCount > 0)
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
    }

    void DragStart()
    {
        dragStartPos = Camera.main.ScreenToWorldPoint(touch.position);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, dragStartPos);
    }
    void Dragging()
    {
        Vector2 draggingPos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector2 resultVector = Vector2.ClampMagnitude(
            new Vector2(draggingPos.x - dragStartPos.x, draggingPos.y - dragStartPos.y), maxDrag);
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(1, new Vector2(
            resultVector.x + dragStartPos.x, resultVector.y + dragStartPos.y));
        animator.enabled = false;
    }
    void DragRelease()
    {
        lineRenderer.positionCount = 0;
        Vector2 dragReleasePos = Camera.main.ScreenToWorldPoint(touch.position);

        if (canJump)
        {
            Debug.Log("jumped");
            Vector2 force = dragStartPos - dragReleasePos;
            Vector2 clampedForce = Vector2.ClampMagnitude(force, maxDrag) * power;
            
            rigidBody.gravityScale = 2;
            rigidBody.AddForce(clampedForce, ForceMode2D.Impulse);
            animator.enabled = false;
            canJump = false;
            timeSinceJumped = 0.0f;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.gray;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("entered");
        Quaternion hitRotation = Quaternion.FromToRotation(Vector2.up, other.contacts[0].normal);
        hitRotation = Quaternion.Euler(0,0, hitRotation.eulerAngles.z);
        transform.rotation = hitRotation;
        animator.enabled = true;
        canJump = true;
        onGround = true;
        rigidBody.velocity = Vector2.zero;
        rigidBody.gravityScale = 0;
        rigidBody.angularVelocity = 0f;
        
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.gray;
    }
    

    private void OnCollisionExit2D()
    {
        Debug.Log("exit");
        onGround = false;
    }
}

    
