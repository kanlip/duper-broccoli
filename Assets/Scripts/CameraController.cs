using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour

{
    public GameObject fpsCam;
    public GameObject tpsCam;

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
            fpsCam.SetActive(true);
            tpsCam.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fpsCamToggle.isOn == true && tpsCamToggle.isOn == false)
        {
            fpsCam.SetActive(true);
            tpsCam.SetActive(false);
        }
        else if(tpsCamToggle.isOn == true && fpsCamToggle.isOn == false)
        {
            fpsCam.SetActive(false);
            tpsCam.SetActive(true);
        }
    }  
}
