using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_health : MonoBehaviour
{
    [SerializeField]
    private int _MaxShield;
    [SerializeField]
    private int _MaxHealth;

    private int _CurrShield;
    private int _CurrHealth;

    [SerializeField]
    private scr_hud_control _Hud;

    [SerializeField]
    private float _ShieldRechargeDelay;
    void Awake()
    {
        _CurrHealth = _MaxHealth;
        _CurrShield = _MaxShield;

        _Hud.Setup(_MaxHealth, _MaxShield);
    }

    public void Damage(int damage)
    {
        if (damage > _CurrShield)
        {
            damage -= _CurrShield;
            _CurrHealth -= damage;
            _CurrShield = 0;
        }
        else
        {
            _CurrShield -= damage;
        }
        _Hud.SetShield(_MaxShield, _CurrShield);
        _Hud.SetHealth(_MaxHealth, _CurrHealth);
        StopCoroutine(ShieldRecharge());
        StartCoroutine(ShieldRecharge());
    }

    private IEnumerator ShieldRecharge()
    {
        int tenPercent = (int)(_MaxShield * 0.05);
        yield return new WaitForSeconds(_ShieldRechargeDelay);


        while (_CurrShield < _MaxShield)
        {
            if ((int)(_CurrShield + Time.deltaTime * tenPercent) > _MaxShield)
            {

                _CurrShield = _MaxShield;

            }
            else
            {
                _CurrShield = (int)System.Math.Ceiling(_CurrShield + Time.deltaTime * tenPercent);

            }
            _Hud.SetShield(_MaxShield, _CurrShield);

            yield return null;

        }
        yield return null;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Damage(50);
        }
    }
}
