using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            Cursor.visible = _IsPaused;

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

    public void Reset()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Application.Quit();
    }

}
