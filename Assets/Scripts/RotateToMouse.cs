using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    public Camera cam;
    public float maxLength = 50;

    private Ray mouseRay;
    private Vector3 pos;
    private Vector3 direction;
    private Quaternion rotation;
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam)
        {
            RaycastHit hit;
            Vector3 mousePosition = Input.mousePosition;
            mouseRay = cam.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(transform.position, transform.forward, out hit, maxLength))
            //if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit, maxLength))
            {
                //RotateToMouseDirection(gameObject, hit.point);
                gameObject.transform.LookAt(hit.point);
                //Debug.Log("hit");
            }
            else
            {
                pos = mouseRay.GetPoint(maxLength);
                //RotateToMouseDirection(gameObject, pos);
                gameObject.transform.LookAt(pos);
                //Debug.Log("hitmax");
            }
        }
        else { Debug.Log("Can't find camera"); }
    }

    void RotateToMouseDirection(GameObject obj, Vector3 destination)
    {
        direction = direction - obj.transform.position;
        rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }

    public Quaternion GetRotation()
    {
        return rotation;
    }
}
