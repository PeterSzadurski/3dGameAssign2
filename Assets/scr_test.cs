using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_test : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 mousePos;
    Rigidbody rb;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        /*Vector3 positionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z - transform.position.z));
        Vector3 relativePos = mousePosition - transform.position;
        //Quaternion rot = Quaternion.LookRotation(relativePos);
        //float angle = Vector2.Angle(positionOnScreen, relativePos);
        //m_Spine.rotation = Quaternion.Euler(new Vector3(m_Spine.rotation.eulerAngles.x, angle*5, m_Spine.rotation.eulerAngles.z));
        transform.LookAt(mousePosition);*/
        /*  Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
          float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
          Quaternion rot = Quaternion.AngleAxis(angle -50, Vector3.down);
          transform.rotation = rot;
        */
        mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.y - transform.position.y);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.LookAt(mousePos);

    }

    void FixedUpdate()
    {
       /* Debug.Log("fixed update");
        Vector3 lookDir = (new Vector3(mousePos.x, this.transform.position.y, mousePos.z) - transform.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = (Quaternion.Euler(angle, angle, transform.rotation.eulerAngles.z));
       */
    }
    private void LateUpdate()
    {

    }
}
