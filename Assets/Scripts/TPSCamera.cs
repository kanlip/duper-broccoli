using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class TPSCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject pivotGO = new GameObject("Pivot");
        if (pivotGO)
        {
            pivotGO.transform.position = new Vector3(1,2,-3);
            pivotGO.transform.SetParent(transform, false);
        }
        Camera cam = Camera.main;
        cam.transform.position = new Vector3(0, 1, 0);
        cam.transform.SetParent(GameObject.Find("Pivot").GetComponent<Transform>(), false);

        gameObject.AddComponent<FreeLookCam>();
        gameObject.AddComponent<ProtectCameraFromWallClip>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
