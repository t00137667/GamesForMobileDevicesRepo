using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchManager : MonoBehaviour, ITouchController
{
    IControllable selectedObject = null;
    IControllable controllableFound;
    IControllable[] controllables;
    public void drag(List<Vector2> positions)
    {
        print("I have been dragged");
        if (selectedObject != null && controllableFound == selectedObject)
        {
            selectedObject.drag(positions);
        }
    }

    public void pinch(Vector2 position1, Vector2 position2, float relative_distance)
    {
        throw new System.NotImplementedException();
    }

    public void tap(Vector2 position)
    {
        print("tapped at " + position);
        Ray tapRay = Camera.main.ScreenPointToRay(position);
        Debug.DrawRay(tapRay.origin, tapRay.direction * 50, Color.red, 4f);
        RaycastHit hitInfo;
        if (Physics.Raycast(tapRay, out hitInfo))
        {
            print("i hit something");
            IControllable controllable = hitInfo.transform.GetComponent<IControllable>();
            if (selectedObject != null && controllable != selectedObject)
            {
                // Tap manager behaviour: Only ONE Selected object at a time.
                // Ensure all other objects are deselected.
                for (int i = 0; i < controllables.Length; i++)
                {
                    IControllable c = controllables[i];
                    c.selectToggle(false);
                    print("Set to False");
                }
                selectedObject = null;
            }
            selectedObject = controllable;
            selectedObject.tap(position);
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
                print("Controllable not Found");
            }
            else
            {
                controllableFound = controllable;
                print("Controllable Found");
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

    // Start is called before the first frame update
    void Start()
    {
        controllableFound = null;
        controllables = FindObjectsOfType<MonoBehaviour>().OfType<IControllable>().ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
