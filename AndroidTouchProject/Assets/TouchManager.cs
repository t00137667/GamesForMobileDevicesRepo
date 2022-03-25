using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour, ITouchController
{
    IControllable selectedObject = null;
    IControllable controllableFound;
    IControllable[] controllables;
    private Quaternion startingOrientation;
    private Quaternion workingOrientation;
    private Vector3 startingCameraPosition;
    private Vector3 workingCameraPosition;
    private Vector3 targetPoint;
    private bool rotationStarted = false;
    private float panSpeed = 0.1f;
    private float zoomSpeed = 0.05f;
    GameObject myGO;
    Canvas myCanvas;
    GameObject newButton;

    private float scaleFactor = 1f;

    public void drag(List<Vector2> positions, Touch lastTouch)
    {
        //print("I have been dragged");
        if (selectedObject != null && controllableFound == selectedObject)
        {
            selectedObject.drag(positions);
        }
        else
        {
            Camera.main.transform.Rotate(new Vector3(lastTouch.deltaPosition.y, lastTouch.deltaPosition.x, 0) * 0.015f);
        }
    }

    public void pinch(Vector2 position1, Vector2 position2, float relative_distance)
    {
        if (selectedObject != null)
        {
            selectedObject.scale(relative_distance * scaleFactor);
        }
        else
        {
            Vector3 forward = Camera.main.transform.forward;
            if (relative_distance < 1)
            {
                forward = -forward;
            }
            Camera.main.transform.Translate(forward * zoomSpeed, Space.Self);
        }
    }

    public void tap(Vector2 position)
    {
        //print("tapped at " + position);
        Ray tapRay = Camera.main.ScreenPointToRay(position);
        Debug.DrawRay(tapRay.origin, tapRay.direction * 50, Color.red, 4f);
        RaycastHit hitInfo;
        if (Physics.Raycast(tapRay, out hitInfo))
        {
            if (hitInfo.transform.GetComponent<Button>() != null)
            {
                print("Found a button");
            }
            // Set of If-else to manage the tap process based on if an object has been selected already or not
            IControllable controllable = hitInfo.transform.GetComponent<IControllable>();
            if (selectedObject != null && controllable != selectedObject)
            {
                // Tap manager behaviour: Only ONE Selected object at a time.
                // Ensure all other objects are deselected.
                for (int i = 0; i < controllables.Length; i++)
                {
                    IControllable c = controllables[i];
                    c.selectToggle(false);
                    //print("Set to False");
                }
                selectedObject = null;
                selectedObject = controllable;
                selectedObject.tap(position);
            }
            else if (selectedObject != null && controllable == selectedObject)
            {
                selectedObject.tap(position);
                selectedObject = null;
                controllable = null;
            }
            else
            {
                selectedObject = controllable;
                if (selectedObject != null)
                {
                    selectedObject.tap(position);
                }
            } 
        }
        else
        {
            selectedObject = null;
        }
    }

    public void ControllableFound(Vector2 touchPosition)
    {
        Ray tapRay = Camera.main.ScreenPointToRay(touchPosition);
        Debug.DrawRay(tapRay.origin, tapRay.direction * 50, Color.blue, 2f);
        RaycastHit hitInfo;
        if (Physics.Raycast(tapRay, out hitInfo))
        {
            IControllable controllable = hitInfo.transform.GetComponent<IControllable>();
            if (controllable == null)
            {
                ClearControllable();
                //print("Controllable not Found");
            }
            else
            {
                controllableFound = controllable;
                //print("Controllable Found");
            }
        }
        else
        {
            ClearControllable();
        }
    }

    public void ClearControllable()
    {
        controllableFound = null;
    }

    public void UpdateScale()
    {
        if (selectedObject != null)
        {
            selectedObject.updateScale();
        }
        rotationStarted = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        controllableFound = null;
        controllables = FindObjectsOfType<MonoBehaviour>().OfType<IControllable>().ToArray();
        startingOrientation = Camera.main.transform.rotation;
        workingOrientation = startingOrientation;
        startingCameraPosition = Camera.main.transform.position;
        workingCameraPosition = startingCameraPosition;

        // Canvas
        myGO = new GameObject();
        myGO.name = "Canvas";
        myGO.AddComponent<Canvas>();

        myCanvas = myGO.GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //myCanvas.gameObject.layer = LayerMask.NameToLayer("UI");
        myGO.AddComponent<CanvasScaler>();
        myGO.AddComponent<GraphicRaycaster>();

        newButton = DefaultControls.CreateButton(new DefaultControls.Resources());
        newButton.transform.SetParent(myCanvas.transform, false);
        newButton.GetComponentInChildren<Text>().text = "Reset Button";
        newButton.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(0,-200);
        //UnityEventTools.AddPersistentListener(newButton.GetComponent<Button>().onClick, new UnityAction(ResetPositions));
        newButton.GetComponent<Button>().onClick.AddListener(ResetPositions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPositions()
    {
        Debug.Log("Resetting");
        Camera.main.transform.position = startingCameraPosition;
        Camera.main.transform.rotation = startingOrientation;

        if (controllables.Length > 0)
        {
            foreach (IControllable c in controllables)
            {
                c.resetPosition();
            }
        }
    }

    public void rotate(float currentRotation)
    {
        if (selectedObject != null)
        {
            selectedObject.rotate(currentRotation);
        }
        else
        {
            if (!rotationStarted)
            {
                rotationStarted = true;
                workingOrientation = Camera.main.transform.rotation;
                //print("Rotation Started");
            }
            else
            {
                Camera.main.transform.rotation = Quaternion.AngleAxis(-currentRotation, Camera.main.transform.forward) * workingOrientation;
                //print("Rotation Applied");
            }
            
        }
    }

    internal void drag2(Vector3 direction)
    {
        if (selectedObject != null)
        {
            print("Oh hai");
        }
        else
        {
            Camera.main.transform.Translate(new Vector3(-direction.x, -direction.y, 0) * panSpeed, Space.World);
        }
    }
}
