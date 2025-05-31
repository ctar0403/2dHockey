using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public float maxVelocity = 1f;
    public float playerForce = 5f;
    Vector2 pos;

    private Vector3 targetPosition;
    private bool isDragging = false;

    void LateUpdate()
    {
        HandleTouchInput();
        HandleKeyboardInput();
        HandleMouseInput();

        // Move smoothly toward the target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxVelocity);

        ClampPosition();
    }

    void HandleTouchInput()
    {
        foreach (Touch touch in Input.touches)
        {
            pos = Camera.main.ScreenToWorldPoint(new Vector2(touch.position.x - 1.5f, touch.position.y - 0.5f));
            if (pos.x > -3)
            {
                targetPosition = pos; // Move target position to touch location
            }
        }
    }

    void HandleKeyboardInput()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow)) direction += Vector3.up;
        if (Input.GetKey(KeyCode.DownArrow)) direction += Vector3.down;
        if (Input.GetKey(KeyCode.LeftArrow)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow)) direction += Vector3.right;

        if (direction != Vector3.zero)
        {
            targetPosition = transform.position + direction;
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f; // Distance from camera
            targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    void ClampPosition()
    {
        Vector3 clamped = transform.position;
        clamped.y = Mathf.Clamp(clamped.y, -3.8f, 3.8f);
        clamped.x = Mathf.Clamp(clamped.x, 0.05f, 7.15f);
        transform.position = clamped;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Vector2 awayPlayer = collision.transform.position - transform.position;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = awayPlayer * playerForce;
        }
    }
}
