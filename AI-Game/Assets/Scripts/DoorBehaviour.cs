using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorBehaviour : MonoBehaviour 
{
    private Transform initialPosition;

    private void Awake()
    {
        initialPosition = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>()){

        }
    }

    public void InitAnimation(){
        this.transform.DOMoveY(this.transform.position.y + 4f, 4f);
    }
    
    public void ResetDoor()
    {
        this.transform.DOMoveY(this.transform.position.y - 4f, 4f);
    }
}
