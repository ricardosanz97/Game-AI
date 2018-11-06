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
    public List<DoorObject> levelDoors;
    public int LevelId;
    public Transform initialPlayerSpawnPosition;
    public Transform initialCameraSpawnPoint;
    public bool KeyTaken;

    public GameObject thePlayer;
    private GameObject player;
    public bool playerAlive = true;
    public bool levelCompleted = false;
    public string targetObject = "TargetObject";

    [SerializeField] private Image fadeImage;
    [SerializeField] private Text levelCompletedText;
    [SerializeField] private GameObject canvas;

    private void Awake()
    {
        GameManager.I.RestartLevel(this);
        canvas = HUDManager.I.canvas;
        fadeImage = HUDManager.I.FadeImageLevelCompleted;
        levelCompletedText = HUDManager.I.LevelCompletedText;
        if (LevelEnemies.Count == 0){
            Debug.LogError("No se ha asignado la variable LevelEnemies! Arrastralo en el inspector!");
        }
        if (levelDoors.Count == 0){
            Debug.LogError("No se ha asignado la variable LevelDoorss! Arrastralo en el inspector!");
        }
        if (levelKeys.Count == 0){
            Debug.LogError("No se ha asignado la variable LevelKeys! Arrastralo en el inspector!");
        }
    }

    public void SpawnPlayer()
    {
        HUDManager.I.ResetHUD();
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
            
            //s.AppendInterval(3f);
            
            s.AppendCallback(() =>
            {
                Camera.main.transform.DOMove(initialCameraSpawnPoint.transform.position, 2f);
                Camera.main.transform.DORotate(initialCameraSpawnPoint.rotation.eulerAngles, 2f);
                player.GetComponent<PlayerController>().SetDeadAnimatorParamenter();
                player.GetComponent<CharacterController>().enabled = true;
            });
            
            s.AppendCallback(() => {HUDManager.I.LoadingBar.enabled = true;});
            s.Append(HUDManager.I.FadeImage.DOFade(0f, 8f).SetEase(Ease.InBack));
            s.AppendCallback(() => {HUDManager.I.LoadingBar.enabled = false;});
            
            s.AppendInterval(1f);
            
            s.AppendCallback(() =>
            {
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
                Camera.main.transform.DOMove(initialCameraSpawnPoint.transform.position, 2f);
                Camera.main.transform.DORotate(initialCameraSpawnPoint.rotation.eulerAngles, 2f);
                player.GetComponent<CharacterController>().enabled = true;
            });

            s.AppendCallback(() => {HUDManager.I.LoadingBar.enabled = true;});
                        
            s.Append(HUDManager.I.FadeImage.DOFade(0f, 8f).SetEase(Ease.InBack));
            s.AppendCallback(() => {HUDManager.I.LoadingBar.enabled = false;});
            GameManager.I.playerSpawned = true;
;
            
            s.AppendInterval(0.5f);
            
            s.AppendCallback(() =>
            {
                player.GetComponent<PlayerController>().enabled = true;
            }); 
        
        }
       
    }

    public void PlayerDead()
    {
        
        for (int i = 0; i < LevelEnemies.Count; i++)
        {
            LevelEnemies[i].SetInitialState();
            Debug.Log(LevelEnemies[i].name + " resetado a " + LevelEnemies[i].currentState.stateName);
        }
        player.GetComponent<PlayerController>().SetDeadAnimatorParamenter();
        player.GetComponent<PlayerHealth>().PlayerDeath(this);

    }

    public void LevelCompleted()
    {
        // TODO enviar todos los enemigos al punto de inicio y activar las puertas y las llaves

        Sequence s = DOTween.Sequence();
        s.Append(fadeImage.DOFade(0f, 0f));
        s.Append(levelCompletedText.DOFade(0f, 0f));
        s.AppendInterval(3.5f);
        s.Append(fadeImage.DOFade(1f, 2f));
        s.AppendInterval(1f);
        s.Append(levelCompletedText.DOFade(1f, 1f));
        s.AppendInterval(1f);
        s.Append(levelCompletedText.DOFade(0f, 1f));
        s.OnComplete(() =>
        {
            GameManager.I.RestartLevel(this);
        });
 
    }
}
