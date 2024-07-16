using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameObject player;
    PlayerMovement playerMovement;
    public Transform lastCheckpoint;

    public float fadeDuration = 4f;
    public Image fadePanel;
    public TextMeshProUGUI deathText;
    public AudioSource audioSource;

    private bool waitForInput = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();

        FadeInScreen(fadeDuration);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager when reloading
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SetCheckpoint(Transform checkpointPosition)
    {
        lastCheckpoint = checkpointPosition;
    }

    public Transform GetLastCheckpoint()
    {
        return lastCheckpoint;
    }

    public void RespawnPlayer()
    {
        if (lastCheckpoint == null)
        {
            Debug.LogWarning("No checkpoint set!");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("Player object not found!");
            return;
        }

        player.transform.position = lastCheckpoint.position;
        player.transform.rotation = lastCheckpoint.rotation;
    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void KillPlayer(string deathReason)
    {
        fadePanel.gameObject.SetActive(true);
        playerMovement?.disableMovement();
        audioSource.volume = .7f;
        audioSource.Play();
        StartCoroutine(FadeToDeathScreen(deathReason));
    }

    private IEnumerator FadeToDeathScreen(string deathReason)
    {
        yield return FadeOut(fadeDuration);

        // Show death text
        deathText.gameObject.SetActive(true);
        deathText.text = $"You died!\nCause of death: {deathReason}\n\nPress Enter to Respawn";
        waitForInput = true;

        while (waitForInput)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                waitForInput = false;
            }
            yield return null;
        }

        RespawnPlayer();
        playerMovement?.enableMovement();

        yield return FadeIn(fadeDuration);

        // Disable deathtext after fading back
        deathText.gameObject.SetActive(false);
    }

    public void FadeInScreen(float duration)
    {
        StartCoroutine(FadeIn(duration));
    }

    private IEnumerator FadeIn(float duration)
    {
        yield return Fade(1f, 0f, duration);
        fadePanel.gameObject.SetActive(false);
    }

    public void FadeOutScreen(float duration)
    {
        StartCoroutine(FadeOut(duration));
    }

    public IEnumerator FadeOut(float duration)
    {
        yield return Fade(0f, 1f, duration);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {

        fadePanel.gameObject.SetActive(true);
        float timer = 0f;
        Color color = Color.black;

        while (timer < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            color.a = alpha;
            fadePanel.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        fadePanel.color = color;
    }

}
