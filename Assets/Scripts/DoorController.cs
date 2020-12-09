using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public float openSpeed = 0.1f;
    public float closeSpeed = 0.1f;
    public float openHeigh = 2;
    public float waitingTime = 0.1f;

    private Vector3 closePosition;
    private Vector3 openPosition;
    private bool opening;
    private float closedTime;

	// Use this for initialization
	void Start () {
        closePosition = transform.position;
        openPosition = closePosition;
        openPosition.y += openHeigh;
        opening = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 newPos = transform.position;
        if (opening) {
            newPos.y = Mathf.Min(openPosition.y, newPos.y + openSpeed);
            if (Vector3.Magnitude(openPosition - transform.position) <= 0.01f) {
                opening = false;
                closedTime = Time.time + waitingTime;
            }
        } else {
            if (Time.time >= closedTime) {
                newPos.y = Mathf.Max(closePosition.y, newPos.y - closeSpeed);
            }
        }
        transform.position = newPos;

    }

    public void Open () {
        opening = true;
    }

}
