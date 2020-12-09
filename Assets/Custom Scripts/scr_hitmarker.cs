using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_hitmarker : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(DestroyHitMarker());
    }
    private IEnumerator DestroyHitMarker()
    {
        yield return new WaitForSeconds(0.2f);
        this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        yield return new WaitForSeconds(0.3f);
        GameObject.Destroy(this.gameObject);

    }
}
