using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_followMouse : MonoBehaviour
{

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.y - transform.position.y);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = mousePos;
    }
}
