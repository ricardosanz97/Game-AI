using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class KeyBehaviour : MonoBehaviour {

    LevelManager levelManager;
	private AudioSource audio;
	public AudioClip magic;

    private void Awake()
    {
		audio = GetComponent<AudioSource> ();
        levelManager = FindObjectOfType<LevelManager>().GetComponent<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>()){
            KeyObject key = levelManager.levelKeys.Find((x) => x.keyGameObject == this.gameObject);
            DoorObject door = levelManager.levelDoors.Find((x) => x.idKey == key.idDoor);
            door.doorGameObject.GetComponent<DoorBehaviour>().InitAnimation();

            GameObject keyGO = key.keyGameObject;

			audio.clip = magic;
			audio.Play();

            keyGO.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 1.5f);
            keyGO.transform.DOMove(PlayerController.I.gameObject.transform.position, 2f).OnComplete(()=>{
                Destroy(keyGO);

            });
        }
    }
}
