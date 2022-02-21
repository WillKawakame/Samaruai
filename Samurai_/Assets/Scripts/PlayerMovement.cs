using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 force;
    Rigidbody2D rb;

    Trajectory trajectory;

    public float jumpForce;


    private void Awake() {

        rb = GetComponent<Rigidbody2D>();
        trajectory = GetComponent<Trajectory>();

    }
    private void Update()
    {
        GetInput();
    }

    private void GetInput() {

        if(Input.touchCount > 0) {

            Touch touch = Input.GetTouch(0);

            switch (touch.phase) {

                case TouchPhase.Began:
                    TouchStarted(touch);
                    break;

                case TouchPhase.Moved:
                    TouchMoved(touch);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if(force.x > 0.5f || force.y > 0.5f)
                        TouchEnded();
                    break;
            }
        }
    }

    private void TouchStarted(Touch t) {

        startPoint = Camera.main.ScreenToWorldPoint(t.position);

    }

    private void TouchMoved(Touch t) {

        endPoint = Camera.main.ScreenToWorldPoint(t.position);
        float distance = Vector2.Distance(startPoint, endPoint);
        Vector2 direction = (startPoint - endPoint).normalized;
        force = jumpForce * distance * direction;

    }

    private void TouchEnded() {

        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);

    }
}
