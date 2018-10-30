using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class KeyBehaviour : MonoBehaviour {

    LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>().GetComponent<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>()){
            KeyObject key = levelManager.levelKeys.Find((x) => x.keyGameObject == this.gameObject);
            DoorObject door = levelManager.levelDoors.Find((x) => x.idKey == key.idDoor);
            door.doorGameObject.GetComponent<DoorBehaviour>().InitAnimation();

            GameObject keyGO = key.keyGameObject;
            keyGO.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 3f);
            keyGO.transform.DOMove(PlayerController.I.gameObject.transform.position, 4f).OnComplete(()=>{
                Destroy(keyGO);
            });
            /*
            Sequence s = DOTween.Sequence();
            s.Append(keyGO.transform.DORotate(new Vector3(0, 1, 0), 0.2f, RotateMode.Fast).SetLoops(-1));
            s.Append(keyGO.GetComponent<MeshRenderer>().material.DOFade(0f, 3f).SetLoops(-1));
            s.OnComplete(() =>
            {
                Destroy(keyGO);
            });
            */
        }
    }
}
