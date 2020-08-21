using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public float SENS_HOR = 3.0f;
    public float SENS_VER = 2.0f;
    GameObject character;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        character = this.transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseMove = new Vector2(Input.GetAxisRaw("Mouse X"),
                                    Input.GetAxisRaw("Mouse Y"));
        mouseMove = Vector2.Scale(mouseMove, new Vector2(SENS_HOR, SENS_VER));

        character.transform.Rotate(0, mouseMove.x, 0);
        transform.Rotate(-mouseMove.y, 0, 0);

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
    }
}
