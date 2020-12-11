using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_enemy_health : MonoBehaviour
{
    public int EnemyId;

    [SerializeField]
    private AudioClip[] _Sounds;

    [SerializeField]
    private int _Health;

    private AudioSource _AS;

    [SerializeField]
    private GameObject _ExplodeSound;
    void Awake()
    {
        _AS = GetComponent<AudioSource>();

    }
    public int Damage(int dam)
    {
        _Health -= dam;

        if (_Health > 0)
        {
            _AS.PlayOneShot(_Sounds[0]);
        }
        else
        {
            GameObject go = GameObject.Instantiate(_ExplodeSound, transform);
            go.transform.parent = null;
        }
        return _Health;
    }
}
