using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour, IControllable
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

    public void drag(List<Vector2> positions)
    {
        if (isSelected)
        {
            Vector2 lastPosition = positions[positions.Count-1];
            Vector3 pos = new Vector3(lastPosition.x, lastPosition.y, transform.position.z - Camera.main.transform.position.z );
            transform.position = Camera.main.ScreenToWorldPoint(pos);
        }
    }
}
