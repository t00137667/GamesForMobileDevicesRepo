using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour, IControllable
{
    //Collider collider;
    private bool isSelected = false;
    Renderer cubeRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        //collider = gameObject.GetComponent<Collider>();
        cubeRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool selectToggle()
    {
        isSelected = !isSelected;
        if (isSelected)
        {
            cubeRenderer.material.color = Color.red;
        }
        else
        {
            cubeRenderer.material.color = Color.white;
        }
        return isSelected;
    }
}
