using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ammo : MonoBehaviour
{
    [SerializeField]
    private int _AmmoInClip;
    [SerializeField]
    private int _ClipAmmo;
    [SerializeField]
    private int _CurrentAmmo;
    [SerializeField]
    private int _MaxAmmo;
    private bool _IsReloading = false;

    public void ReloadStart()
    {
        if (_CurrentAmmo != -1)
            _CurrentAmmo += _AmmoInClip;
        _AmmoInClip = 0;
        _IsReloading = true;
    }

    public void ReloadEnd()
    {
        if (_CurrentAmmo != -1)
        {
            if (_CurrentAmmo - _ClipAmmo > -1)
            {
                _AmmoInClip = _ClipAmmo;
                _CurrentAmmo -= _ClipAmmo;
            }
            else
            {
                _AmmoInClip = _CurrentAmmo;
                _CurrentAmmo = 0;
            }
        }
        else
        {
            _AmmoInClip = _ClipAmmo;
        }
        _IsReloading = false;

    }

    public int SubtractAmmo(int amount)
    {

        if (_AmmoInClip - amount > -1)
        {
            _AmmoInClip -= amount;
            return amount;
        }
        else
        {
            return 1;
        }

    }
    public bool GetIsReloading()
    {
        return _IsReloading;
    }
    public int GetAmmoInClip()
    {
        return _AmmoInClip;
    }
    public int GetClipAmmo()
    {
        return _ClipAmmo;

    }
    public int GetCurrentAmmo()
    {
        return _CurrentAmmo;
    }

    public string GetCurrentAmmoToString()
    {
        if (_CurrentAmmo == -1)
        {
            return ("∞");
        }
        else
        {
            return (_CurrentAmmo.ToString());
        }
    }
    public int GetMaxAmmo()
    {
        return _MaxAmmo;
    }
}
