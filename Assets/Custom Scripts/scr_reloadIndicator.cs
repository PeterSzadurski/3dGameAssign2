using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scr_reloadIndicator : MonoBehaviour
{
    private float _ReloadTime = 0f;
    private TextMeshProUGUI _AmmoText;
    private Image _Img;

    public void Awake()
    {
        _Img = GetComponent<Image>();
        _AmmoText = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<TextMeshProUGUI>();
    }

    public void StartReload(float reload, scr_ammo ammo)
    {
        _ReloadTime = reload;
        _Img.fillAmount = 0;
        StopAllCoroutines();
        ammo.ReloadStart();
        _AmmoText.text = ammo.GetAmmoInClip() + "/" + ammo.GetCurrentAmmoToString();
        StartCoroutine(Reload(ammo));
    }

    IEnumerator Reload(scr_ammo ammo)
    {
        float time = 0;
        while (time < _ReloadTime)
        {
            time += Time.deltaTime;
            float progress = Mathf.Lerp(0, 1, time / _ReloadTime);
            _Img.fillAmount = progress;
            yield return null;
        }
        ammo.ReloadEnd();
        _AmmoText.text = ammo.GetAmmoInClip() + "/" + ammo.GetCurrentAmmoToString();

        yield return new WaitForSeconds(0.1f);
        _Img.fillAmount = 0;
    }
}
