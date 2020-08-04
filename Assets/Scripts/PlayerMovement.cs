using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    protected Joystick joystick;
    
    public float movementSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {

#if UNITY_ANDROID || UNITY_IOS
        joystick = FindObjectOfType<Joystick>();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        var rigidBody = GetComponent<Rigidbody>();

        float mvX = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        float mvZ = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        transform.Translate(mvX, 0, mvZ);


#if UNITY_ANDROID || UNITY_IOS

         rigidBody.velocity = new Vector3(joystick.Horizontal * movementSpeed, 
         rigidBody.velocity.y, joystick.Vertical * movementSpeed);

#endif
    }
}
