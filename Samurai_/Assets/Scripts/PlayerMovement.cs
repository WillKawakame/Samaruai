using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 force;
    bool zoomOut = false;
    Rigidbody2D rb;
    float distance;

    Trajectory trajectory;

    public float jumpForce;

    public float zoomIntensity;

    private Transform Hologram;

    [HideInInspector]
    public static int jumpLimit = 3;
    private int nJump = 0;
    private bool canJump = true;
    private bool inHolo = false;
    private void Awake() {

        rb = GetComponent<Rigidbody2D>();
        trajectory = GetComponent<Trajectory>();

    }
    private void Update()
    {
        CanJumpManage();

        #if UNITY_ANDROID

        if(canJump){
            GetTouchInput();
        }

        #endif


        #if UNITY_EDITOR
        if(canJump){
            GetMouseInput();
        }

        #endif

        if(inHolo){
            
            if(Input.GetMouseButtonUp(0)){
                OutHolo();
                inHolo = false;
            }
        }

        if(zoomOut)
            CameraZoomOut();
    }


    private void CanJumpManage(){
        if(nJump < jumpLimit && !inHolo){
            canJump = true;
        }else{
            canJump = false;
        }
    }

    private void GetMouseInput(){

        if(Input.GetMouseButtonDown(0)){
            TouchStarted(Input.mousePosition);
        }

        if(Input.GetMouseButton(0)){
            TouchMoved(Input.mousePosition);
        }

        if(Input.GetMouseButtonUp(0)){
            if(force.x > 0.5f || 
                    force.y > 0.5f ||  
                    force.x > 0.5f || 
                    force.y > 0.5f)
                TouchEnded();
            
            nJump += 1;
        }

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

                    nJump += 1;

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
        Vector2 direction = (startPoint - endPoint).normalized;
        force = jumpForce * distance * direction;


        // Limit the Distance(Line Radious)
        if(distance >= 2.5f)
            distance = 2.5f;
            

        // Zoom
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

    // Zoom
    private void CameraZoomIn(){
        //Camera.main.orthographicSize -= Time.deltaTime * zoomValue;
        Camera.main.orthographicSize =  Mathf.SmoothStep(Camera.main.orthographicSize, 4.5f, Time.deltaTime * zoomIntensity); 
    }
    //  Make the code more clean
    private void CameraZoomOut(){
        Camera.main.orthographicSize =  Mathf.Lerp(Camera.main.orthographicSize, 5f, Time.deltaTime * zoomIntensity * 2);
        if(Camera.main.orthographicSize >= 4.9f)
            zoomOut = false;
    }

    private void InHolo(){
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        // LineRenderer Management (ResetLine)
        trajectory.resetLinePosition();
    }

    private void OutHolo(){
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        transform.position = Hologram.position;
        rb.isKinematic = false;
        rb.AddForce(Vector2.up * jumpForce * 1.5f, ForceMode2D.Impulse);
    }
    

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Holo")){
            inHolo = true;
            nJump = 0;
            Hologram = other.gameObject.transform.GetChild(0).GetComponent<Transform>();
            InHolo();
            // Make Player static and unvisible
        }
    }

}