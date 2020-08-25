/* Start Header **************************************************************/
/*!
\file       TPSCamera.cs
\author     Eugene Lee Yuih Chin
\StudentNo  6572595
\par        xelycx@gmail.com
\date       25.8.2020
\brief

Reproduction or disclosure of this file or its contents
without the prior written consent of author is prohibited.
*/
/* End Header ****************************************************************/

using UnityEngine;
using UnityStandardAssets.Cameras;

using Photon.Pun;

public class TPSCamera : MonoBehaviourPunCallbacks
{
    Camera cam;

    GameObject pivotGO;
    GameObject rightJoystick;
    GameObject playerObject;

    void Awake()
    {
        // prevent cant find main cam crash for UnityStandardAssets.Cameras,
        // awake cant find cam in photon network
        // createMainCamera();
        if (photonView.IsMine)
        {
            //create pivot child to attach gameobject with this script
            pivotGO = new GameObject("Pivot");
            if (pivotGO)
            {
                pivotGO.transform.position = new Vector3(0, 2, 0);//(1, 1, -4);
                pivotGO.transform.rotation = Quaternion.identity;
                pivotGO.transform.SetParent(transform, false);
                cam = Camera.main;
                
             /* if (cam)
                {
                    cam.transform.position = new Vector3(0, 2, -4);
                    cam.transform.rotation = Quaternion.identity;
                    //cam.transform.SetParent(GameObject.Find("Pivot").GetComponent<Transform>(), false);
                    //cam.transform.SetParent(transform, false);
                }*/

#if UNITY_ANDROID || UNITY_IOS
                rightJoystick = GameObject.FindWithTag("RotateCam");
                playerObject = GameObject.FindWithTag("Player");            

                if (Camera.main && pivotGO)
                {
                    //gameObject.AddComponent<ProtectCameraFromWallClip>();
                }
#else
                if (Camera.main && pivotGO)
                {
                    gameObject.AddComponent<FreeLookCam>();

                   // gameObject.AddComponent<ProtectCameraFromWallClip>();
                }
#endif
            }
        }
    }


    private void Update()
    {
        float touchX = rightJoystick.GetComponent<FixedJoystick>().Horizontal * 5.0f;
        playerObject.transform.Rotate(0, touchX, 0);
    }

    void createMainCamera()
    {
        if (!Camera.main)
        {
            GameObject camera;
            camera = new GameObject("Main Camera");
            camera.AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.transform.position = new Vector3(0, 0, -4);
            camera.transform.rotation = Quaternion.identity;
        }
    }
}
