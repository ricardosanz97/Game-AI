using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            PlayerController.I.inCorrectWall = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            PlayerController.I.inCorrectWall = false;
        }
    }
}
