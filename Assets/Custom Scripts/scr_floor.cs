using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_floor : MonoBehaviour
{
    void OnTriggerEnter(Collider col)

    {
        if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<scr_player>().ResetPosition();
        }
    }
}
