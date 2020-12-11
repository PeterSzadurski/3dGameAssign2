using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_crosshair : MonoBehaviour
{
    private Vector3 _OriginalScale;
    private Vector3 _IncreasedScale;
    void Awake()
    {
        _OriginalScale = transform.localScale;
        _IncreasedScale = _OriginalScale;
        _IncreasedScale.x *= 1.5f;
        _IncreasedScale.y *= 1.5f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    public void IncreaseSize()
    {
        transform.localScale = _IncreasedScale;
    }
    public void OriginalSize()
    {
        transform.localScale = _OriginalScale;
    }
    void FixedUpdate()
    {
        gameObject.transform.position = Input.mousePosition;
    }
}
