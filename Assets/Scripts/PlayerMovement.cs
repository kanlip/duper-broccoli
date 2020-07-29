using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected Joystick joystick;
    protected AttackButtonManager attackButton;
    
    public float movementSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        attackButton = FindObjectOfType<AttackButtonManager>();
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
