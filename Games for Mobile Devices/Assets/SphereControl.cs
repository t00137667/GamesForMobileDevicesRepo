using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereControl : MonoBehaviour, IControllable
{
    public bool IsSelected = false;
    Quaternion startRotation;
    Vector3 startPosition;
    Vector3 startScale;
    Renderer ourRenderer;
    float speed = 7.5f;
    public void Deselect()
    {
        IsSelected = false;
    }

    public void Drag(Vector2 position)
    {

    }

    public void Hold(Vector2 vector2)
    {
        
    }

    public bool IsDraggable()
    {
        return false;
    }

    public bool IsHoldable()
    {
        return false;
    }

    public bool IsRotatable()
    {
        return false;
    }

    public bool IsScaleable()
    {
        return false;
    }

    public bool IsTappable()
    {
        return true;
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.localScale = startScale;
        Deselect();
    }

    public void Rotate(float rotation)
    {
        throw new System.NotImplementedException();
    }

    public void Scale(float scaleValue)
    {
        throw new System.NotImplementedException();
    }

    public void Tap()
    {
        IsSelected = !IsSelected;
    }

    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
        startPosition = transform.position;
        startScale = transform.localScale;
        ourRenderer = transform.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSelected)
        {
            Vector3 dir = Vector3.zero;

            // we assume that device is held parallel to the ground
            // and Home button is in the right hand

            // remap device acceleration axis to game coordinates:
            //  1) XY plane of the device is mapped onto XZ plane
            //  2) rotated 90 degrees around Y axis
            dir.x = Input.acceleration.x;
            dir.z = Input.acceleration.y;

            // clamp acceleration vector to unit sphere
            if (dir.sqrMagnitude > 1)
                dir.Normalize();

            // Make it move 10 meters per second instead of 10 meters per frame...
            dir *= Time.deltaTime;

            // Move object
            transform.Translate(dir * speed);
        }
        if (IsSelected)
        {
            ourRenderer.material.color = Color.blue;
        }
        else
        {
            ourRenderer.material.color = Color.gray;
        }
    }
}
