using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class SlimeController : MonoBehaviour
{
    public float power = 10f;
    public float maxDrag = 5f;
    public Rigidbody2D rb;
    public LineRenderer lr;

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
        lr.positionCount = 1;
        lr.SetPosition(0, dragStartPos);
    }
    void Dragging()
    {
        Vector2 draggingPos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector2 resultVector = Vector2.ClampMagnitude(
            new Vector2(draggingPos.x - dragStartPos.x, draggingPos.y - dragStartPos.y), maxDrag);
        lr.positionCount = 2;
        lr.SetPosition(1, new Vector2(
            resultVector.x + dragStartPos.x, resultVector.y + dragStartPos.y));
    }
    void DragRelease()
    {
        lr.positionCount = 0;
        Vector2 dragReleasePos = Camera.main.ScreenToWorldPoint(touch.position);

        Vector2 force = dragStartPos - dragReleasePos;
        Vector2 clampedForce = Vector2.ClampMagnitude(force, maxDrag) * power;

        rb.AddForce(clampedForce, ForceMode2D.Impulse);
    }
}
