using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_menu : MonoBehaviour
{
    [SerializeField]
    private GameObject _Panel;
    private bool _IsPaused = false;
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            _IsPaused = !_IsPaused;
            _Panel.SetActive(_IsPaused);
            if (_IsPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}
