using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    //public GameObject cameraMovement;
    public Transform playerMovement;
    public float mouseSensitivity = 100.0f;
    private float rotateX = 0.0f;

    private float cameraAngle;
    private float cameraAngleSpeed = 100.0f;

    Camera tpsCamera;
    Camera fpsCamera;

    public Joystick camJoystick;


    // Start is called before the first frame update
    void Start()
    {
        tpsCamera = GameObject.FindWithTag("TPSCam").GetComponent<Camera>();
        fpsCamera = GameObject.FindWithTag("FPSCam").GetComponent<Camera>();
        //touchCamPanel = GameObject.FindWithTag("RotateCam");
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS

        float touchX = camJoystick.Horizontal * cameraAngleSpeed * Time.deltaTime;
        float touchY = camJoystick.Vertical * cameraAngleSpeed * Time.deltaTime;

         //rotateX -= touchY;
         //rotate up and down 90 degree
         //rotateX = Mathf.Clamp(rotateX, -90.0f, 90.0f);

         transform.localRotation = Quaternion.Euler(rotateX, 0.0f, 0.0f);
         playerMovement.Rotate(Vector3.up * touchX);
/*
         if (tpsCamera.enabled == true && fpsCamera.enabled == false)
          {
              rotateX -= touchY;
              //rotate up and down 90 degree
              rotateX = Mathf.Clamp(rotateX, -90.0f, 90.0f);

              transform.localRotation = Quaternion.Euler(rotateX, 0.0f, 0.0f);
              playerMovement.Rotate(Vector3.up * touchX);
          }
          else if (fpsCamera.enabled == true && tpsCamera.enabled == false)
          {

              rotateX -= touchY;
              //rotate up and down 90 degree
              rotateX = Mathf.Clamp(rotateX, -90.0f, 90.0f);

              transform.localRotation = Quaternion.Euler(rotateX, 0.0f, 0.0f);
              playerMovement.Rotate(Vector3.up * touchX);
          }  */
#else
//#if UNITY_STANDALONE
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        
        //rotateX -= mouseY;
        //rotate up and down 90 degree
        //rotateX = Mathf.Clamp(rotateX, -90.0f, 90.0f); 

        transform.localRotation = Quaternion.Euler(rotateX, 0.0f, 0.0f);
        playerMovement.Rotate(Vector3.up * mouseX);
#endif
    }
}
