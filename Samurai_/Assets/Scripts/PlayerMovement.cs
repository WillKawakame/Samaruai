using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 force;
    bool zoomOut;
    Rigidbody2D rb;
    float distance;

    Trajectory trajectory;

    public float jumpForce;

    public float zoomValue;
    private void Awake() {

        rb = GetComponent<Rigidbody2D>();
        trajectory = GetComponent<Trajectory>();

    }
    private void Update()
    {
        #if UNITY_ANDROID

        GetTouchInput();

        #endif


        #if UNITY_EDITOR

        //GetMouseInput();

        #endif

        if(zoomOut)
            CameraZoomOut();
    }

    

    private void GetTouchInput() {

        if(Input.touchCount > 0) {

            Touch touch = Input.GetTouch(0);

            switch (touch.phase) {

                case TouchPhase.Began:

                    TouchStarted(touch.position);

                    break;

                case TouchPhase.Moved:

                    TouchMoved(touch.position);

                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:

// Make sure player can't fly (there will be a mechanic with this in the future)
                    if(force.x > 0.5f || 
                    force.y > 0.5f ||  
                    force.x > 0.5f || 
                    force.y > 0.5f)
                        TouchEnded();
                    break;
            }
        }
    }

    private void TouchStarted(Vector2 pointerPos) {

        startPoint = Camera.main.ScreenToWorldPoint(pointerPos);

        // SlowMotion On
        // Make more slow purchasable in store
        Time.timeScale = 0.2f;

    }

    private void TouchMoved(Vector2 pointerPos) {

        endPoint = Camera.main.ScreenToWorldPoint(pointerPos);

        // Take the distance, the direction and the jumpForce off the jump
        distance = Vector2.Distance(startPoint, endPoint);
        DistanceLimit();
        Vector2 direction = (startPoint - endPoint).normalized;
        force = jumpForce * distance * direction;

        CameraZoomIn();

        // LineRenderer Management(StartLine)
        trajectory.linePosition(direction * distance);

    }

    private void TouchEnded() {

        // SlowMotion Off
        Time.timeScale = 1f;

        // LineRenderer Management (ResetLine)
        trajectory.resetLinePosition();

        // Add force to player and make the velocity zero before it
        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);

        zoomOut = true;
    }

    // Limit the Distance(Line Radious)
    private void DistanceLimit(){
        if(distance >= 2.5f)
            distance = 2.5f;
    }

    // Zoom
    private void CameraZoomIn(){

        //Camera.main.orthographicSize -= Time.deltaTime * zoomValue;
        Camera.main.orthographicSize =  Mathf.SmoothStep(Camera.main.orthographicSize, 4.5f, Time.deltaTime * zoomValue); 
    }
    //  Make the code more clean
    private void CameraZoomOut(){
        Camera.main.orthographicSize =  Mathf.Lerp(Camera.main.orthographicSize, 5f, Time.deltaTime * zoomValue * 2);
        if(Camera.main.orthographicSize >= 4.9f)
            zoomOut = false;
    }
}