using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallReset : MonoBehaviour {

    private Vector3 home;
	// Use this for initialization
	void Start () {
        home = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < -1f) {
            transform.position = home;
        }
	}
}
