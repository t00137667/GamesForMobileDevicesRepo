using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDeterminer : MonoBehaviour
{

    private float tapTimer = 0f;
    private float MAX_ALLOWED_TAP_TIME = 0.2f;
    private bool hasMoved;
    private float startingDistance;
    private float startingRotation;
    private Vector3 startingVector;
    private Vector2 startingMidpoint;

    TouchManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<TouchManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(Input.touchCount);
        //Update times
        tapTimer += Time.deltaTime;
        if (Input.touchCount > 0)
        {
            Touch[] all_touches = Input.touches;
            Touch firstTouch = all_touches[0];
            List<Vector2> touchPositions = new List<Vector2>();
            //print(firstTouch.phase);

            switch (all_touches.Length)
            {
                case 1:
                    manager.UpdateScale();
                    touchPositions.Add(firstTouch.position);
                    switch (firstTouch.phase)
                    {
                        case TouchPhase.Began:
                            tapTimer = 0f;
                            hasMoved = false;
                            manager.ControllableFound(firstTouch.position);
                            break;

                        case TouchPhase.Moved:
                            hasMoved = true;
                            if (tapTimer > MAX_ALLOWED_TAP_TIME && hasMoved)
                            {
                                manager.drag(touchPositions, firstTouch);
                            }
                            break;

                        case TouchPhase.Stationary:
                            break;

                        case TouchPhase.Ended:
                            if ((tapTimer < MAX_ALLOWED_TAP_TIME) && !hasMoved)
                            {
                                manager.tap(firstTouch.position);
                            }
                            touchPositions.Clear();
                            manager.ClearControllable();
                            break;
                    }
                    break;
                case 2:
                    Touch secondTouch = all_touches[1];
                    Vector2 midPoint = firstTouch.position + (secondTouch.position - firstTouch.position) / 2;
                    Vector2 midPointUp = midPoint + (midPoint * Vector3.up);
                    Debug.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(midPoint.x, midPoint.y, 5)), Camera.main.ScreenToWorldPoint(new Vector3(midPointUp.x, midPointUp.y,5)), Color.cyan, 0.0f, false) ;
                    Debug.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(firstTouch.position.x, firstTouch.position.y, 5)), Camera.main.ScreenToWorldPoint(new Vector3(secondTouch.position.x, secondTouch.position.y, 5)), Color.blue, 0.0f, false);
                    switch (secondTouch.phase)
                    {
                        case TouchPhase.Began:
                            startingDistance = Vector2.Distance(secondTouch.position, firstTouch.position);
                            startingRotation = Mathf.Atan2(secondTouch.position.y - firstTouch.position.y, secondTouch.position.x - firstTouch.position.x);
                            startingVector = Camera.main.ScreenToWorldPoint(firstTouch.position) - Camera.main.ScreenToWorldPoint(secondTouch.position);
                            startingMidpoint = midPoint;
                            if (firstTouch.phase == TouchPhase.Stationary)
                            {
                                //float scaleRatio = (float)(adjustedScale - scaleDistance);
                                //print("Both touches began");
                                //print(startingDistance);
                            }
                            break;

                        case TouchPhase.Moved:

                            if (firstTouch.phase == TouchPhase.Moved)
                            {
                                float currentDistance = Vector2.Distance(secondTouch.position, firstTouch.position);
                                float relative_distance = currentDistance / startingDistance;
                                print("RedDist: " + relative_distance);
                                

                                float currentRotation = Mathf.Atan2(secondTouch.position.y - firstTouch.position.y, secondTouch.position.x - firstTouch.position.x);
                                float relativeRotation = currentRotation - startingRotation;
                                print("RelRotate: " + relativeRotation);
                                

                                if (relativeRotation > -0.1f && relativeRotation < 0.1f && relative_distance < 1.1f && relative_distance > 0.9f)
                                {
                                    Vector2 relativeMidpoint = Camera.main.ScreenToWorldPoint(new Vector3(midPoint.x, midPoint.y, 1)) - Camera.main.ScreenToWorldPoint(new Vector3(startingMidpoint.x, startingMidpoint.y, 1));
                                    manager.drag2(relativeMidpoint);
                                }
                                else
                                {
                                    
                                    if (relativeRotation > 0.03f || relativeRotation < 0.03f)
                                    {
                                        manager.rotate(relativeRotation * Mathf.Rad2Deg);
                                    }
                                    if (relative_distance > 1.2 || relative_distance < 0.8)
                                    {
                                        manager.pinch(firstTouch.position, secondTouch.position, relative_distance);
                                    }
                                }
                            }
                            break;
                            
                        case TouchPhase.Stationary:
                            if (firstTouch.phase == TouchPhase.Stationary)
                            {

                            }
                            break;

                        case TouchPhase.Ended:
                            manager.UpdateScale();
                            break;
                    }
                    break;
            }
        }
    }
}
