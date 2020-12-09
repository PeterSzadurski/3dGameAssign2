using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetector : MonoBehaviour {

    private void OnTriggerStay(Collider other) {
      //  Debug.Log(other.transform.tag);
        if (other.transform.tag == "Door") {
            other.transform.GetComponent<DoorController>().Open();
        }
    }
}
