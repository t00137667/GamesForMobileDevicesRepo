using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            TouchPoint(Input.touches[0].position);
        }
    }

    void TouchPoint(Vector2 position)
    {
        Ray tapRay = Camera.main.ScreenPointToRay(position);
        Debug.DrawRay(tapRay.origin, tapRay.direction * 50, Color.red, 4f);
        RaycastHit hitInfo;
        if (Physics.Raycast(tapRay, out hitInfo))
        {
            if (hitInfo.transform.GetComponent<lb_Bird>() != null)
            {
                hitInfo.transform.GetComponent<lb_Bird>().KillBird();
            }
        }
    }
}
