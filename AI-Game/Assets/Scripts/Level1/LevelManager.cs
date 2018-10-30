using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public struct KeyObject{
    public GameObject keyGameObject;
    public int idDoor;
    public KeyObject(GameObject _go, int _id){
        this.keyGameObject = _go;
        this.idDoor = _id;
    }
}

[System.Serializable]
public struct DoorObject{
    public GameObject doorGameObject;
    public int idKey;
    public DoorObject (GameObject _go, int _id){
        this.doorGameObject = _go;
        this.idKey = _id;
    }
}

[DisallowMultipleComponent]
public class LevelManager : MonoBehaviour {

    public List<NPCStatesBehaviour> LevelEnemies;
    public List<KeyObject> levelKeys;
    private KeyBehaviour[] levelKeysGameObject;
    public List<DoorObject> levelDoors;
    private DoorBehaviour[] levelDoorsGameObject;
    public int LevelId;
    public Transform initialPlayerSpawnPosition;
    public Transform initialCameraSpawnPoint;
    public bool KeyTaken;

    public GameObject thePlayer;
    private GameObject player;
    public bool playerAlive = true;
    public bool levelCompleted = false;
    public string targetObject = "TargetObject";

    private void Awake()
    {
        GameManager.I.StarLevel(this);
    }

    private void Start()
    {
        /*
        levelKeysGameObject = FindObjectsOfType<KeyBehaviour>();
        levelDoorsGameObject = FindObjectsOfType<DoorBehaviour>();

        for (int i = 0; i < levelKeysGameObject.Length; i++){
            levelKeys.Add(new KeyObject(levelKeysGameObject[i].gameObject, i));
        }

        for (int i = 0; i < levelDoorsGameObject.Length; i++)
        {
            levelDoors.Add(new DoorObject(levelDoorsGameObject[i].gameObject, i));
        }
        */
    }

    public void SpawnPlayer()
    {        
        if (GameManager.I.playerSpawned)
        {
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() =>
            {
                player.GetComponent<PlayerHealth>().ResetPlayerHealth();
                player.transform.position = initialPlayerSpawnPosition.position;
                player.transform.rotation = initialPlayerSpawnPosition.rotation;

                Camera.main.GetComponent<vThirdPersonCamera>().enabled = true;
                Camera.main.GetComponent<vThirdPersonCamera>().target = player.transform.Find(targetObject);
                player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<PlayerController>().enabled = false;
                
            });
            s.Append(HUDManager.I.YouDiedImage.DOFade(0f, 0.001f));
            
            //s.AppendInterval(3f);
            
            s.AppendCallback(() =>
            {
                Camera.main.transform.DOMove(initialCameraSpawnPoint.transform.position, 2f);
                Camera.main.transform.DORotate(initialCameraSpawnPoint.rotation.eulerAngles, 2f);
                player.GetComponent<PlayerController>().SetDeadAnimatorParamenter();
            });
            s.AppendInterval(2f);
            s.AppendCallback(() =>
            {
                s.Append(HUDManager.I.FadeImage.DOFade(0f, 3f));

                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<PlayerController>().enabled = true;
            });
        }

        else
        {   
            Sequence s = DOTween.Sequence();
            Camera.main.GetComponent<vThirdPersonCamera>().enabled = true;
            player = Instantiate(thePlayer, initialPlayerSpawnPosition.position, initialPlayerSpawnPosition.rotation);
            Camera.main.GetComponent<vThirdPersonCamera>().target = player.transform.Find(targetObject);
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<CharacterController>().enabled = false;

            s.AppendCallback(() =>
            {
                HUDManager.I.FadeImage.DOFade(0f, 5f);
                GameManager.I.playerSpawned = true;
            });
            s.AppendCallback(() =>
            {
                Camera.main.transform.DOMove(initialCameraSpawnPoint.transform.position, 2f);
                Camera.main.transform.DORotate(initialCameraSpawnPoint.rotation.eulerAngles, 2f);
            });
            s.AppendInterval(2f);
            s.AppendCallback(() =>
            {
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<PlayerController>().enabled = true;
            }); 
        
        }
       
    }

    public void PlayerDead()
    {
        player.GetComponent<PlayerController>().SetDeadAnimatorParamenter();
        player.GetComponent<PlayerHealth>().PlayerDeath(this);
    }

    public void LevelCompleted()
    {
        SceneLoader.I.LoadScene(SceneLoader.SCENES.Menu);
    }
}
