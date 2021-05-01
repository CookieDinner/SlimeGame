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
    public float maxDrag = 5f;

    public Rigidbody2D rigidBody;
    public LineRenderer lineRenderer;
    public CinemachineVirtualCamera cinemachineVirtual;

    public bool canJump = false;
    public Animator animator;

    Vector2 dragStartPos;
    Touch touch;


    private void Update()
    {
        if (!cinemachineVirtual)
        {
            cinemachineVirtual = GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CinemachineVirtualCamera>();
        }

        if (Input.touchCount > 0 && canJump == false)
        {
            touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Moved)
            {
                animator.SetTrigger("Attack");
            }
        }

        if (Input.touchCount > 0 && canJump==true)
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
        
        Vector2 force = dragStartPos - draggingPos;
        Vector2 clampedForce = Vector2.ClampMagnitude(force, maxDrag) * power;
        float myForce = Convert.ToSingle(Math.Sqrt(Math.Pow(clampedForce.x, 2) + Math.Pow(clampedForce.y, 2)));

        animator.SetFloat("Force", myForce);
        cinemachineVirtual.m_Lens.OrthographicSize = 5 + myForce/8;
       
    }
    void DragRelease()
    {
        lineRenderer.positionCount = 0;
        Vector2 dragReleasePos = Camera.main.ScreenToWorldPoint(touch.position);

        if (canJump)
        {
            Vector2 force = dragStartPos - dragReleasePos;
            Vector2 clampedForce = Vector2.ClampMagnitude(force, maxDrag) * power;
            
            rigidBody.gravityScale = 2;
            rigidBody.AddForce(clampedForce, ForceMode2D.Impulse);
            canJump = false;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.gray;
            animator.SetBool("InAir", true);
            animator.SetFloat("Force", 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Quaternion hitRotation = Quaternion.FromToRotation(Vector2.up, other.contacts[0].normal);
        hitRotation = Quaternion.Euler(0,0, hitRotation.eulerAngles.z);
        transform.rotation = hitRotation;
        canJump = true;
        rigidBody.velocity = Vector2.zero;
        rigidBody.gravityScale = 0;
        rigidBody.angularVelocity = 0f;
        
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.gray;
        animator.SetBool("InAir", false);
        cinemachineVirtual.m_Lens.OrthographicSize = 5;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        //canJump = true;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.gray;
    }

}

    
