using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchCameraMovement : MonoBehaviour
{
    Vector3 FirstPoint;
    Vector3 SecondPoint;
    float xAngle = 0.0f;
    float yAngle = 0.0f;
    float xAngleTemp;
    float yAngleTemp;

    Camera fpsCamera;
    Camera tpsCamera;

    GameObject playerMovement;

    RectTransform rectTranform;

    // Start is called before the first frame update
    void Start()
    {
        tpsCamera = GameObject.FindWithTag("TPSCam").GetComponent<Camera>();
        fpsCamera = GameObject.FindWithTag("FPSCam").GetComponent<Camera>();
        playerMovement = GameObject.FindWithTag("Player");
        rectTranform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        Touch myTouch = Input.GetTouch(0);
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                FirstPoint = Input.GetTouch(1).position;
                xAngleTemp = xAngle;
                yAngleTemp = yAngle;
            }
            if (Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                SecondPoint = Input.GetTouch(1).position;
                xAngle = xAngleTemp + (SecondPoint.x - FirstPoint.x) * 180 / Screen.width;
                yAngle = yAngleTemp + (SecondPoint.y - FirstPoint.y) * 90 / Screen.height;
                xAngle = Mathf.Clamp(xAngle, -90f, 90f);
                playerMovement.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
            }
        }       
#endif
    }

}
