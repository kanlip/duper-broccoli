using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour {

    public float SPEED = 1.0F;  // rotation speed
    private int stopAngle;
    private Vector3 direction;

	// Use this for initialization
	void Start () {
        stopAngle = (int)transform.eulerAngles.y; // stay in the current position
	}
	
	// Update is called once per frame
	void Update () {
        if ((int)transform.eulerAngles.y != stopAngle) // rotate untill current angle != stop angle
        {
            transform.Rotate(direction * SPEED);
        }

        if( Input.GetKeyDown(KeyCode.O)  ) 
        {
            stopAngle = 90;              // stop abgle 90
            direction = Vector3.up;  // direction around Y clock-wise
            // Debug.Log("O key pressed");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            stopAngle = 0;                // stop abgle 90
            direction = -Vector3.up;  // direction around Y counter clockwise
        }
    }
}
