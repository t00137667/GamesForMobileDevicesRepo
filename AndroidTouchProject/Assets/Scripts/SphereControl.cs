using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereControl : MonoBehaviour, IControllable
{
    Renderer myRenderer;
    Collider planeCollider, sphereCollider;
    private bool isSelected = false;

    private Vector3 startingScale;
    private Vector3 startingPosition;
    private Quaternion startingRotation;

    public void drag(List<Vector2> positions)
    {
        Vector2 position = positions[positions.Count - 1];
        Ray tapRay = Camera.main.ScreenPointToRay(position);
        Debug.DrawRay(tapRay.origin, tapRay.direction * 50, Color.red, 4f);
        RaycastHit hitInfo;
        ColliderToggle();
        if (Physics.Raycast(tapRay, out hitInfo))
        {
            print("i hit something");
            transform.position = hitInfo.point;
        }
        else
        {
            print("I hit nothing");
        }
        ColliderToggle();
    }

    public bool selectToggle(bool selected)
    {
        isSelected = selected;
        ColourUpdate();
        return isSelected;
    }

    public void tap(Vector2 position)
    {
        isSelected = !isSelected;
        ColourUpdate();
    }

    private void ColourUpdate()
    {
        if (isSelected)
        {
            myRenderer.material.color = Color.red;
        }
        else
        {
            myRenderer.material.color = Color.white;
        }
    }
    private void ColliderToggle()
    {
        if (sphereCollider.enabled)
        {
            sphereCollider.enabled = false;
            planeCollider.enabled = true;
        }
        else
        {
            sphereCollider.enabled = true;
            planeCollider.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        sphereCollider = GetComponent<Collider>();
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = transform.position;
        plane.transform.Rotate(new Vector3(1, 0, 0), -90f);
        plane.transform.localScale = plane.transform.localScale * 5f;
        planeCollider = plane.GetComponent<Collider>();
        planeCollider.enabled = false;
        MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();
        meshRenderer.forceRenderingOff = true;

        startingScale = transform.localScale;
        startingPosition = transform.position;
        startingRotation = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void scale(float scaleFactor)
    {
        transform.localScale = startingScale * scaleFactor;
    }

    public void updateScale()
    {
        startingScale = transform.localScale;
    }

    public void rotate(float rotation)
    {
        throw new System.NotImplementedException();
    }

    public void resetPosition()
    {
        transform.position = startingPosition;
        transform.rotation = startingRotation;
    }
}
