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
    //camera left right
    //public float SENS_HOR = 3.0f;
    //public float SENS_VER = 2.0f;
    
    bool inited = false;
    float timeElapsed = 0;
    Camera cam;

    void Start()
    {
        if (photonView.IsMine)
        {
            //create pivot child to attach gameobject with this script
            GameObject pivotGO = new GameObject("Pivot");
            if (pivotGO)
            {
                pivotGO.transform.position = new Vector3(0, 2, 0);//(1, 1, -4);
                pivotGO.transform.rotation = Quaternion.identity;
                pivotGO.transform.SetParent(transform, false);
                cam = Camera.main;

                cam.transform.position = new Vector3(0, 0, -4);
                cam.transform.rotation = Quaternion.identity;
                cam.transform.SetParent(GameObject.Find("Pivot").GetComponent<Transform>(), false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (photonView.IsMine)
        //{
        //    var mouseMove = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        //    mouseMove = Vector2.Scale(mouseMove, new Vector2(SENS_HOR, SENS_VER));

        //    transform.Rotate(0, mouseMove.x, 0);

        //    cam.transform.Rotate(-mouseMove.y, 0, 0);

        //}

        // prevent cant find main cam crash for UnityStandardAssets.Cameras,
        // awake cant find cam in photon network
        if (!inited)
        {
            if (timeElapsed > 1.3f)
            {
                gameObject.AddComponent<FreeLookCam>();
                gameObject.AddComponent<ProtectCameraFromWallClip>();
                inited = true;
            }
            timeElapsed += Time.deltaTime;
        }
    }
}
