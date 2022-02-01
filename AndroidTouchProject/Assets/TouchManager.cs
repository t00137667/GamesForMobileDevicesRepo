using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour, ITouchController
{
    IControllable selectedObject = null;
    public void drag(List<Vector2> positions)
    {
        print("I have been dragged");
        if (selectedObject != null)
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
            selectedObject = controllable;
            controllable.selectToggle();
        }
        else
        {
            selectedObject = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
