using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_bullet : MonoBehaviour
{
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

    void OnTriggerEnter(Collider col)
    {
     //   Debug.Log("Trigger");
        HitSomething(col.gameObject);
    }

    private void HitSomething(GameObject go)
    {
        StopCoroutine(DestoryBullet());
        _tr.enabled = false;
        if (go.layer == 8)
        {


            Vector3 point = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            // Debug.Log("point: " + point);
            GameObject marker = Instantiate(_Hitmarker, _HitmarkerCanvas.transform);
            marker.transform.position = point;
            marker.transform.parent = _HitmarkerCanvas.transform;



            _removeTime = 0;
            StartCoroutine(DestoryBullet());
        }

    }

    void OnCollisionEnter(Collision col)

    {
       // Debug.Log("Not Trigger: " + col.gameObject.name);

        HitSomething(col.gameObject);
    }

    // extra collision code
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2) && hit.transform.gameObject.layer == 8)
        {
            Debug.Log(hit.transform.gameObject.name);
            HitSomething(hit.transform.gameObject);
        }


    }

}
