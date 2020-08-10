using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    
    public float movementSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var rigidBody = GetComponent<Rigidbody>();

        float mvX = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        float mvZ = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        transform.Translate(mvX, 0, mvZ);


#if UNITY_ANDROID || UNITY_IOS

        float touchMvX = joystick.Horizontal * Time.deltaTime * movementSpeed; 
        float touchMvZ = joystick.Vertical * Time.deltaTime * movementSpeed; 
        transform.Translate(touchMvX, 0, touchMvZ);
#endif
    }
}
