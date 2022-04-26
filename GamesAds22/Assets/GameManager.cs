using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Text score;
    // Start is called before the first frame update
    void Start()
    {
        score = FindObjectOfType<Text>();
        score.text = "0";
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
        //Debug.DrawRay(tapRay.origin, tapRay.direction * 50, Color.red, 4f);
        RaycastHit hitInfo;
        if (Physics.Raycast(tapRay, out hitInfo))
        {
            if (hitInfo.transform.GetComponent<lb_Bird>() != null)
            {
                hitInfo.transform.GetComponent<lb_Bird>().KillBird();
                //IncrementScore(1);
            }
        }
    }

    public void IncrementScore(int value)
    {
        int scoreInt = Int32.Parse(score.text);
        scoreInt += value;
        score.text = scoreInt.ToString();
    }
}
