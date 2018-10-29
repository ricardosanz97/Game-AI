using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public float playerHealth = 100f;

    [SerializeField] private GameObject canvas;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Image redFlash;
    [SerializeField] private Image youDiedImage;
    [SerializeField] private Text youDiedText;
    [SerializeField] private Image fadeImage;
    private float maxPlayerHealth;

    private void Awake()
    {
        canvas = HUDManager.I.canvas;
        redFlash = canvas.transform.GetChild(4).GetComponent<Image>();
        youDiedImage = HUDManager.I.YouDiedImage;
        youDiedText = canvas.transform.GetChild(3).GetComponent<Text>();
        playerHealthSlider = canvas.GetComponentInChildren<Slider>();
        fadeImage = HUDManager.I.FadeImage;
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
            GameManager.I.playerAlive = false;
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

    public void PlayerDeath(LevelManager levelWhereDied)
    {
        Sequence s = DOTween.Sequence();
        s.Append(youDiedImage.DOFade(1f, 1f));
        s.Append(youDiedText.DOFade(1f, 2f));
        s.AppendInterval(1f);
        s.Append(fadeImage.DOFade(1f, 2f));
        s.AppendInterval(1.5f);
        s.Append(youDiedText.DOFade(0f, 1f));
        s.OnComplete(() =>
        {
            GameManager.I.StarLevel(levelWhereDied);
        });
    }

}
