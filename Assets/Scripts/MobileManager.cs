using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileManager : MonoBehaviour
{
    [SerializeField]
    public GameObject mobileCanvas;

    // Start is called before the first frame update
    void Start()
    {

#if UNITY_ANDROID || UNITY_IOS
        mobileCanvas.SetActive(true);
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }
}
