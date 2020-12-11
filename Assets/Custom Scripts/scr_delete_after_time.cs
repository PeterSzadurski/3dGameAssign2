using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_delete_after_time : MonoBehaviour
{
    public float DeleteTime = 3f;
    void Start()
    {
        Destroy(gameObject, 5);   
    }

    

}
