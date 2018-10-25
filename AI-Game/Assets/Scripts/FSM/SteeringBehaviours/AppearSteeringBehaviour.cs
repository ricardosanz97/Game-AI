using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AppearSteeringBehaviour : SteeringBehaviour
{
    public override void Act()
    {
        if (GetComponent<AssassinNPC>().appeared)
        {
            return;
        }

        int random = Random.Range(0, GetComponent<AssassinNPC>().assassinSpawnPoints.Count);
        Transform spawnPoint = GetComponent<AssassinNPC>().assassinSpawnPoints[random];

        GetComponent<AssassinNPC>().gameObject.transform.DOMove(spawnPoint.position, 0.01f);
        GetComponent<AssassinNPC>().appeared = true;
    }
}
