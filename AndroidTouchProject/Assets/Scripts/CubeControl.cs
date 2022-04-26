using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour, IControllable
{
    //Collider collider;
    private bool isSelected = false;
    bool rotationStarted = false;
    private Quaternion startingOrientation;
    private Quaternion workingOrientation;
    Renderer cubeRenderer;
    private Vector3 startingScale;
    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        //collider = gameObject.GetComponent<Collider>();
        cubeRenderer = GetComponent<Renderer>();
        startingScale = transform.localScale;
        startingOrientation = transform.rotation;
        workingOrientation = startingOrientation;
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ColourUpdate()
    {
        if (isSelected)
        {
            cubeRenderer.material.color = Color.red;
        }
        else
        {
            cubeRenderer.material.color = Color.white;
        }
    }

    public bool selectToggle(bool selected)
    {
        isSelected = selected;
        ColourUpdate();
        return isSelected;
    }

    public void drag(List<Vector2> positions)
    {
        if (isSelected)
        {
            Vector2 lastPosition = positions[positions.Count-1];
            Vector3 pos = new Vector3(lastPosition.x, lastPosition.y, transform.position.z - Camera.main.transform.position.z );
            transform.position = Camera.main.ScreenToWorldPoint(pos);
        }
    }

    public void tap(Vector2 position)
    {
        isSelected = !isSelected;
        ColourUpdate();
    }

    public void scale(float scaleFactor)
    {
        transform.localScale = startingScale * scaleFactor;
    }

    public void updateScale()
    {
        startingScale = transform.localScale;
        rotationStarted=false;
    }

    public void rotate(float rotation)
    {
        if (!rotationStarted)
        {
            rotationStarted = true;
            workingOrientation = transform.rotation;
        }

        else
        {
            transform.rotation = Quaternion.AngleAxis(rotation, Camera.main.transform.forward) * startingOrientation  ;
        }
      //  transform.Rotate(0, 0, rotation);
    }

    public void resetPosition()
    {
        transform.position = startingPosition;
        transform.rotation = startingOrientation;
    }
}
