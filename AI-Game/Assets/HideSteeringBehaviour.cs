using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSteeringBehaviour : SteeringBehaviour
{
    public Transform home;
    public override void Act()
    {
        if (GetComponent<AssassinNPC>().inHome)
        {
            return;
        }
        this.transform.position = home.position;
        GetComponent<AssassinNPC>().inHome = true;
        GetComponent<AssassinNPC>().appeared = false;
    }
}
