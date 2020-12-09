using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_dashbar : MonoBehaviour
{
    private Image _Bar;
    void Awake() {
        _Bar = gameObject.GetComponent<Image>();
    }
    public void SetFill(float fill)
    {
        _Bar.fillAmount = fill;
    }
    public void SetColor(Color color)
    {
        _Bar.color = color;
    }
}
