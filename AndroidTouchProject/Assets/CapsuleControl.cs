using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleControl : MonoBehaviour, IControllable
{
    Renderer myRenderer;
    private bool isSelected = false;
    Collider myCollider;

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
            //print("i hit something");
            transform.position = hitInfo.point;
        }
        else
        {
            //print("I hit nothing");
        }
        ColliderToggle();
    }

    public bool selectToggle(bool selected)
    {
        isSelected = selected;
        ColourUpdate();
        return isSelected;
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
        if (myCollider.enabled)
        {
            myCollider.enabled = false;
        }
        else
        {
            myCollider.enabled = true;
        }
    }

    public void tap(Vector2 position)
    {
        isSelected = !isSelected;
        ColourUpdate();
    }

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        myCollider = GetComponent<Collider>();

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
