using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public float playerHealth = 100f;

    [SerializeField] private GameObject canvas;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Image redFlash;
    private float maxPlayerHealth;

    private void Awake()
    {
        canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
        redFlash = canvas.transform.GetChild(4).GetComponent<Image>();
        playerHealthSlider = canvas.GetComponentInChildren<Slider>();
        fillImage = playerHealthSlider.transform.GetChild(1).GetComponentInChildren<Image>();
    }

    private void Start()
    {
        redFlash.gameObject.SetActive(false);
        maxPlayerHealth = playerHealth;
        playerHealthSlider.value = playerHealth;
    }

    public void ReceiveDamage(float value)
    {
        StartCoroutine(ReduceHealthSlider(playerHealth, playerHealth - value));
        StartCoroutine(RedFlash());
        playerHealth -= value;
        if (playerHealth <= 0)
        {
            GameController.I.playerAlive = false;
            GetComponent<CharacterController>().enabled = false;
        }
    }

    public void ResetPlayerHealth()
    {
        playerHealth = maxPlayerHealth;
        playerHealthSlider.value = maxPlayerHealth;
        fillImage.color = Color.green;
    }

    IEnumerator ReduceHealthSlider(float initialValue, float finalValue)
    {
        float lerpValue = 0f;
        Color currentColor = Color.Lerp(Color.red, Color.green, playerHealth / maxPlayerHealth);
        while (lerpValue < 1f)
        {
            lerpValue += Time.deltaTime;
            playerHealthSlider.value = Mathf.Lerp(initialValue, finalValue, lerpValue);
            fillImage.color = Color.Lerp(fillImage.color, currentColor, lerpValue); 
            yield return null;
        }
        yield return null;
    }

    IEnumerator RedFlash()
    {
        redFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        redFlash.gameObject.SetActive(false);
    }

}
