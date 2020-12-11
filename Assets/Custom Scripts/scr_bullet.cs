using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_bullet : MonoBehaviour
{
    private int _Damage = 1;
    private float _speed = 200;
    private float _removeTime = 1f;
    private TrailRenderer _tr;
    private Canvas _HitmarkerCanvas;
    [SerializeField]
    private GameObject _Hitmarker;
    void Awake()
    {
        _tr = this.GetComponent<TrailRenderer>();
        GetComponent<Rigidbody>().AddForce(transform.forward * _speed);
        StartCoroutine(DestoryBullet());
        _HitmarkerCanvas = GameObject.FindGameObjectWithTag("Hitmarker").GetComponent<Canvas>();
    }

    void Update()
    {

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private IEnumerator DestoryBullet()
    {
        yield return new WaitForSeconds(_removeTime);
        GameObject.Destroy(gameObject);
    }



    private void HitSomething(GameObject go)
    {
        StopCoroutine(DestoryBullet());
        _tr.enabled = false;
        if (go.layer == 8)
        {
            StartCoroutine(DestoryBullet());
            scr_enemy_health eH = go.GetComponent<scr_enemy_health>();
            int healthLeft = eH.Damage(_Damage);
            if (healthLeft <= 0)
            {
                Vector3 point = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                GameObject marker = Instantiate(_Hitmarker, _HitmarkerCanvas.transform);
                marker.transform.position = point;
                marker.transform.parent = _HitmarkerCanvas.transform;
                Destroy(go);
            }


            _removeTime = 0;
        }

    }

    void OnCollisionEnter(Collision col)

    {
        HitSomething(col.gameObject);
    }

    // extra collision code
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2) && hit.transform.gameObject.layer == 8)
        {
            HitSomething(hit.transform.gameObject);
        }
        else if (Physics.Raycast(transform.position, transform.up * -1, out hit, 0.75f) && hit.transform.gameObject.tag != "bullet")
        {
            HitSomething(hit.transform.gameObject);
        }


    }

}
