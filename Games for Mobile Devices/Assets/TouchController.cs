using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    float touchDuration;
    float stationaryTolerance = 5.0f;
    float movementTolerance = 1.0f;
    int screenWidth = Screen.width;
    int screenheight = Screen.height;
    IControllable currentlySelected;
    IControllable[] controllables;
    Vector3 cameraStart;
    Vector3 cameraStartPosition;
    Quaternion cameraStartRotation;
    float? scaleDistance = null;
    float reduceCraziness = 0.01f;
    Vector3 touchOne;
    Vector3 touchTwo;
    // Start is called before the first frame update
    void Start()
    {
        controllables = FindObjectsOfType<MonoBehaviour>().OfType<IControllable>().ToArray();
        //Debug.Log(controllables.Length);
        currentlySelected = null;
        cameraStart = Camera.main.transform.position;
        cameraStartPosition = Camera.main.transform.position;
        cameraStartRotation = Camera.main.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                    Touch touch = Input.GetTouch(0);
                    touchDuration += Time.deltaTime;
                IControllable controllable;
                // I elected not to use Touch.tapCount
                if (touch.phase == TouchPhase.Ended && touchDuration < 0.2f && Input.touchCount == 1)
                {
                    if(Physics.Raycast(ray, out hit))
                    {
                        controllable = hit.collider.GetComponent<IControllable>();
                        if (controllable.IsTappable())
                        {
                            // If there is a current selection, and it is tapped again, deselect it
                            if (currentlySelected != null && currentlySelected == controllable)

                            {
                                currentlySelected.Deselect();
                                currentlySelected = null;
                            }
                            else
                            {
                                if (currentlySelected != null)
                                {
                                    currentlySelected.Deselect();
                                    currentlySelected = null;
                                }
                                currentlySelected = controllable;
                                currentlySelected.Tap();
                            }

                        }
                        else
                        {
                            Debug.Log("No Tap Interface");
                        }
                    }
                    
                }
                if (touch.phase == TouchPhase.Stationary && touchDuration > 0.2f)
                {
                    if (currentlySelected != null)
                    {
                        currentlySelected.Hold(touch.position);
                    }
                }
                if (touch.phase == TouchPhase.Moved && touchDuration > 0.2f)
                {
                    if (currentlySelected != null)
                    {
                        currentlySelected.Drag(touch.position);
                    }
                    else
                    {
                        Camera.main.transform.position -= new Vector3(touch.deltaPosition.x * 0.001f, touch.deltaPosition.y * 0.001f, 0);
                    }
                }
            }
            if (Input.touchCount == 2)
            {
                touchDuration = 0;
                //Debug.Log("Double Touch");
                if (Input.touches[0].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Began)
                {
                    scaleDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                    cameraStart = Camera.main.transform.position;
                    touchOne = Input.touches[0].position;
                    touchTwo = Input.touches[1].position;
                }
                if ( ((Input.touches[0].phase == TouchPhase.Stationary || (Input.touches[0].phase == TouchPhase.Moved && Input.touches[0].deltaPosition.magnitude < stationaryTolerance))  && Input.touches[1].phase == TouchPhase.Moved && Input.touches[1].deltaPosition.magnitude > movementTolerance) ||
                    ((Input.touches[1].phase == TouchPhase.Stationary || (Input.touches[1].phase == TouchPhase.Moved && Input.touches[1].deltaPosition.magnitude < stationaryTolerance)) && Input.touches[0].phase == TouchPhase.Moved && Input.touches[0].deltaPosition.magnitude > movementTolerance) )
                {
                    Debug.Log("One Finger is stationary(ish)");
                    Vector3 firstAngle = touchOne - touchTwo;
                    Vector3 secondAngle = Input.touches[0].position - Input.touches[1].position;
                    float angle = Vector3.SignedAngle(firstAngle, secondAngle, Camera.main.transform.forward) * 2;

                    if (currentlySelected == null)
                    {
                        Camera.main.transform.Rotate(new Vector3(0, 0, angle) * Time.deltaTime * 2f);
                    }
                    else if (currentlySelected.IsRotatable())
                    {
                        currentlySelected.Rotate(angle);
                    }
                    float adjustedScale = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                    float scaleRatio = (float)(adjustedScale - scaleDistance);
                    //Debug.Log("Scale Ratio: " + scaleRatio);
                    if (Mathf.Abs(scaleRatio) > 0.5f)
                    {
                        if (currentlySelected == null)
                        {
                            Camera.main.transform.position = cameraStart + Camera.main.transform.forward * scaleRatio * reduceCraziness;
                        }
                        else if (currentlySelected.IsScaleable())
                        {
                            currentlySelected.Scale(scaleRatio);
                        }
                    }
                }
                else if (Input.touches[0].phase == TouchPhase.Moved && Input.touches[0].deltaPosition.magnitude > movementTolerance ||
                    Input.touches[1].phase == TouchPhase.Moved && Input.touches[1].deltaPosition.magnitude > movementTolerance)
                {
                    //Camera.main.transform.Rotate(new Vector3(Input.touches[0].deltaPosition.y, Input.touches[0].deltaPosition.x, 0) * 0.01f);
                    if (currentlySelected == null)
                    {
                        Camera.main.transform.Rotate(new Vector3(Input.touches[0].deltaPosition.y, Input.touches[0].deltaPosition.x, 0) * 0.01f);
                    }
                }
            }

        }
        else touchDuration = 0f;
    }
    public void ResetAll()
    {
        Debug.Log("Resetting");
        Camera.main.transform.position = cameraStartPosition;
        Camera.main.transform.rotation = cameraStartRotation;

        if (controllables.Length > 0)
        {
            foreach (IControllable c in controllables){
                c.ResetPosition();
            }
        }
    }
}
