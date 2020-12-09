using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject player;
    private Vector3 offset;

    private void Awake()
    {
        player = GameObject.Find("Player");
        offset = player.transform.position - transform.position;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//        transform..LookAt(player.transform.position);
        Vector3 diff = player.transform.position - transform.position;
        diff -= offset;
//        diff.y = 0;
        Vector3 pos = transform.position;
        pos += diff / 10f;
        transform.position = pos;
	}
}
