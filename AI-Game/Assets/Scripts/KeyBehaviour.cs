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
    private GameObject keyGO;

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

            keyGO = key.keyGameObject;

			audio.clip = magic;
			audio.Play();
            destructionPS.Play();

            gameObject.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 1.5f);
            gameObject.transform.DOMove(PlayerController.I.gameObject.transform.position, 2f).OnComplete(()=>{
                gameObject.SetActive(false);

            });
        }
    }
    
    public void ResetKey()
    {
        
        this.gameObject.SetActive(true);
        gameObject.transform.DOScale(new Vector3(0.5f,0.5f,0.5f), 0f);
        gameObject.transform.DOMove(initialPosition.position, 2f);
        

    }
}
