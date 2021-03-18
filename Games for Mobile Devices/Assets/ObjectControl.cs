using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl : MonoBehaviour, IControllable
{
    public bool IsSelected = false;
    Quaternion startRotation;
    Vector3 start_position;
    Vector3 startScale;
    Renderer ourRenderer;
    float maxScale = 2f;
    float minScale = 0.1f;

    public void Deselect()
    {
        IsSelected = false;
        Debug.Log("DESELECTED");
    }

    public void Drag(Vector2 position)
    {
        Vector3 pos = new Vector3(position.x, position.y, Vector3.Distance(start_position, Camera.main.transform.position));

        //Debug.Log("Dragged: " + pos.ToString());
        if (IsSelected)
        {
            if ((transform.position - Camera.main.ScreenToWorldPoint(pos)).magnitude < 1f)
            {
                transform.position = Camera.main.ScreenToWorldPoint(pos);
            }
        }
    }

    public void Hold(Vector2 position)
    {
        
        Debug.Log("Held");
        
    }

    public bool IsDraggable()
    {
        return true;
    }

    public bool IsHoldable()
    {
        throw new System.NotImplementedException();
    }

    public bool IsRotatable()
    {
        return true;
    }
    public bool IsScaleable()
    {
        return true;
    }

    public bool IsTappable()
    {
        return true;
    }

    public void ResetPosition()
    {
        transform.position = start_position;
        transform.rotation = startRotation;
        transform.localScale = startScale;
    }

    public void Rotate(float rotation)
    {
        //Debug.Log("Rotation Value: " + rotation);
        transform.Rotate(new Vector3(0, 0, rotation) * Time.deltaTime * 2f) ;
    }

    public void Scale(float scaleValue)
    {
        Vector3 scale = new Vector3(scaleValue, scaleValue, scaleValue) * 0.001f;
        if (transform.localScale.x >= minScale && transform.localScale.x <= maxScale)
        {
            transform.localScale += scale;
        }
        if (transform.localScale.x < minScale)
        {
            transform.localScale = new Vector3(minScale, minScale, minScale);
        }
        if (transform.localScale.x > maxScale)
        {
            transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        }
    }

    public void Tap()
    {
        IsSelected = !IsSelected;
    }

    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
        start_position = transform.position;
        startScale = transform.localScale;
        ourRenderer = transform.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSelected)
        {
            ourRenderer.material.color = Color.blue;
        }
        else
        {
            ourRenderer.material.color = Color.gray;
        }
        //if (Input.touchCount>0 && IsSelected)
        //{
        //    foreach (var touch in Input.touches)
        //    {
                
        //        if (touch.position.x > 360)
        //        {
        //            transform.position += Vector3.right * Time.deltaTime;
        //        }
        //        else if (touch.position.x < 360)
        //        {
        //            transform.position += -Vector3.right * Time.deltaTime;
        //        }
        //        if (touch.position.y > 600)
        //        {
        //            transform.position += Vector3.up * Time.deltaTime;
        //        }
        //        else if (touch.position.y < 600)
        //        {
        //            transform.position += -Vector3.up * Time.deltaTime;
        //        }
        //    }
        //}
    }
}
