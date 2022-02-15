using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDeterminer : MonoBehaviour
{

    private float tapTimer = 0f;
    private float MAX_ALLOWED_TAP_TIME = 0.2f;
    private bool hasMoved;
    private float startingDistance;

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
                                manager.drag(touchPositions);
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
                    switch (secondTouch.phase)
                    {
                        case TouchPhase.Began:
                            startingDistance = Vector2.Distance(secondTouch.position, firstTouch.position);
                            if (firstTouch.phase == TouchPhase.Stationary)
                            {
                                //float scaleRatio = (float)(adjustedScale - scaleDistance);
                                
                                print("Both touches began");
                                //print(startingDistance);
                            }
                            break;

                        case TouchPhase.Moved:

                            if (firstTouch.phase == TouchPhase.Moved)
                            {
                                float currentDistance = Vector2.Distance(secondTouch.position, firstTouch.position);
                                float relative_distance = currentDistance / startingDistance;
                                //print(relative_distance);
                                manager.pinch(firstTouch.position, secondTouch.position, relative_distance);
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
