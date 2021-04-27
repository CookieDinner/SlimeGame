using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SlimeController : MonoBehaviour
{
    public float power = 10f;
    public float maxDrag = 5f;
    public Rigidbody2D rigidBody;
    public LineRenderer lineRenderer;
    public bool onGround = false;
    public int rotation;
    public GameObject[] objectsArray;

    private Vector3 targetRotation;


    Vector2 dragStartPos;
    Touch touch;

    private void Update()
    {
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
    }
    void DragRelease()
    {
        lineRenderer.positionCount = 0;
        Vector2 dragReleasePos = Camera.main.ScreenToWorldPoint(touch.position);

        if (onGround)
        {
            Vector2 force = dragStartPos - dragReleasePos;
            Vector2 clampedForce = Vector2.ClampMagnitude(force, maxDrag) * power;

            rigidBody.AddForce(clampedForce, ForceMode2D.Impulse);
            onGround = false;
            rigidBody.gravityScale = 2;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.gray;
        }
    }
    private void rotateObject()
    {
        targetRotation.z = rotation;
        gameObject.transform.eulerAngles = targetRotation;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        onGround = true;
        rigidBody.gravityScale = 0;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.gray;
        var relativePosition = transform.InverseTransformPoint(collision.transform.position);

        if (relativePosition.x > 0.05f)
        {
            rotation = 90;
        }
        {
        else if (relativePosition.x < -0.05f)
            rotation = 270;
        }

        if (relativePosition.y > 0.05f)
        {
            rotation = 180;
        }
        {
        else if (relativePosition.y < -0.05f)
            rotation = 0;
        }

        rotateObject();
    }
}

    
