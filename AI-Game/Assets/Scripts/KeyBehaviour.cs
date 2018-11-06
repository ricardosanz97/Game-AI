using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class KeyBehaviour : MonoBehaviour {

    LevelManager levelManager;
	private AudioSource audio;
    public ParticleSystem destructionPS;
	public AudioClip magic;
    private Transform initialPosition;

    private void Awake()
    {
		audio = GetComponent<AudioSource> ();
        destructionPS.Stop();
        levelManager = FindObjectOfType<LevelManager>().GetComponent<LevelManager>();
        initialPosition = transform;         
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
            destructionPS.Play();

            keyGO.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 1.5f);
            keyGO.transform.DOMove(PlayerController.I.gameObject.transform.position, 2f).OnComplete(()=>{
                Destroy(keyGO);

            });
        }
    }
    
    public void ResetKey()
    {
        transform.position = initialPosition.position;
    }
}
