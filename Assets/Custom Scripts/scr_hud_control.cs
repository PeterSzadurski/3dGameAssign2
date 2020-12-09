using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class scr_hud_control : MonoBehaviour
{
    [SerializeField]
    private RectTransform _Healthbar;
    [SerializeField]
    private RectTransform _Shieldbar;
    private float _HealthWidth;
    private float _ShieldWidth;

    [SerializeField]
    private Text _HealthText;
    [SerializeField]
    private Text _ShieldText;
    // Start is called before the first frame update
    void Start()
    {
        _HealthWidth = _Healthbar.rect.width;
        _ShieldWidth = _Shieldbar.rect.width;
    }

    // Update is called once per frame
    public void Setup(int maxH, int maxS)
    {
        _HealthText.text = maxH + " / " + maxH;
        _ShieldText.text = maxS + " / " + maxS;

    }

    public void SetHealth(int max, int current)
    {
        float perc = ((float)current / max);
       // if (perc == 1) { perc = 0; }
        _Healthbar.sizeDelta = new Vector2(_HealthWidth * perc, _Healthbar.rect.height);
        _HealthText.text = current + " / " + max;
    }
    public void SetShield(int max, int current)
    {
        float perc = ((float)current / max);
        //if (perc == 1) { perc = 0; }
        _Shieldbar.sizeDelta = new Vector2(_ShieldWidth * perc, _Shieldbar.rect.height);
        _ShieldText.text = current + " / " + max;
    }


}
