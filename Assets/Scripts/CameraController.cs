using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour

{
    public Camera mainCam;
    public Camera fpsCam;
    public Camera tpsCam;

    private Toggle fpsCamToggle;
    private Toggle tpsCamToggle;

    private GameObject settingsPanel;

    // Start is called before the first frame update
    void Start()
    {
        fpsCamToggle = GameObject.FindWithTag("FPS").GetComponent<Toggle>();
        tpsCamToggle = GameObject.FindWithTag("TPS").GetComponent<Toggle>();

        settingsPanel = GameObject.FindWithTag("SettingsPanel");

        settingsPanel.SetActive(false);

        //make fps as default
        fpsCamToggle.isOn = true;
        tpsCamToggle.isOn = false;

        if (fpsCamToggle.isOn == true && tpsCamToggle.isOn == true)
        {
            mainCam.enabled = false;
            fpsCam.enabled = true;
            tpsCam.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
          if(fpsCamToggle.isOn == true && tpsCamToggle.isOn == false)
          {
                mainCam.enabled = false;
                fpsCam.enabled = true;
                tpsCam.enabled = false;
          }
          else if(tpsCamToggle.isOn == true && fpsCamToggle.isOn == false)
          {
                mainCam.enabled = false;
                fpsCam.enabled = false;
                tpsCam.enabled = true;
          }
    }  
}
