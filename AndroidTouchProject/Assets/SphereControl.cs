using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereControl : MonoBehaviour, IControllable
{
    Renderer myRenderer;
    private bool isSelected = false;
    public void drag(List<Vector2> positions)
    {
        throw new System.NotImplementedException();
    }

    public bool selectToggle(bool selected)
    {
        isSelected = selected;
        if (isSelected)
        {
            myRenderer.material.color = Color.red;
        }
        else
        {
            myRenderer.material.color = Color.white;
        }
        return isSelected;
    }

    public void tap()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
